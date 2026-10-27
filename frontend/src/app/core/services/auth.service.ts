import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse, Session } from '../models/auth.model';

const SESSION_KEY = 'session';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private session = signal<Session | null>(this.readSession());

  readonly email = computed(() => this.session()?.email ?? '');

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap(res => {
        const session: Session = { token: res.token, email: res.email };
        sessionStorage.setItem(SESSION_KEY, JSON.stringify(session));
        this.session.set(session);
      })
    );
  }

  logout(): void {
    sessionStorage.removeItem(SESSION_KEY);
    this.session.set(null);
  }

  get token(): string | null {
    return this.session()?.token ?? null;
  }

  isLoggedIn(): boolean {
    const token = this.token;
    if (!token) return false;

    try {
      const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
      const payload = JSON.parse(atob(base64));
      return payload.exp * 1000 > Date.now();
    } catch {
      return false;
    }
  }

  private readSession(): Session | null {
    const raw = sessionStorage.getItem(SESSION_KEY);
    return raw ? JSON.parse(raw) : null;
  }
}
