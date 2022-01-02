import { NgModule } from '@angular/core';
import { RegistrationComponent } from './registration/registration.component';
import { AuthorizationModule } from 'projects/authentication/src/lib/authorization.module';
import {
	MatInputModule,
	MatFormFieldModule,
	MatCheckboxModule,
	MatButtonModule,
	MatDialogModule
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RecaptchaModule } from 'ng-recaptcha';
import { LicenseConditionsComponent } from './license-conditions/license-conditions.component';

@NgModule({
	declarations: [RegistrationComponent, LicenseConditionsComponent],
	imports: [
		AuthorizationModule,
		MatInputModule,
		MatFormFieldModule,
		FormsModule,
		MatCheckboxModule,
		MatButtonModule,
		CommonModule,
		RecaptchaModule,
		ReactiveFormsModule,
		MatDialogModule
	],
	exports: [RegistrationComponent, LicenseConditionsComponent],
	entryComponents: [LicenseConditionsComponent]
})
export class RegistrationModule {}
