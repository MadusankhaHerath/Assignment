import { Component, input, output } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { PurchaseBillItem } from '../../../core/models/purchase-bill.model';

@Component({
  selector: 'app-item-table',
  imports: [DecimalPipe],
  templateUrl: './item-table.html',
  styleUrl: './item-table.css'
})
export class ItemTable {
  items = input.required<PurchaseBillItem[]>();
  remove = output<number>();
}
