import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Location } from '../models/location.model';

@Injectable({ providedIn: 'root' })
export class LocationService {
  private http = inject(HttpClient);

  getAll(): Observable<Location[]> {
    return this.http.get<Location[]>(`${environment.apiUrl}/locations`);
  }
}
