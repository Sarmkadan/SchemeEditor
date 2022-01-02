import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { UserGridElement } from './user-grid-element';
import { ProfileDialogComponent } from 'projects/user-profile/src/public-api';
import { MatDialog, MatIconRegistry, PageEvent } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';
import { AccountClient, UserModel } from 'projects/rest/src/public-api';
import { map } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { ActionPanelType } from './ActionPanelType.enum';
import { CurrentUserService } from 'projects/login/src/lib/current-user.service';
import * as _ from 'lodash';

@Component({
	selector: 'admin-panel',
	templateUrl: './admin-panel.component.html',
	styleUrls: ['./admin-panel.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class AdminPanelComponent implements OnInit, OnDestroy {
	public dataSource: UserGridElement[] = [];
	public displayedColumns: string[] = ['name', 'group', 'cmd'];
	public length: number;
	public pageSize: number = 10;
	public page: number = 1;
	public actionPanel: ActionPanelType;
	public actionPanelType = ActionPanelType;

	private _subscriptions: Subscription[] = [];

	constructor(
		private _dialog: MatDialog,
		iconRegistry: MatIconRegistry,
		sanitizer: DomSanitizer,
		private _accountClient: AccountClient,
		private _currentUser: CurrentUserService
	) {
		iconRegistry.addSvgIcon(
			'edit',
			sanitizer.bypassSecurityTrustResourceUrl('assets/icons/edit.svg')
		);
	}

	public ngOnInit() {
		this._loadCount();
		this._loadUsers();
	}

	public ngOnDestroy(): void {
		this._subscriptions.filter(x => !x.closed).forEach(x => x.unsubscribe());
	}

	public onUserEdit = (element: UserGridElement): void => {
		const dialogRef = this._dialog.open(ProfileDialogComponent, {
			width: '500px',
			data: element.user
		});
		this._subscriptions.push(dialogRef.afterClosed().subscribe(result => {
			this._loadCount();
			this._loadUsers();
		}));
	}

	public onPageChanged = (ev: PageEvent): void => {
		this.page = ev.pageIndex + 1;
		this.pageSize = ev.pageSize;
		this._loadUsers();
	}

	public createNotification = () => {
		this.actionPanel = ActionPanelType.Notifications;
	}

	private _loadUsers = (): void => {
		this._subscriptions.push(this._accountClient.getUsers(undefined, undefined, undefined, undefined, true, this.page, this.pageSize)
			.pipe(map((element) => element
				.map((user: UserModel) => new UserGridElement(user, `${user.lastName || ''} ${user.name || ''} ${user.middleName || ''}`.trim(), user.role.name))))
			.subscribe((items: UserGridElement[]) => {
				this.dataSource = items
					.filter((user: UserGridElement) => _.isNil(this._currentUser.currentUser) || user.user.id !== this._currentUser.currentUser.id);
			}));
	}

	private _loadCount = (): void => {
		this._accountClient.getUsersCount(undefined, undefined, undefined, undefined, true, this.page, this.pageSize)
			.subscribe((count: number) => this.length = count);
	}
}
