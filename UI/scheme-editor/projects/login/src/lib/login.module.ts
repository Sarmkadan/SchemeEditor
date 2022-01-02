import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { AuthorizationModule } from 'projects/authentication/src/lib/authorization.module';
import {
	MatInputModule,
	MatFormFieldModule,
	MatCheckboxModule,
	MatButtonModule,
	MatSnackBarModule,
	MatDialogModule
} from '@angular/material';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { RecaptchaModule } from 'ng-recaptcha';
import { CommonModule } from '@angular/common';
import { RecoveryPasswordComponent } from './recovery-password/recovery-password.component';
import { CookieWarningModule } from 'projects/cookie-warning/src/public-api';

@NgModule({
	declarations: [LoginComponent, RecoveryPasswordComponent],
	imports: [
		AuthorizationModule,
		MatInputModule,
		MatFormFieldModule,
		FormsModule,
		MatCheckboxModule,
		MatButtonModule,
		RouterModule,
		MatSnackBarModule,
		RecaptchaModule,
		CommonModule,
		MatDialogModule,
		CookieWarningModule
	],
	exports: [LoginComponent, RecoveryPasswordComponent],
	entryComponents: [RecoveryPasswordComponent]
})
export class LoginModule {}
