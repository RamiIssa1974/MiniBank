import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

export interface Account { id: number; iban: string; balance: number; isLocked: boolean; }

@Component({
  selector: 'app-transaction-form',
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TransactionFormComponent {  
  @Input() accounts: Account[] | null = [];
  @Output() deposit  = new EventEmitter<{accountId:number; amount:number}>();
  @Output() withdraw = new EventEmitter<{accountId:number; amount:number}>();
  @Output() transfer = new EventEmitter<{fromAccountId:number; toAccountId:string; amount:number}>();

  form = this.fb.group({
    op: this.fb.control<'deposit'|'withdraw'|'transfer'>('deposit', { nonNullable:true }),
    accountId: this.fb.control<number|null>(null, { validators:[Validators.required] }),
    amount: this.fb.control<number>(0, { validators:[Validators.required, Validators.min(0.01)] }),
    toIban: this.fb.control<string>(''),
  });

  constructor(private fb: FormBuilder) {}

  submit(){
    const v = this.form.getRawValue();
    if (v.op === 'deposit')  this.deposit.emit({ accountId: v.accountId!, amount: v.amount! });
    if (v.op === 'withdraw') this.withdraw.emit({ accountId: v.accountId!, amount: v.amount! });
    if (v.op === 'transfer') this.transfer.emit({ 
      fromAccountId: v.accountId!,
      toAccountId: v.toIban!, 
      amount: v.amount! });
  }
}
