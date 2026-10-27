import { Component, input } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-item-summary',
  imports: [DecimalPipe],
  templateUrl: './item-summary.html',
  styleUrl: './item-summary.css'
})
export class ItemSummary {
  totalItems = input(0);
  totalQty = input(0);
  totalCost = input(0);
  totalSelling = input(0);
}
