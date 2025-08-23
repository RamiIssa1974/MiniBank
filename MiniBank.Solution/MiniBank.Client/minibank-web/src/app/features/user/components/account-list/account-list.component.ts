import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

export interface Account {
  id: number; iban: string; balance: number; isLocked: boolean;
}

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AccountListComponent {
  @Input() accounts: Account[] | null = null;
  @Output() select = new EventEmitter<number>();
  mask(iban: string){ return iban ? iban.slice(0,4)+'****'+iban.slice(-4) : ''; }
}
