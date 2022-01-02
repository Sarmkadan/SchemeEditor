import { Message } from 'projects/rest/src/public-api';

export class MessageModel {
	public from: string;
	public to: string;
	public title: string;
	public item: Message;
}
