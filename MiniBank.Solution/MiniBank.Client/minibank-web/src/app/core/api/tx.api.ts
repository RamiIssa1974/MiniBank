import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Paged, Tx, TxQuery } from '../models';
import { environment } from 'src/environments/environment.development';

export type TxType = 'Deposit'|'Withdraw'|'TransferIn'|'TransferOut';

@Injectable({ providedIn: 'root' })
export class TxApi {
  constructor(private http: HttpClient) {}
  getPaged(q: TxQuery) {
  return this.http.get<Paged<Tx>>(
    `${environment.apiBase}/transactions`,
    { params: q as any }
  );
  }
  deposit(p: {accountId:number; amount:number}) { 
    return this.http.post(`${environment.apiBase}/transactions/deposit`, p); 
  }
  withdraw(p: {accountId:number; amount:number}) { 
    return this.http.post(`${environment.apiBase}/transactions/withdraw`, p); 
  }
  transfer(p: {fromAccountId:number; toAccountId:string; amount:number}) { 
    return this.http.post(`${environment.apiBase}/transactions/transfer`, p); 
  }
}
