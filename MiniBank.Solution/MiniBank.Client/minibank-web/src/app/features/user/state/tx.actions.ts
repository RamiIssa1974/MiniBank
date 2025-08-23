import { createAction, props } from '@ngrx/store';
import { Tx, TxType, TxQuery } from '../../../core/models';

export const setQuery = createAction('[Tx] Set Query', props<{query: Partial<TxQuery>}>());
export const loadTx = createAction('[Tx] Load');
export const loadTxSuccess = createAction('[Tx] Load Success', props<{items: Tx[]; total: number}>());
export const loadTxFailure = createAction('[Tx] Load Failure', props<{error: string}>());

export const doDeposit = createAction('[Tx] Deposit', props<{accountId:number; amount:number}>());
export const doWithdraw = createAction('[Tx] Withdraw', props<{accountId:number; amount:number}>());
export const doTransfer = createAction('[Tx] Transfer', props<{
    fromAccountId:number; 
    toAccountId:string; 
    amount:number}>());
