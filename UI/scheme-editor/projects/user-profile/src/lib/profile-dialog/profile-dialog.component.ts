import { Component, OnInit, Inject, ViewEncapsulation } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UserModel } from 'projects/rest/src/public-api';

@Component({
	selector: 'profile-dialog',
	templateUrl: './profile-dialog.component.html',
	styleUrls: ['./profile-dialog.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class ProfileDialogComponent implements OnInit {
	public get user(): UserModel {
		return this._data;
	}
	constructor(
		private _dialogRef: MatDialogRef<ProfileDialogComponent>,
		@Inject(MAT_DIALOG_DATA) private _data: UserModel
	) {}

	public ngOnInit() {
	}
	public onCancel(ev: any): void {
		this._dialogRef.close();
	}
}
