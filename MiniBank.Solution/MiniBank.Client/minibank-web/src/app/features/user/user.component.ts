// src/app/features/user/user.component.ts
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Store } from '@ngrx/store';
import * as A from './state/accounts.actions';
import * as Tx from './state/tx.actions';
import { selectAllTx, selectTxTotal } from './state/tx.reducer';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { selectAllAccounts } from './state/accounts.reducer';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserComponent {
  accounts$ = this.store.select(selectAllAccounts);
  tx$ = this.store.select(selectAllTx);
  total$ = this.store.select(selectTxTotal);
  user$ = this.auth.me$;

  constructor(private store: Store, private auth: AuthService, private router: Router) { }

  ngOnInit() {
    this.store.dispatch(A.loadMyAccounts());
    this.accounts$.subscribe(accs => {
      if (accs && accs.length) {
        this.onSelectAccount(accs[0].id);
      }
    });
    this.store.dispatch(Tx.setQuery({ query: { page: 1, pageSize: 20 } }));
    this.store.dispatch(Tx.loadTx());
  }

  onLogout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
  onPageChange(page: number) {
    this.store.dispatch(Tx.setQuery({ query: { page } }));
    this.store.dispatch(Tx.loadTx());
  }

  onSelectAccount(accountId: number) {
    this.store.dispatch(Tx.setQuery({ query: { accountId, page: 1 } }));
    this.store.dispatch(Tx.loadTx());
  }
  onDeposit(p: { accountId: number; amount: number }) {
    this.store.dispatch(Tx.doDeposit(p));
  }
  onWithdraw(p: { accountId: number; amount: number }) {
    this.store.dispatch(Tx.doWithdraw(p));
  }
  onTransfer(p: { fromAccountId: number; toAccountId: string; amount: number }) {
    console.log('Transfer requested: ', p);
    this.store.dispatch(Tx.doTransfer(p));
  }
}
