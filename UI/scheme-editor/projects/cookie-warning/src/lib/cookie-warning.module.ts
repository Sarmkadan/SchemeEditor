import { NgModule } from '@angular/core';
import { CookieWarningComponent } from './cookie-warning/cookie-warning.component';
import { MatButtonModule, MatIconModule } from '@angular/material';
import { CommonModule } from '@angular/common';

@NgModule({
	declarations: [CookieWarningComponent],
	imports: [MatIconModule, MatButtonModule, CommonModule],
	exports: [CookieWarningComponent]
})
export class CookieWarningModule {}
