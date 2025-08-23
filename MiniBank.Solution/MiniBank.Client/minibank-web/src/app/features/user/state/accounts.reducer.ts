import { createFeatureSelector, createReducer, on } from '@ngrx/store';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { Account } from '../../../core/models';
import * as A from './accounts.actions';

export interface AccountsState extends EntityState<Account> { loaded: boolean; error?: string; }
export const adapter = createEntityAdapter<Account>({ selectId: a => a.id });
const initial = adapter.getInitialState({ loaded: false });

export const accountsReducer = createReducer(initial,
  on(A.loadMyAccountsSuccess, (s,{accounts}) => adapter.setAll(accounts, {...s, loaded:true, error:undefined})),
  on(A.loadMyAccountsFailure, (s,{error}) => ({...s, error}))
);
export const selectAccountsState = createFeatureSelector<AccountsState>('accounts');

// ✅ Bind adapter selectors to the feature slice
const _selectors = adapter.getSelectors(selectAccountsState);
export const selectAllAccounts   = _selectors.selectAll;
export const selectAccountsTotal = _selectors.selectTotal;
export const selectAccountEntities = _selectors.selectEntities;