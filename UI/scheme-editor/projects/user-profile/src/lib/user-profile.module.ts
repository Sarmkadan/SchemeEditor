import { NgModule } from '@angular/core';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { ProfileDialogComponent } from './profile-dialog/profile-dialog.component';
import {
	MatInputModule,
	MatFormFieldModule,
	MatCheckboxModule,
	MatButtonModule,
	MatDialogModule,
	MatSelectModule,
	MatExpansionModule,
	MatSlideToggleModule
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@NgModule({
	declarations: [UserProfileComponent, ProfileDialogComponent],
	imports: [
		MatInputModule,
		MatFormFieldModule,
		FormsModule,
		MatCheckboxModule,
		MatButtonModule,
		CommonModule,
		ReactiveFormsModule,
		MatDialogModule,
		MatSelectModule,
		MatExpansionModule,
		MatSlideToggleModule
	],
	exports: [UserProfileComponent, ProfileDialogComponent],
	entryComponents: [ProfileDialogComponent]
})
export class UserProfileModule {}
