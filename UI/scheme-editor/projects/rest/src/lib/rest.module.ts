import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { MatSnackBarModule } from '@angular/material';
import { ApiErrorsInterceptor } from './api-errors.interceptor';


@NgModule({
	imports: [BrowserModule, HttpClientModule, MatSnackBarModule],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: ApiErrorsInterceptor, multi: true }
	]
})
export class RestModule { }
