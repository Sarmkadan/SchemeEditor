import { UserModel } from 'projects/rest/src/lib/rest.service';

export class UserGridElement {
	constructor(
		public user: UserModel,
		public name: string,
		public group: string
	) {}
}
