import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthorizationService } from './authorization.service';

@Injectable({
	providedIn: 'root'
})
export class AuthorizationGuardService implements CanActivate {
	// tslint:disable-next-line:max-line-length
	public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
		if (!this._authService.isAuthorized) {
			this._router.navigate(['login']);
			return false;
		}
		return true;
	}
	constructor(private _authService: AuthorizationService, private _router: Router) {}
}
