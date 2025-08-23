// src/app/features/user/state/accounts.effects.ts
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as A from './accounts.actions';
import * as T from './tx.actions'; // ⬅️ add
import { AccountApi } from '../../../core/api/account.api';
import { catchError, map, of, switchMap, tap } from 'rxjs';

@Injectable()
export class AccountsEffects {
  load$ = createEffect(() => this.actions$.pipe(
    ofType(A.loadMyAccounts),
    switchMap(() => this.api.getMy().pipe(
      map(accounts => A.loadMyAccountsSuccess({ accounts })),
      catchError(e => of(A.loadMyAccountsFailure({ error: e?.message ?? 'Failed to load accounts' })))
    ))
  ));

  // ⬇️ When accounts load, pick first account and load transactions
  // selectFirstAndLoadTx$ = createEffect(() => this.actions$.pipe(
  //   ofType(A.loadMyAccountsSuccess),
  //   tap(({ accounts }) => console.log('[accounts] loaded:', accounts.length)),
  //   switchMap(({ accounts }) => {
  //     const first = accounts?.[0];
  //     if (!first) return of({ type: '[Tx] No Accounts' } as any);
  //     return of(
  //       T.setQuery({ query: { accountId: first.id, page: 1 } }),
  //       T.loadTx()
  //     );
  //   })
  // ));

  constructor(private actions$: Actions, private api: AccountApi) {}
}
