import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';
import { AddAddresseComponent } from '../add-addresse/add-addresse.component';
import { MatDialog, MatSnackBar, PageEvent } from '@angular/material';
import { Subscription } from 'rxjs';
import { Addresse, MessageClient, SendMessageModel, Message } from 'projects/rest/src/public-api';
import { MessageModel } from './MessageModel';
import { map } from 'rxjs/operators';
import * as _ from 'lodash';

@Component({
	selector: 'notification-sender',
	templateUrl: './notification-sender.component.html',
	styleUrls: ['./notification-sender.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class NotificationSenderComponent implements OnInit, OnDestroy {
	public dataSource: MessageModel[] = [];
	public displayedColumns: string[] = ['from', 'to', 'title', 'cmd'];
	public length: number;
	public pageSize: number = 10;
	public page: number = 1;

	public title: string;
	public text: string;
	public addresse: Addresse[] = [];

	private _subscriptions: Subscription[] = [];
	constructor(private _dialog: MatDialog, private _messageClient: MessageClient, private _snackBar: MatSnackBar) {}

	public ngOnInit() {
		this._loadHistoryCount();
		this._loadHistory();
	}

	public ngOnDestroy(): void {
		this._subscriptions.filter(x => !x.closed).forEach(x => x.unsubscribe());
	}

	public displayAddAddresse = () => {
		const dialogRef = this._dialog.open(AddAddresseComponent, {
			width: '500px',
			data: this.addresse
		});
		this._subscriptions.push(dialogRef.afterClosed()
			.subscribe((result: Addresse[]) => this.addresse = result || []));
	}

	public send = () => {
		const model = new SendMessageModel();
		model.text = this.text;
		model.title = this.title;
		model.addresses = this.addresse;
		this._subscriptions.push(this._messageClient.send(model)
			.subscribe(() => {
				this._snackBar.open('Рассылка успешно отправлена', 'закрыть', { duration: 2000 });
				this._loadHistory();
				this._loadHistoryCount();
			}));
	}

	public onPageChanged = (ev: PageEvent): void => {
		this.page = ev.pageIndex + 1;
		this.pageSize = ev.pageSize;
		this._loadHistory();
	}

	public deleteMessage = (element: MessageModel) => {
		this._messageClient.delete(element.item.id)
			.subscribe(() => {
				this._loadHistory();
				this._loadHistoryCount();
			});
	}

	private _loadHistory = () => {
		this._subscriptions.push(this._messageClient.history(undefined, undefined, undefined, this.pageSize, this.page)
			.pipe(map((result: Message[]) => {
				return result.map((x: Message) => {
					const message = new MessageModel();
					message.from = [x.author.lastName, x.author.name, x.author.middleName].filter(y => !_.isEmpty(y)).join(' ');
					message.to = x.addresses;
					message.title = x.title;
					message.item = x;
					return message;
				});
			})).subscribe((history: MessageModel[]) => this.dataSource = history));
	}

	private _loadHistoryCount = () => {
		this._subscriptions.push(this._messageClient.historyCount(undefined, undefined, undefined, undefined, undefined)
			.subscribe((count: number) => this.length = count));
	}
}
