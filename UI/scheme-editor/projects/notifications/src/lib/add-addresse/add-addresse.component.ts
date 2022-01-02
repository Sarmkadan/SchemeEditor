import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { PageEvent, MatCheckboxChange, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { UserModel, AccountClient, Addresse, AddresseType, Role } from 'projects/rest/src/public-api';
import { map } from 'rxjs/operators';
import * as _ from 'lodash';

@Component({
	selector: 'add-addresse',
	templateUrl: './add-addresse.component.html',
	styleUrls: ['./add-addresse.component.scss'],
	encapsulation: ViewEncapsulation.None
})
export class AddAddresseComponent implements OnInit {
	public displayedColumns: string[] = ['cmd', 'name'];
	public length: number;
	public pageSize: number = 5;
	public page: number = 1;

	public selectedUsers: Addresse[] = [];
	public users: Addresse[] = [];
	public roles: Addresse[] = [];

	constructor(private _accountClient: AccountClient,
		private _dialogRef: MatDialogRef<AddAddresseComponent>,
		@Inject(MAT_DIALOG_DATA) private _data: Addresse[]
	) {
		this.selectedUsers = _.cloneDeep(this._data || []);
	}

	public ngOnInit() {
		this._accountClient.getUsersCount(undefined, undefined, undefined, undefined, false, this.page, this.pageSize)
			.subscribe((count: number) => this.length = count);
		this._accountClient.getRoles()
			.subscribe((roles: Role[]) => {
				this.roles = roles.map((role: Role) => {
					const addresse = new Addresse();
					addresse.id = role.id;
					addresse.title = role.name;
					addresse.name = role.normalizedName;
					addresse.type = AddresseType.Role;
					return addresse;
				});
				this.roles = [new Addresse({title: 'Все', type: AddresseType.Role} as any), ...this.roles];
			});
		this._loadAddresse();
	}

	public onPageChanged = (ev: PageEvent): void => {
		this.page = ev.pageIndex + 1;
		this.pageSize = ev.pageSize;
		this._loadAddresse();
	}

	public checked = (item: Addresse): boolean => {
		if (_.isNil(item.id)) {
			return !this.roles.filter(x => !_.isNil(x.id)).some((y: Addresse) => !this.selectedUsers.some((x: Addresse) => y.id === x.id && y.type === x.type));
		}
		return this.selectedUsers.some((x: Addresse) => item.id === x.id && item.type === x.type);
	}

	public onChecked = (ev: MatCheckboxChange, item: Addresse): void => {
		if (ev.checked) {
			if (_.isNil(item.id)) {
				this.selectedUsers = _.uniqBy([...this.selectedUsers, ...this.roles.filter(x => !_.isNil(x.id))], (x) => x.id * 10 + x.type);
			} else {
				this.selectedUsers.push(item);
			}
		} else {
			if (_.isNil(item.id)) {
				this.selectedUsers = this.selectedUsers.filter(x => x.type !== item.type);
			} else {
				this.selectedUsers = this.selectedUsers.filter(x => x.id !== item.id || x.type !== item.type);
			}
		}
	}

	public cancel = () => {
		this._dialogRef.close();
	}

	private _loadAddresse = () => {
		this._accountClient.getUsers(undefined, false, undefined, undefined, false, this.page, this.pageSize)
			.pipe(
				map((users: UserModel[]) => users.map(((user: UserModel) => {
					const addresse = new Addresse();
					addresse.id = user.id,
					addresse.type = AddresseType.User;
					addresse.name = [user.lastName, user.name, user.middleName].filter(x => !_.isEmpty(x)).join(' ');
					addresse.title = addresse.name;
					return addresse;
				})))
			).subscribe((addresses: Addresse[]) => {
				this.users = addresses;
			});
	}
}
