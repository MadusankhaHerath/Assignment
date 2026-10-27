import { HttpErrorResponse } from '@angular/common/http';

export function getErrorMessage(err: HttpErrorResponse, fallback: string): string {
  if (err.status === 0) return 'Cannot connect to the server. Please try again later.';
  return err.error?.message ?? fallback;
}
