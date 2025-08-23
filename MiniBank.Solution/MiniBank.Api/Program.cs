using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiniBank.Api.Data;
using MiniBank.Api.Infrastructure.Filters;
using MiniBank.Api.Infrastructure.Middleware;
using MiniBank.Api.Interfaces;
using MiniBank.Api.Repositories;
using MiniBank.Api.Services; 
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
var AllowWeb = "AllowWeb";

builder.Services.AddCors(o =>
{
    o.AddPolicy(AllowWeb, p => p
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});
// ✅ Register AuthService (needed by handler)
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddScoped<ValidateModelFilter>();
builder.Services.AddScoped<ExecutionTimeActionFilter>();


builder.Services.AddControllers(options =>
{
    // רישום פילטרים גלובלית
    options.Filters.Add<ValidateModelFilter>();
    options.Filters.Add<ExecutionTimeActionFilter>();
});

builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<RequestLoggingMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MiniBank API", Version = "v1" });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authentication"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("MiniBank")));

// ✅ Align scheme name to "Basic" everywhere
builder.Services
    .AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("Basic", null);

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// --- Seed/Upsert users so admin always works ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var adminHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd!");
    await db.Database.ExecuteSqlInterpolatedAsync($@"
        INSERT INTO Users (UserName, PasswordHash, IsAdmin, CustomerId)
        VALUES ({"admin"}, {adminHash}, 1, 1)
        ON CONFLICT(UserName) DO UPDATE SET
            PasswordHash = excluded.PasswordHash,
            IsAdmin = 1,
            CustomerId = 1;
    ");

    var userHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd!");
    await db.Database.ExecuteSqlInterpolatedAsync($@"
        INSERT INTO Users (UserName, PasswordHash, IsAdmin, CustomerId)
        VALUES ({"user1"}, {userHash}, 0, 1)
        ON CONFLICT(UserName) DO UPDATE SET
            PasswordHash = excluded.PasswordHash,
            IsAdmin = 0,
            CustomerId = 1;
    ");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors(AllowWeb);
// ✅ Order matters
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
