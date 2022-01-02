import { Component, OnInit, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { AccountClient } from 'projects/rest/src/public-api';
import { NvmSettingsService } from 'nvm-settings';
import * as _ from 'lodash';

@Component({
	selector: 'recovery-password',
	templateUrl: './recovery-password.component.html',
	styleUrls: ['./recovery-password.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class RecoveryPasswordComponent implements OnInit {
	public key: string;
	private _email: string;
	private _recapchaResolved: boolean = false;

	constructor(
		private _dialogRef: MatDialogRef<RecoveryPasswordComponent>,
		@Inject(MAT_DIALOG_DATA) private _data: any,
		private _accountClient: AccountClient,
		private _snackBar: MatSnackBar,
		private _settings: NvmSettingsService
	) {
		this.key = this._settings.get<string>('recapcha-key');
		this._email = this._data.email;
	}

	public ngOnInit() {}

	public resolved = (): void => {
		this._recapchaResolved = true;
	}

	public onRecover = () => {
		if (!this._recapchaResolved && !_.isEmpty(this.key)) {
			return;
		}
		this._dialogRef.close();
		this._accountClient
			.forgotPassword(this._email)
			.subscribe(() =>
				this._snackBar.open(
					'Новый пароль отправлен на указанный при регистрации номер телефона',
					'закрыть',
					{ duration: 2000 }
				)
			);
	}

	public onCancel = () => {
		this._dialogRef.close();
	}
}
