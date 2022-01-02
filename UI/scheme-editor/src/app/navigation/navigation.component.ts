import { Component, OnInit, ViewEncapsulation, OnDestroy, Input } from '@angular/core';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { MatDialog } from '@angular/material';
import { CurrentUserService } from 'projects/login/src/lib/current-user.service';
import { UserModel, MessageClient } from 'projects/rest/src/public-api';
import { ProfileDialogComponent } from 'projects/user-profile/src/public-api';
import { SchemeService } from '../../../projects/scheme-editor-page/src/lib/services/scheme.service';
import * as _ from 'lodash';
import { NotificationListlerService } from 'projects/notifications/src/lib/notification-listler.service';
import { Subscription } from 'rxjs';

@Component({
	selector: 'navigation',
	templateUrl: './navigation.component.html',
	styleUrls: ['./navigation.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class NavigationComponent implements OnInit, OnDestroy {
	public get hasUnread() {
		return this._unreadCount > 0;
	}

	public get currentUser(): UserModel {
		return this._userService.currentUser;
	}

	@Input() public set visible(val: boolean) {
		if (this._visible !== val) {
			this._init();
		}
		this._visible = val;
	}

	public get visible(): boolean {
		return this._visible;
	}

	private _unreadCount: number = 0;
	private _subscriptions: Subscription[] = [];
	private _visible: boolean = false;

	constructor(
		iconRegistry: MatIconRegistry,
		sanitizer: DomSanitizer,
		private _dialog: MatDialog,
		private _userService: CurrentUserService,
		private _messagesClient: MessageClient,
		private _schemeService: SchemeService,
		private _notificationListlerService: NotificationListlerService
	) {
		iconRegistry.addSvgIcon(
			'edit',
			sanitizer.bypassSecurityTrustResourceUrl('assets/icons/edit.svg')
		);
		iconRegistry.addSvgIcon(
			'account',
			sanitizer.bypassSecurityTrustResourceUrl('assets/icons/account.svg')
		);
		iconRegistry.addSvgIcon(
			'folder',
			sanitizer.bypassSecurityTrustResourceUrl('assets/icons/folder.svg')
		);
		iconRegistry.addSvgIcon(
			'notifications',
			sanitizer.bypassSecurityTrustResourceUrl(
				'assets/icons/notifications.svg'
			)
		);
		iconRegistry.addSvgIcon(
			'settings',
			sanitizer.bypassSecurityTrustResourceUrl(
				'assets/icons/settings.svg'
			)
		);
	}

	public ngOnInit(): void {
		this._init();
	}

	public ngOnDestroy(): void {
		this._subscriptions.filter(x => !x.closed).forEach(x => x.unsubscribe());
	}

	public openProfile = (): void => {
		const dialogRef = this._dialog.open(ProfileDialogComponent, {
			width: '500px',
			data: this._userService.currentUser
		});
	}

	public get editIsDisabled(): boolean {
		return _.isNil(this._schemeService.lastScheme);
	}

	private _init = () => {
		this._subscriptions.push(this._userService.currentUser$.subscribe((user: UserModel) => {
			if (_.isNil(user)) {
				return;
			}
			this._subscriptions.push(this._messagesClient.count(false).subscribe((count: number) => this._unreadCount = count));
			this._subscriptions.push(this._notificationListlerService.messageRecieved.subscribe((data: string) => this._unreadCount++));
			this._subscriptions.push(this._notificationListlerService.messageWasReaded.subscribe(() => this._unreadCount--));
		}));
	}
}
