import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CookieService } from 'ngx-cookie-service';

@NgModule({
	imports: [CommonModule],
	declarations: [],
	providers: [CookieService]
})
export class AuthorizationModule {}
