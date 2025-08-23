import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptor } from './core/auth/auth.interceptor'; // adjust path if needed
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { registerLocaleData } from '@angular/common';
import localeHe from '@angular/common/locales/he';
import localeHeExtra from '@angular/common/locales/extra/he';

registerLocaleData(localeHe, 'he-IL', localeHeExtra);

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: LOCALE_ID, useValue: 'he-IL' },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
