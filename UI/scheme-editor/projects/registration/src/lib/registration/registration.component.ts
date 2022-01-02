import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { RegisterModel, AccountClient } from 'projects/rest/src/public-api';
import * as _ from 'lodash';
import { Router } from '@angular/router';
import {
	Validators,
	FormControl,
	FormGroupDirective,
	NgForm,
	FormGroup
} from '@angular/forms';
import { ErrorStateMatcher, MatDialog } from '@angular/material';
import { NvmSettingsService } from 'nvm-settings';
import { LicenseConditionsComponent } from '../license-conditions/license-conditions.component';

export class RegistrationStateMatcher implements ErrorStateMatcher {
	public isErrorState(
		control: FormControl | null,
		form: FormGroupDirective | NgForm | null
	): boolean {
		const isSubmitted = form && form.submitted;
		return !!(
			control &&
			control.invalid &&
			(control.dirty || control.touched || isSubmitted)
		);
	}
}

@Component({
	selector: 'registration',
	templateUrl: './registration.component.html',
	styleUrls: ['./registration.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class RegistrationComponent implements OnInit {
	public emailFormControl: FormControl = new FormControl('', [
		Validators.required,
		Validators.email
	]);
	public phoneFormControl: FormControl = new FormControl('', [
		Validators.required,
		Validators.pattern(/^(\+[0-9]{1,3}|8)?([0-9]{3,4})?[0-9]{5,7}$/)
	]);
	public lastNameFormControl: FormControl = new FormControl('', [
		Validators.required,
		Validators.minLength(2)
	]);
	public firstNameFormControl: FormControl = new FormControl('', [
		Validators.required,
		Validators.minLength(2)
	]);
	public companyFormControl: FormControl = new FormControl('', [
		Validators.required,
		Validators.minLength(2)
	]);
	public acceptFormControl: FormControl = new FormControl('', [
		Validators.required,
		Validators.requiredTrue
	]);
	public middleNameFormControl: FormControl = new FormControl('', []);
	public positionFormControl: FormControl = new FormControl('', []);
	public cityFormControl: FormControl = new FormControl('', []);

	public formGroup = new FormGroup({
		phone: this.phoneFormControl,
		email: this.emailFormControl,
		lastName: this.lastNameFormControl,
		firstName: this.firstNameFormControl,
		company: this.companyFormControl,
		accept: this.acceptFormControl,
		middleName: this.middleNameFormControl,
		city: this.cityFormControl,
		position: this.positionFormControl
	});

	public acceptTermsValidated: boolean;
	public matcher = new RegistrationStateMatcher();
	public key: string;
	private _capchaResolved: boolean = false;

	constructor(
		private _rest: AccountClient,
		private _router: Router,
		private _settings: NvmSettingsService,
		private _dialog: MatDialog
	) {
		this.key = this._settings.get('recapcha-key');
	}

	public displayConditions = (ev: Event): void => {
		ev.preventDefault();
		this._dialog.open(LicenseConditionsComponent, {
			width: '353px',
			data: {}
		});
	}

	public resolved = (): void => {
		this._capchaResolved = true;
	}

	public ngOnInit() {}

	public register = (): void => {
		this.acceptTermsValidated = true;

		if (
			!this.formGroup.valid ||
			!this.formGroup.dirty ||
			(!_.isEmpty(this.key) && !this._capchaResolved)
		) {
			return;
		}

		const model = new RegisterModel();
		model.phone = this.phoneFormControl.value;
		model.email = this.emailFormControl.value;
		model.lastName = this.lastNameFormControl.value;
		model.name = this.firstNameFormControl.value;
		model.middleName = this.middleNameFormControl.value;
		model.city = this.cityFormControl.value;
		model.company = this.companyFormControl.value;
		model.termsAccepted = this.acceptFormControl.value;
		model.position = this.positionFormControl.value;
		this._rest
			.register(model)
			.subscribe(() => this._router.navigate(['/']));
	}
}
