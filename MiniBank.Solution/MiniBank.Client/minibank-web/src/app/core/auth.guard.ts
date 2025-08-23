import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { catchError, map, of } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private http: HttpClient, private router: Router) {}

  canActivate() {
    return this.http.get(`${environment.apiBase}/auth/me`).pipe(
      map(() => true),
      catchError(() => of(this.router.parseUrl('/login') as UrlTree))
    );
  }
}
