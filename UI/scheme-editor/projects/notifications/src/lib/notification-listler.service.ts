import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Injectable({
	providedIn: 'root'
})
export class NotificationListlerService {
	public messageRecieved: Subject<string> = new Subject<string>();
	public messageWasReaded: Subject<string> = new Subject<string>();

	private _hubConnection: HubConnection;
	private _token: string;

	constructor() {
		this._createConnection();
		this._registerOnServerEvents();
		this._startConnection();
		this._token = localStorage.getItem('JwtToken');
	}

	private _createConnection() {
		this._hubConnection = new HubConnectionBuilder()
			.withUrl('http://localhost:5000/api/notifications', { accessTokenFactory: () => this._token })
			.build();
	}

	private _startConnection(): void {
		this._hubConnection
			.start()
			.catch((err: any) => {
				setTimeout(() => this._startConnection(), 5000);
			});
	}

	private _registerOnServerEvents(): void {
		this._hubConnection.on('Send', (data: any) => this.messageRecieved.next(data));
	}
}
