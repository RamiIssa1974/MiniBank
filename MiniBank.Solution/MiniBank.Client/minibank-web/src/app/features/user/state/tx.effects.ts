import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as T from './tx.actions';
import * as A from '../state/accounts.actions';
import { TxApi } from '../../../core/api/tx.api';
import { Store } from '@ngrx/store';
import { catchError, map, of, switchMap, withLatestFrom, tap } from 'rxjs';

@Injectable()
export class TxEffects {
  load$ = createEffect(() => this.actions$.pipe(
  ofType(T.loadTx),
  withLatestFrom(this.store.select(s => s.tx.query)),
  switchMap(([_, q]) => this.api.getPaged(q).pipe(
  // if your PagedResult uses different property names (e.g., totalCount),
  // normalize them here:
  map((res: any) => {
    const items = res?.items ?? [];
    const total = res?.total ?? res?.totalCount ?? items.length;
    return T.loadTxSuccess({ items, total });
  }),
  catchError(e => of(T.loadTxFailure({ error: e?.message ?? 'Failed to load transactions' })))
))

));

  deposit$ = createEffect(() => this.actions$.pipe(
    ofType(T.doDeposit),
    switchMap(a => this.api.deposit(a).pipe(
      tap(() => console.log('Deposit done')), // אפשר להחליף ב-MatSnackBar
      switchMap(() => of(T.loadTx(), A.loadMyAccounts())),
      catchError(e => of(T.loadTxFailure({error: e?.message ?? 'Deposit failed'})))
    ))
  ));

  withdraw$ = createEffect(() => this.actions$.pipe(
    ofType(T.doWithdraw),
    switchMap(a => this.api.withdraw(a).pipe(
      tap(() => console.log('Withdraw done')),
      switchMap(() => of(T.loadTx(), A.loadMyAccounts())),
      catchError(e => of(T.loadTxFailure({error: e?.message ?? 'Withdraw failed'})))
    ))
  ));

  transfer$ = createEffect(() => this.actions$.pipe(
    ofType(T.doTransfer),
    switchMap(a => this.api.transfer(a).pipe(
      tap(() => console.log('Transfer done')),
      switchMap(() => of(T.loadTx(), A.loadMyAccounts())),
      catchError(e => of(T.loadTxFailure({error: e?.message ?? 'Transfer failed'})))
    ))
  ));

  constructor(private actions$: Actions, 
    private api: TxApi, 
    private store: Store<any>) {}
}
