import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent {
  loginError = '';
  loading = false;

  // Typed reactive form (Angular 14+ typed forms)
  loginForm = this.fb.nonNullable.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {}

  onSubmit(): void {
    this.loginError = '';
    if (this.loginForm.invalid) return;

    const { username, password } = this.loginForm.getRawValue();

    // store Basic credentials for the interceptor
    const base64 = btoa(`${username}:${password}`);
    localStorage.setItem('basicCreds', base64);

    this.loading = true;
    const headers = new HttpHeaders({ Authorization: `Basic ${base64}` });

    // sanity-check credentials by calling the protected echo endpoint
    this.http.get(`${environment.apiBase}/auth/me`, { headers }).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/dashboard']); // or '/dashboard' if you add that route
      },
      error: () => {
        this.loading = false;
        this.loginError = 'Login failed. Check username/password.';
        localStorage.removeItem('basicCreds');
      }
    });
  }
}
