import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LoginModel } from 'projects/rest/src/public-api';
import { CurrentUserService } from '../current-user.service';
import { MatDialog, MatSnackBar } from '@angular/material';
import { RecoveryPasswordComponent } from '../recovery-password/recovery-password.component';
import * as _ from 'lodash';

@Component({
	selector: 'login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
	public loginModel: LoginModel = new LoginModel();

	constructor(
		private _userService: CurrentUserService,
		private _dialog: MatDialog,
		private _snackBar: MatSnackBar
	) {
	}

	public ngOnInit() {}

	public login = () => {
		this._userService.login(this.loginModel).subscribe();
	}

	public onForgot = (ev) => {
		ev.preventDefault();
		if (_.isEmpty(this.loginModel.login)) {
			this._snackBar.open(
				'Нужно указать логин',
				'закрыть',
				{ duration: 2000 }
			);
			return;
		}
		const dialogRef = this._dialog.open(RecoveryPasswordComponent, {
			width: '353px',
			data: {email: this.loginModel.login}
		});
	}
}
