import { Component, input } from '@angular/core';

@Component({
  selector: 'app-loading-spinner',
  template: `<span class="spinner" [style.width.px]="size()" [style.height.px]="size()"></span>`,
  styles: `
    .spinner {
      display: inline-block;
      border: 2px solid rgba(0, 0, 0, 0.15);
      border-top-color: currentColor;
      border-radius: 50%;
      animation: spin 0.7s linear infinite;
      vertical-align: middle;
    }
    @keyframes spin { to { transform: rotate(360deg); } }
  `
})
export class LoadingSpinner {
  size = input(18);
}
