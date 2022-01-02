import { Injectable } from '@angular/core';
import * as _ from 'lodash';
@Injectable({
	providedIn: 'root'
})
export class RestConfig {
	private _jwtKey: string = 'JwtToken';
	public get jwtToken(): string {
		return localStorage.getItem(this._jwtKey);
	}

	public set jwtToken(val: string) {
		if (_.isEmpty(val))  {
			localStorage.removeItem(this._jwtKey);
			return;
		}
		localStorage.setItem(this._jwtKey, val);
	}
}
