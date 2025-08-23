import { createFeatureSelector, createReducer, on } from '@ngrx/store';
import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { Tx, TxQuery } from '../../../core/models';
import * as T from './tx.actions';

export interface TxState extends EntityState<Tx> { total: number; query: TxQuery; loading: boolean; error?: string; }
export const txAdapter = createEntityAdapter<Tx>({ selectId: t => t.id });
const initial: TxState = txAdapter.getInitialState({ total: 0, loading: false, query: { page:1, pageSize:20 } });

export const txReducer = createReducer(initial,
  on(T.setQuery, (s,{query}) => ({...s, query: {...s.query, ...query}})),
  on(T.loadTx, (s) => ({...s, loading:true})),
  on(T.loadTxSuccess, (s, { items, total }) =>
  txAdapter.setAll(items ?? [], {
    ...s,
    total: typeof total === 'number' ? total : (items?.length ?? 0),
    loading: false,
    error: undefined
  })
),
  on(T.loadTxFailure, (s,{error}) => ({...s, loading:false, error}))
);
export const selectTxState = createFeatureSelector<TxState>('tx');

// ✅ Bind adapter selectors
const _txSel = txAdapter.getSelectors(selectTxState);
export const selectAllTx   = _txSel.selectAll;
export const selectTxIds   = _txSel.selectIds;
export const selectTxTotal = (state: any) => selectTxState(state).total;
export const selectTxQuery = (state: any) => selectTxState(state).query;