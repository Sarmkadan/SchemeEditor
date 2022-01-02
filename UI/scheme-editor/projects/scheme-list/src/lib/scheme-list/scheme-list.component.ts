import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { Subscription } from 'rxjs';
import { SchemeView, SchemesClient } from 'projects/rest/src/public-api';
import { Router } from '@angular/router';
import { SchemeService } from '../../../../scheme-editor-page/src/lib/services/scheme.service';
import * as _ from 'lodash';
import { CreateSchemeDialogComponent } from '../create-scheme-dialog/create-scheme-dialog.component';

@Component({
	selector: 'scheme-list',
	templateUrl: './scheme-list.component.html',
	styleUrls: ['./scheme-list.component.scss']
})
export class SchemeListComponent implements OnInit, OnDestroy {
	public dataSource: MatTableDataSource<SchemeView>;
	public columnsToDisplay = [
		'name',
		'createdBy',
		'createdAt',
		'edit',
		'delete'
	];
	public checkedDelete = false;
	private _schemeSubscription: Subscription;

	constructor(
		private _schemeClient: SchemesClient,
		private _router: Router,
		private _schemeService: SchemeService,
		private _dialog: MatDialog
	) {}

	public ngOnInit() {
		this._updateSchemes();
	}

	public ngOnDestroy(): void {
		if (this._schemeSubscription) {
			this._schemeSubscription.unsubscribe();
		}
	}

	public onChangeToggle(checked: boolean) {
		this.checkedDelete = checked;
		this.dataSource.filter = checked.toString();
	}

	public getName = (scheme: SchemeView): string => {
		if (!_.isNil(scheme.author)) {
			scheme['authorName'] = [scheme.author.lastName, scheme.author.name, scheme.author.middleName].filter(x => !_.isEmpty(x)).join(' ');
		}
		return scheme['authorName'] || scheme.createdBy;
	}

	public onMarkDelete(scheme: SchemeView) {
		if (
			!_.isNil(this._schemeService.lastScheme) &&
			this._schemeService.lastScheme.id === scheme.id
		) {
			this._schemeService.lastScheme = null;
		}

		if (this.checkedDelete && scheme.deletedAt !== undefined) {
			this._schemeClient
				.deletePermanent(scheme.id)
				.subscribe(x => this._updateSchemes());
		} else {
			this._schemeClient
				.delete(scheme.id)
				.subscribe(x => this._updateSchemes());
		}
	}

	public onRestore(scheme: SchemeView) {
		this._schemeClient
			.restore(scheme.id)
			.subscribe(x => this._updateSchemes());
	}

	public onEdit(scheme: SchemeView) {
		this._schemeService.lastScheme = scheme;
		this._router.navigate(['editor']);
	}

	public showDialog = (): void => {
		const dialogRef = this._dialog.open(CreateSchemeDialogComponent, {
			width: '500px',
			data: {}
		});
	}

	private _filter = (data: SchemeView, filter: string): boolean => {
		if (filter === 'true') {
			return data.deletedAt !== undefined;
		} else {
			return data.deletedAt === undefined;
		}
	}

	private _updateSchemes = () => {
		this._schemeSubscription = this._schemeClient
			.getAll()
			.subscribe(schemes => {
				this.dataSource = new MatTableDataSource(schemes);
				this.dataSource.filterPredicate = this._filter;
				this.onChangeToggle(this.checkedDelete);
			});
	}
}
