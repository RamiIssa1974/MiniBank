import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Account } from '../models';
import { environment } from 'src/environments/environment.development';

@Injectable({ providedIn: 'root' })
export class AccountApi {
  constructor(private http: HttpClient) {}
  getMy() { return this.http.get<Account[]>(`${environment.apiBase}/accounts/my`); }
}
