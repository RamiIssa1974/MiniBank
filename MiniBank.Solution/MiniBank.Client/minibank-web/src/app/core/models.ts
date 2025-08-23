export interface Me { userName: string; isAdmin: boolean; customerId: number; }
export interface Account { id: number; iban: string; balance: number; isLocked: boolean; }
export type TxType = 'Deposit'|'Withdraw'|'TransferIn'|'TransferOut';
export interface Tx { id: number; accountId: number; type: TxType; amount: number; createdAt: string; }
export interface Paged<T> { items: T[]; total: number; }
export interface TxQuery { accountId?: number; page: number; pageSize: number; type?: TxType; from?: string; to?: string; }
