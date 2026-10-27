import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PurchaseBillItem, SavePurchaseBillResponse } from '../models/purchase-bill.model';

@Injectable({ providedIn: 'root' })
export class PurchaseBillService {
  private http = inject(HttpClient);

  save(items: PurchaseBillItem[]): Observable<SavePurchaseBillResponse> {
    return this.http.post<SavePurchaseBillResponse>(`${environment.apiUrl}/purchase-bills`, { items });
  }
}
