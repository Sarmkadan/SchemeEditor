import { Component, OnInit, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SchemesClient, SchemeView } from 'projects/rest/src/public-api';
import { Router } from '@angular/router';
import { SchemeService } from 'projects/scheme-editor-page/src/public-api';

@Component({
	selector: 'create-scheme-dialog',
	templateUrl: './create-scheme-dialog.component.html',
	styleUrls: ['./create-scheme-dialog.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class CreateSchemeDialogComponent implements OnInit {
	public name: string;
	constructor(
		private _dialogRef: MatDialogRef<CreateSchemeDialogComponent>,
		@Inject(MAT_DIALOG_DATA) private _data: any,
		private _schemeClient: SchemesClient,
		private _router: Router,
		private _schemeService: SchemeService,
	) {}

	public ngOnInit() {}

	public onCancel = (): void => {
		this.name = '';
		this._dialogRef.close();
	}

	public onCreate() {
		const model = new SchemeView();
		model.name = this.name;
		this._dialogRef.close();
		this._schemeClient.post(model)
			.subscribe((scheme: SchemeView) => {
				this._schemeService.lastScheme = scheme;
				this._router.navigate(['editor']);
			});
	}
}
