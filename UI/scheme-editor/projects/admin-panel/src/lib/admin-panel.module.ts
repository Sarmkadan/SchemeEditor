import { NgModule } from '@angular/core';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { RestModule } from 'projects/rest/src/lib/rest.module';
import {
	MatIconModule,
	MatButtonModule,
	MatPaginatorModule
} from '@angular/material';
import { CommonModule } from '@angular/common';
import { NotificationsModule } from 'projects/notifications/src/public-api';

@NgModule({
	declarations: [AdminPanelComponent],
	imports: [
		MatCardModule,
		MatTableModule,
		RestModule,
		MatIconModule,
		MatButtonModule,
		MatPaginatorModule,
		CommonModule,
		NotificationsModule
	],
	exports: [AdminPanelComponent]
})
export class AdminPanelModule {}
