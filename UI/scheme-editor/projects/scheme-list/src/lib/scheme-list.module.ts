import { NgModule } from '@angular/core';
import { SchemeListComponent } from './scheme-list/scheme-list.component';
import {
	MatButtonModule,
	MatSlideToggleModule,
	MatTableModule,
	MatDialogModule,
	MatFormFieldModule,
	MatInputModule
} from '@angular/material';
import { RestModule } from '../../../rest/src/lib/rest.module';
import { CdkTableModule } from '@angular/cdk/table';
import { SharedModule } from '../../../shared/src/lib/shared.module';
import { CommonModule } from '@angular/common';
import { CreateSchemeDialogComponent } from './create-scheme-dialog/create-scheme-dialog.component';
import { FormsModule } from '@angular/forms';

@NgModule({
	declarations: [SchemeListComponent, CreateSchemeDialogComponent],
	imports: [
		MatSlideToggleModule,
		MatTableModule,
		RestModule,
		SharedModule,
		CdkTableModule,
		MatButtonModule,
		CommonModule,
		MatDialogModule,
		FormsModule,
		MatFormFieldModule,
		MatInputModule
	],
	exports: [SchemeListComponent, CreateSchemeDialogComponent],
	entryComponents: [CreateSchemeDialogComponent]
})
export class SchemeListModule {}
