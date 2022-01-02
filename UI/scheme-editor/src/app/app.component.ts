import { Component, ViewEncapsulation } from '@angular/core';
import { AuthorizationService } from 'projects/authentication/src/lib/authorization.service';

@Component({
	selector: 'root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class AppComponent {
	public title = 'scheme-editor';
	public get navOpened(): boolean {
		return this._authService.isAuthorized;
	}
	constructor(private _authService: AuthorizationService) {}
}
