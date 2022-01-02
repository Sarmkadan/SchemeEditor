import { NgModule } from '@angular/core';
import { NotificationsComponent } from './notifications/notifications.component';
import {
	MatPaginatorModule,
	MatButtonModule,
	MatIconModule,
	MatTableModule,
	MatCardModule,
	MatSlideToggleModule,
	MatInputModule,
	MatCheckboxModule,
	MatDialogModule,
	MatExpansionModule
} from '@angular/material';
import { RestModule } from 'projects/rest/src/public-api';
import { SharedModule } from 'projects/shared/src/public-api';
import { CommonModule } from '@angular/common';
import { NotificationSenderComponent } from './notification-sender/notification-sender.component';
import { FormsModule } from '@angular/forms';
import { AddAddresseComponent } from './add-addresse/add-addresse.component';

@NgModule({
	declarations: [NotificationsComponent, NotificationSenderComponent, AddAddresseComponent],
	imports: [
		MatCardModule,
		MatTableModule,
		RestModule,
		MatIconModule,
		MatButtonModule,
		MatPaginatorModule,
		MatSlideToggleModule,
		SharedModule,
		CommonModule,
		FormsModule,
		MatInputModule,
		MatCheckboxModule,
		MatDialogModule,
		MatExpansionModule
	],
	exports: [NotificationsComponent, NotificationSenderComponent, AddAddresseComponent],
	entryComponents: [AddAddresseComponent]
})
export class NotificationsModule {}
