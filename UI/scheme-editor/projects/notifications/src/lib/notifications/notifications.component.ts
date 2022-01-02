import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { PageEvent, MatSlideToggleChange } from '@angular/material';
import { Subscription } from 'rxjs';
import {
	MessageClient,
	Message,
	MessageUser,
	UserModel
} from 'projects/rest/src/public-api';
import { CurrentUserService } from 'projects/login/src/public-api';
import { NotificationListlerService } from '../notification-listler.service';

@Component({
	selector: 'notifications',
	templateUrl: './notifications.component.html',
	styleUrls: ['./notifications.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class NotificationsComponent implements OnInit, OnDestroy {
	public selectedItem: MessageUser;
	public dataSource: MessageUser[] = [];
	public displayedColumns: string[] = ['title', 'createdAt'];
	public length: number;
	public pageSize: number = 10;
	public page: number = 1;

	private _subscriptions: Subscription[] = [];
	private _user: UserModel;

	constructor(
		private _messagesClient: MessageClient,
		private _currentUserService: CurrentUserService,
		private _notificationListlerService: NotificationListlerService
	) {}

	public ngOnInit() {
		this._loadCount();
		this._subscriptions.push(
			this._currentUserService.currentUser$.subscribe(
				(user: UserModel) => {
					this._user = user;
					this._loadNotifications();
				}
			)
		);
		this._subscriptions.push(this._notificationListlerService.messageRecieved.subscribe((data: string) => {
			this._loadNotifications();
			this.length++;
		}));
	}

	public ngOnDestroy(): void {
		this._subscriptions
			.filter(x => !x.closed)
			.forEach(x => x.unsubscribe());
	}

	public onPageChanged = (ev: PageEvent): void => {
		this.page = ev.pageIndex + 1;
		this.pageSize = ev.pageSize;
		this._loadNotifications();
	}

	public selectItem = (element: MessageUser): void => {
		this.selectedItem = element;
		if (!this.selectedItem.isRead) {
			this._changeIsRead();
		}
	}

	private _changeIsRead = (): void => {
		this._subscriptions.push(
			this._messagesClient
				.setIsRead(this.selectedItem)
				.subscribe((result: MessageUser) => {
					this.selectedItem = result;
					this._loadNotifications();
					this._notificationListlerService.messageWasReaded.next(result.message.body);
				})
		);
	}

	private _loadCount = (): void => {
		this._subscriptions.push(
			this._messagesClient
				.count()
				.subscribe((count: number) => (this.length = count))
		);
	}

	private _loadNotifications = (): void => {
		this._subscriptions.push(
			this._messagesClient
				.list(
					this._user.id,
					undefined,
					undefined,
					this.pageSize,
					this.page
				)
				.subscribe(
					(result: MessageUser[]) => (this.dataSource = result)
				)
		);
	}
}
