import { createAction, props } from '@ngrx/store';
import { Account } from '../../../core/models';
export const loadMyAccounts = createAction('[Accounts] Load My');
export const loadMyAccountsSuccess = createAction('[Accounts] Load My Success', props<{accounts: Account[]}>());
export const loadMyAccountsFailure = createAction('[Accounts] Load My Failure', props<{error: string}>());
