import { Injectable } from '@angular/core';
import { AccountClient, UserModel, LoginModel } from 'projects/rest/src/public-api';
import { Observable, of } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { RestConfig } from 'projects/rest/src/lib/rest-config';
import { Router } from '@angular/router';
import { AuthorizationService } from 'projects/authentication/src/lib/authorization.service';
import * as _ from 'lodash';

@Injectable({
	providedIn: 'root'
})
export class CurrentUserService {
	public get currentUser$(): Observable<UserModel> {
		if (!this._auth.isAuthorized) {
			return of(null);
		}

		if (_.isNil(this.currentUser)) {
			return this._accountClient.current();
		}

		return of(this.currentUser);
	}

	public currentUser: UserModel;

	constructor(private _accountClient: AccountClient, private _config: RestConfig, private _router: Router, private _auth: AuthorizationService) {
		if (this._auth.isAuthorized) {
			this._accountClient.current().subscribe((user: UserModel) => this.currentUser = user);
		}
	}

	public login = (model: LoginModel): Observable<any> => {
		return this._accountClient.login(model)
			.pipe(
				map((result: UserModel) => {
					this.currentUser = result;
					this._router.navigate(['/']);
				})
			);
	}

	public logout = (): Observable<any> => {
		return this._accountClient.logout()
			.pipe(tap(() => {
				this.currentUser = undefined;
				this._config.jwtToken = undefined;
				this._router.navigate(['/login']);
			}));
	}
}
