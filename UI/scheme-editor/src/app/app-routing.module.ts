import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { SchemeListComponent } from 'projects/scheme-list/src/lib/scheme-list/scheme-list.component';
import { LoginComponent } from 'projects/login/src/lib/login/login.component';
import { RegistrationComponent } from 'projects/registration/src/public-api';
import { AuthorizationGuardService } from 'projects/authentication/src/lib/authorization-guard.service';
import { SchemeEditorPageComponent } from '../../projects/scheme-editor-page/src/lib/scheme-editor-page/scheme-editor-page.component';
import { AdminPanelComponent } from 'projects/admin-panel/src/public-api';
import { AdminAccessGuard } from 'projects/authentication/src/lib/admin-access.guard.service';
import { NotificationsComponent } from 'projects/notifications/src/public-api';

const routes: Routes = [
	{ path: '', component: SchemeListComponent, pathMatch: 'full', canActivate: [AuthorizationGuardService]},
	{ path: 'editor', component: SchemeEditorPageComponent, pathMatch: 'full', canActivate: [AuthorizationGuardService]},
	{ path: 'notifications', component: NotificationsComponent, pathMatch: 'full', canActivate: [AuthorizationGuardService]},
	{ path: 'settings', component: AdminPanelComponent, pathMatch: 'full', canActivate: [AuthorizationGuardService, AdminAccessGuard]},
	{ path: 'login', component: LoginComponent, pathMatch: 'full'},
	{ path: 'registration', component: RegistrationComponent, pathMatch: 'full'},
	{ path: '**', redirectTo: '/login'}
];

@NgModule({
	imports: [
		RouterModule.forRoot(routes, {
			preloadingStrategy: PreloadAllModules,
			useHash: true,
			onSameUrlNavigation: 'reload'
		})],
	exports: [RouterModule]
})
export class AppRoutingModule {}
