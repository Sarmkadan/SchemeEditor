import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { CookieService } from 'ngx-cookie-service';

@Component({
	selector: 'cookie-warning',
	templateUrl: './cookie-warning.component.html',
	styleUrls: ['./cookie-warning.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class CookieWarningComponent implements OnInit {
	public display: boolean = false;
	constructor(
		iconRegistry: MatIconRegistry,
		sanitizer: DomSanitizer,
		private _cookies: CookieService
	) {
		iconRegistry.addSvgIcon(
			'close',
			sanitizer.bypassSecurityTrustResourceUrl('assets/icons/close.svg')
		);
	}

	public ngOnInit() {
		const accepted = this._cookies.get('coockie-accepted') === '1';
		this.display = !accepted;
	}

	public close = (e: Event, permanent: boolean = false): void => {
		e.preventDefault();
		this.display = false;
		if (permanent) {
			this._cookies.set('coockie-accepted', '1');
		}
	}
}
