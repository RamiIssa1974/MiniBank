import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

export type TxType = 'Deposit'|'Withdraw'|'TransferIn'|'TransferOut';
export interface Tx { id:number; accountId:number; type:TxType; amount:number; createdAt:string; }

@Component({
  selector: 'app-transaction-table',
  templateUrl: './transaction-table.component.html',
  styleUrls: ['./transaction-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TransactionTableComponent {
  @Input() items: Tx[] | null = null;
  @Input() total: number | null = 0;
  @Output() pageChange = new EventEmitter<number>();

  page = 1;
  readonly pageSize = 20;

  next(){ this.pageChange.emit(++this.page); }
  prev(){ if(this.page>1) this.pageChange.emit(--this.page); }
  disableNext(){ return (this.total ?? 0) <= this.page * this.pageSize; }
}
