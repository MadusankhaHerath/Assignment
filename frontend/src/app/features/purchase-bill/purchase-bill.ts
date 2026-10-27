import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { toSignal } from '@angular/core/rxjs-interop';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DecimalPipe } from '@angular/common';
import { finalize } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { LocationService } from '../../core/services/location.service';
import { PurchaseBillService } from '../../core/services/purchase-bill.service';
import { Location } from '../../core/models/location.model';
import { PurchaseBillItem } from '../../core/models/purchase-bill.model';
import { getErrorMessage } from '../../core/error-message';
import { LoadingSpinner } from '../../shared/loading-spinner/loading-spinner';
import { ItemTable } from './item-table/item-table';
import { ItemSummary } from './item-summary/item-summary';

const ITEM_OPTIONS = ['Mango', 'Apple', 'Banana', 'Orange', 'Grapes', 'Kiwi', 'Strawberry'];

function itemInList(control: AbstractControl): ValidationErrors | null {
  return !control.value || ITEM_OPTIONS.includes(control.value) ? null : { notInList: true };
}

@Component({
  selector: 'app-purchase-bill',
  imports: [ReactiveFormsModule, DecimalPipe, LoadingSpinner, ItemTable, ItemSummary],
  templateUrl: './purchase-bill.html',
  styleUrl: './purchase-bill.css'
})
export class PurchaseBill implements OnInit {
  private auth = inject(AuthService);
  private router = inject(Router);
  private locationService = inject(LocationService);
  private purchaseBillService = inject(PurchaseBillService);

  readonly itemOptions = ITEM_OPTIONS;
  readonly email = this.auth.email;

  locations = signal<Location[]>([]);
  items = signal<PurchaseBillItem[]>([]);
  loadingLocations = signal(false);
  saving = signal(false);
  error = signal('');
  success = signal('');

  form = inject(FormBuilder).group({
    item: ['', [Validators.required, itemInList]],
    batch: ['', Validators.required],
    standardCost: [null as number | null, [Validators.required, Validators.min(0)]],
    standardPrice: [null as number | null, [Validators.required, Validators.min(0)]],
    qty: [null as number | null, [Validators.required, Validators.min(0.01)]],
    freeQty: [0 as number | null, Validators.min(0)],
    discount: [0 as number | null, [Validators.min(0), Validators.max(100)]]
  });

  private values = toSignal(this.form.valueChanges, { initialValue: this.form.getRawValue() });

  margin = computed(() => (this.values().standardPrice ?? 0) - (this.values().standardCost ?? 0));
  totalCost = computed(() => {
    const v = this.values();
    return (v.standardCost ?? 0) * (v.qty ?? 0) * (1 - (v.discount ?? 0) / 100);
  });
  totalSelling = computed(() => (this.values().standardPrice ?? 0) * (this.values().qty ?? 0));

  billTotalQty = computed(() => this.items().reduce((sum, i) => sum + i.qty, 0));
  billTotalCost = computed(() => this.items().reduce((sum, i) => sum + i.totalCost, 0));
  billTotalSelling = computed(() => this.items().reduce((sum, i) => sum + i.totalSelling, 0));

  ngOnInit(): void {
    this.loadingLocations.set(true);
    this.locationService.getAll()
      .pipe(finalize(() => this.loadingLocations.set(false)))
      .subscribe({
        next: locations => this.locations.set(locations),
        error: (err: HttpErrorResponse) => this.error.set(getErrorMessage(err, 'Failed to load batches.'))
      });
  }

  addItem(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const v = this.form.getRawValue();
    const item: PurchaseBillItem = {
      item: v.item!,
      batch: v.batch!,
      standardCost: v.standardCost!,
      standardPrice: v.standardPrice!,
      margin: this.margin(),
      qty: v.qty!,
      freeQty: v.freeQty ?? 0,
      discount: v.discount ?? 0,
      totalCost: this.totalCost(),
      totalSelling: this.totalSelling()
    };

    this.items.update(items => [...items, item]);
    this.form.reset({ item: '', batch: '', standardCost: null, standardPrice: null, qty: null, freeQty: 0, discount: 0 });
    this.success.set('');
  }

  removeItem(index: number): void {
    this.items.update(items => items.filter((_, i) => i !== index));
  }

  save(): void {
    this.error.set('');
    this.success.set('');

    if (this.items().length === 0) {
      this.error.set('Add at least one item before saving.');
      return;
    }

    this.saving.set(true);
    this.purchaseBillService.save(this.items())
      .pipe(finalize(() => this.saving.set(false)))
      .subscribe({
        next: res => {
          this.success.set(`${res.message} (Bill #${res.id})`);
          this.items.set([]);
        },
        error: (err: HttpErrorResponse) => this.error.set(getErrorMessage(err, 'Failed to save purchase bill.'))
      });
  }

  reset(): void {
    this.form.reset({ item: '', batch: '', standardCost: null, standardPrice: null, qty: null, freeQty: 0, discount: 0 });
    this.items.set([]);
    this.error.set('');
    this.success.set('');
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  invalid(field: keyof typeof this.form.controls): boolean {
    const control = this.form.controls[field];
    return control.touched && control.invalid;
  }
}
