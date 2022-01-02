import {
	Component,
	OnInit,
	ViewEncapsulation,
	Output,
	EventEmitter,
	Input
} from '@angular/core';
import { ErrorStateMatcher, MatSlideToggleChange } from '@angular/material';
import {
	FormControl,
	FormGroupDirective,
	NgForm,
	Validators,
	FormGroup,
	AbstractControl,
	ValidatorFn,
	ValidationErrors,
	AsyncValidatorFn
} from '@angular/forms';
import * as _ from 'lodash';
import { Observable, of, forkJoin } from 'rxjs';
import { AccountClient, UserModel, Role } from 'projects/rest/src/public-api';
import { map } from 'rxjs/operators';
import { CurrentUserService } from 'projects/login/src/lib/current-user.service';

export class ProfileStateMatcher implements ErrorStateMatcher {
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
	selector: 'user-profile',
	templateUrl: './user-profile.component.html',
	styleUrls: ['./user-profile.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class UserProfileComponent implements OnInit {
	@Output() public cancel: EventEmitter<any> = new EventEmitter<any>();
	@Input() public user: UserModel;

	public matcher = new ProfileStateMatcher();

	public emailFormControl: FormControl;
	public phoneFormControl: FormControl;
	public lastNameFormControl: FormControl;
	public firstNameFormControl: FormControl;
	public companyFormControl: FormControl;
	public oldPasswordFormControl: FormControl;
	public passwordFormControl: FormControl;
	public middleNameFormControl: FormControl;
	public cityFormControl: FormControl;
	public positionFormControl: FormControl;
	public roleFormControl: FormControl;
	public isBlockedFormControl: FormControl;

	public formGroup: FormGroup;

	public roles: Role[] = [];

	public get isAdmin(): boolean {
		if (_.isNil(this._isAdmin)) {
			this._isAdmin =
				this._userService.currentUser.role.normalizedName ===
				'ADMINISTRATOR';
		}
		return this._isAdmin;
	}

	public get isCurrentUser(): boolean {
		if (_.isNil(this._isCurrentUser) && !_.isNil(this.user)) {
			this._isCurrentUser =
				this._userService.currentUser.id === this.user.id;
		}
		return this._isCurrentUser;
	}

	private _isCurrentUser: boolean;
	private _isAdmin: boolean;

	constructor(
		private _accountClient: AccountClient,
		private _userService: CurrentUserService
	) {}

	public static passwordChangeValidator = (
		oldPasswordControll: AbstractControl
	): ValidatorFn => {
		return (control: AbstractControl): ValidationErrors | null => {
			return !_.isEmpty(control.value) &&
				_.isEmpty(oldPasswordControll.value)
				? { passwordChangeValidator: true }
				: null;
		};
	};

	public static passwordChangeValidatorAsync = (
		accountClient: AccountClient
	): AsyncValidatorFn => {
		return (
			control: AbstractControl
		):
			| Promise<ValidationErrors | null>
			| Observable<ValidationErrors | null> => {
			if (_.isEmpty(control.value)) {
				return of(null);
			}
			return accountClient
				.validatePassword(control.value)
				.pipe(
					map((result: boolean) =>
						result ? null : { passwordChangeValidatorAsync: true }
					)
				);
		};
	}

	public ngOnInit() {
		this._initForm();
	}

	public save = (): void => {
		Object.keys(this.formGroup.controls).forEach((key: string) => {
			if (!_.isNil(this.formGroup.controls[key].validator)) {
				this.formGroup.controls[key].setErrors(
					this.formGroup.controls[key].validator(
						this.formGroup.controls[key]
					)
				);
			}
		});
		if (!this.formGroup.valid || !this.formGroup.dirty) {
			return;
		}
		const model = new UserModel();
		model.id = this.user.id;
		model.name = this.firstNameFormControl.value;
		model.lastName = this.lastNameFormControl.value;
		model.middleName = this.middleNameFormControl.value;
		model.phone = this.phoneFormControl.value;
		model.email = this.emailFormControl.value;
		model.company = this.companyFormControl.value;
		model.city = this.cityFormControl.value;
		model.position = this.positionFormControl.value;
		model.password = this.passwordFormControl.value;
		model.emailConfirmed =
			this.user.emailConfirmed && !this.emailFormControl.dirty;
		model.phoneConfirmed =
			this.user.phoneConfirmed && !this.phoneFormControl.dirty;
		model.isBlocked = this.user.isBlocked;
		const updaters: Array<Observable<any>> = [
			this._accountClient.update(model)
		];
		if (this.isAdmin) {
			model.isBlocked = this.isBlockedFormControl.value;
			if (this.roleFormControl.dirty) {
				const role = this.roles.find(
					(r: Role) => r.normalizedName === this.roleFormControl.value
				);
				updaters.push(this._accountClient.changeRole(model.id, role));
			}
		}
		forkJoin(updaters).subscribe((data: any[]) => {
			this.user = data[data.length - 1];
			if (this.isCurrentUser) {
				this._userService.currentUser = data[data.length - 1];
			}
			this._initForm();
			this.cancel.emit();
		});
	}

	public onCancel = (): void => {
		this.cancel.emit();
	}

	public logout = (): void => {
		this._userService.logout().subscribe(() => {
			this.cancel.emit();
		});
	}

	public onBlockedChange = (ev: MatSlideToggleChange): void => {
		this.user.isBlocked = ev.checked;
	}

	public deleteUser = () => {
		this._accountClient
			.delete(this.user.id)
			.subscribe(() => this.cancel.emit());
	}

	public resetPassword = (): void => {
		this._accountClient.resetPassword(this.user.id)
			.subscribe();
	}

	private _initForm = () => {
		this._accountClient.getRoles().subscribe((roles: Role[]) => {
			this.roles = roles;
		});
		this.emailFormControl = new FormControl(this.user.email, [
			Validators.required,
			Validators.email
		]);
		this.phoneFormControl = new FormControl(this.user.phone, [
			Validators.required,
			Validators.pattern(/^(\+[0-9]{1,3}|8)?([0-9]{3,4})?[0-9]{5,7}$/)
		]);
		this.lastNameFormControl = new FormControl(this.user.lastName, [
			Validators.required,
			Validators.minLength(2)
		]);
		this.firstNameFormControl = new FormControl(this.user.name, [
			Validators.required,
			Validators.minLength(2)
		]);
		this.companyFormControl = new FormControl(this.user.company, [
			Validators.required,
			Validators.minLength(2)
		]);
		this.oldPasswordFormControl = new FormControl(
			'',
			[],
			[
				UserProfileComponent.passwordChangeValidatorAsync(
					this._accountClient
				)
			]
		);
		this.passwordFormControl = new FormControl('', [
			UserProfileComponent.passwordChangeValidator(
				this.oldPasswordFormControl
			)
		]);
		this.roleFormControl = new FormControl(this.user.role.normalizedName);
		this.middleNameFormControl = new FormControl(this.user.middleName, []);
		this.cityFormControl = new FormControl(this.user.city, []);
		this.positionFormControl = new FormControl(this.user.position, []);
		this.isBlockedFormControl = new FormControl(this.user.isBlocked);
		this.formGroup = new FormGroup({
			phone: this.phoneFormControl,
			email: this.emailFormControl,
			lastName: this.lastNameFormControl,
			firstName: this.firstNameFormControl,
			company: this.companyFormControl,
			position: this.positionFormControl,
			middleName: this.middleNameFormControl,
			city: this.cityFormControl,
			password: this.passwordFormControl,
			oldPassword: this.oldPasswordFormControl,
			role: this.roleFormControl,
			isBlocked: this.isBlockedFormControl
		});
	}
}
