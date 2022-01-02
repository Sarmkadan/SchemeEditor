import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import * as _ from 'lodash';
import { RestConfig } from 'projects/rest/src/lib/rest-config';

@Injectable({
	providedIn: 'root'
})
export class AuthorizationService {
	constructor(private _cookieService: CookieService, private _config: RestConfig) {}

	public get isAuthorized(): boolean {
		return !_.isEmpty(this._cookieService.get('.AspNetCore.Identity.Application')) || !_.isEmpty(this._config.jwtToken);
	}
}
