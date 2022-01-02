import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from 'projects/shared/src/public-api';
import { CoreModule } from 'projects/core/src/public-api';
import { UiModule } from 'projects/ui/src/public-api';
import { SchemeListModule } from 'projects/scheme-list/src/public-api';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { AuthorizationModule } from 'projects/authentication/src/lib/authorization.module';
import { LoginModule } from 'projects/login/src/public-api';
import { RegistrationModule } from 'projects/registration/src/public-api';
import {
	MatInputModule,
	MatFormFieldModule,
	MatCheckboxModule,
	MatButtonModule,
	MatDialogModule,
	MatSelectModule,
	MatExpansionModule,
	MatSlideToggleModule,
	MatPaginatorIntl
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserProfileModule } from 'projects/user-profile/src/public-api';
import { SchemeEditorPageModule } from 'projects/scheme-editor-page/src/public-api';
import { AdminPanelModule } from 'projects/admin-panel/src/public-api';
import { NotificationsModule } from 'projects/notifications/src/public-api';
import { CookieWarningModule } from 'projects/cookie-warning/src/public-api';
import { NvmSettingsModule, NvmSettingsService } from 'nvm-settings';

export function getDutchPaginatorIntl() {
	const paginatorIntl = new MatPaginatorIntl();
	paginatorIntl.itemsPerPageLabel = 'Элементов на страницу';
	paginatorIntl.getRangeLabel = (page: number, pageSize: number, length: number) => `${page * pageSize + 1} - ${Math.min((page + 1) * pageSize, length)} из ${length}`;
	return paginatorIntl;
}

export const settingsProvider = (config: NvmSettingsService) => () => {
	return config.load('/assets/settings.json');
};

export const useAppConfigProvider = { provide: APP_INITIALIZER, useFactory: settingsProvider, deps: [NvmSettingsService], multi: true };

@NgModule({
	declarations: [AppComponent, NavigationComponent],
	imports: [
		BrowserModule,
		AppRoutingModule,
		MatSidenavModule,
		BrowserAnimationsModule,
		SharedModule,
		CoreModule,
		UiModule,
		SchemeListModule,
		MatIconModule,
		HttpClientModule,
		AuthorizationModule,
		LoginModule,
		RegistrationModule,
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
		MatSlideToggleModule,
		UserProfileModule,
		SchemeEditorPageModule,
		AdminPanelModule,
		NotificationsModule,
		NvmSettingsModule.forRoot()
	],
	providers: [
		{ provide: MatPaginatorIntl, useValue: getDutchPaginatorIntl() },
		useAppConfigProvider
	],
	bootstrap: [AppComponent]
})
export class AppModule {}
