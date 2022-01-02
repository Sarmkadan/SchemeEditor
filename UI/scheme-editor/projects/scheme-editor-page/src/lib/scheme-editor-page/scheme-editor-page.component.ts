import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { SchemesClient, SchemeView } from '../../../../rest/src/lib/rest.service';
import { ActivatedRoute } from '@angular/router';
import { SchemeService } from '../services/scheme.service';
import { Subscription } from 'rxjs';

@Component({
	selector: 'scheme-editor-page',
	templateUrl: './scheme-editor-page.component.html',
	styleUrls: ['./scheme-editor-page.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class SchemeEditorPageComponent implements OnInit, OnDestroy {
	public scheme: SchemeView;
	public loading = false;
	private _sub: Subscription;
	constructor(private schemeClient: SchemesClient, private route: ActivatedRoute, private schemeService: SchemeService) {}

	public ngOnInit(): void {
		this.scheme = this.schemeService.lastScheme;
	}

	public ngOnDestroy(): void {
		if (this._sub) {
			this._sub.unsubscribe();
		}
	}

	public onSubmit() {
		this.loading = true;
		this._sub = this.schemeClient.put(this.scheme).subscribe(_ => this.loading = false);
	}
}
