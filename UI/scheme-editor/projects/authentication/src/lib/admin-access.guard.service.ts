import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { CurrentUserService } from 'projects/login/src/lib/current-user.service';
import * as _ from 'lodash';

@Injectable({
	providedIn: 'root'
})
export class AdminAccessGuard {
	public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
		if (_.isNil(this._userService.currentUser) || _.isNil(this._userService.currentUser.role) || this._userService.currentUser.role.normalizedName !== 'ADMINISTRATOR') {
			return false;
		}
		return true;
	}
	constructor(private _userService: CurrentUserService) {}
}
