import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { accountsReducer } from './state/accounts.reducer';
import { AccountsEffects } from './state/accounts.effects';
import { txReducer } from './state/tx.reducer';
import { TxEffects } from './state/tx.effects';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { AccountListComponent } from './components/account-list/account-list.component';
import { TransactionFormComponent } from './components/transaction-form/transaction-form.component';
import { TransactionTableComponent } from './components/transaction-table/transaction-table.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    UserComponent,
    AccountListComponent,
    TransactionFormComponent,
    TransactionTableComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule, 
    UserRoutingModule,
    StoreModule.forFeature('accounts', accountsReducer),
    StoreModule.forFeature('tx', txReducer),
    EffectsModule.forFeature([AccountsEffects, TxEffects]),
  ]
})
export class UserModule { }
