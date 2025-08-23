import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';

export interface Me { userName: string; isAdmin: boolean; customerId: number; }
const LS_KEY = 'basicCreds';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private meSubject = new BehaviorSubject<Me | null>(null);
  me$ = this.meSubject.asObservable();

  constructor(private http: HttpClient) {
    // Try resume
    if (localStorage.getItem(LS_KEY)) {
      this.refreshMe().subscribe({ error: () => this.logout() });
    }
  }

  loginWithBasic(user: string, pass: string): Observable<Me> {
     const basic = 'Basic ' + btoa(`${user}:${pass}`);
    localStorage.setItem('basicCreds', basic);
    return this.refreshMe();                            // ✅ then call /auth/me
  }

  refreshMe(): Observable<Me> {
  
    return this.http
      .get<Me>(`${environment.apiBase}/auth/me`)       // NOTE: /auth, no /api
      .pipe(tap(m => this.meSubject.next(m)));
  }

  logout(): void { localStorage.removeItem(LS_KEY); this.meSubject.next(null); }
}
