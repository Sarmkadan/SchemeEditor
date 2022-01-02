import { Injectable } from '@angular/core';
import {
	HttpInterceptor,
	HttpRequest,
	HttpHandler,
	HttpEvent,
	HttpErrorResponse
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap, mergeMap } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';
import * as _ from 'lodash';

@Injectable()
export class ApiErrorsInterceptor implements HttpInterceptor {
	constructor(private _snackBar: MatSnackBar) {}

	public intercept(req: HttpRequest<any>,	next: HttpHandler): Observable<any> {
		return next.handle(req).pipe(
			catchError((error: HttpErrorResponse, caught: Observable<HttpEvent<any>>) => {
				if (error.status === 401) {
					this._snackBar.open('Ошибка! Авторизация не удалась', 'закрыть', { duration: 15000, politeness: 'assertive' });
					return of(error);
				}
				if (error.status === 400) {
					return this._blobToText(error.error)
						.pipe(
							tap((responseText: string) => {
								if (_.isEmpty(responseText)) {
									this._snackBar.open('Ошибка! Произошла неизвестная ошибка', 'закрыть', { duration: 15000, politeness: 'assertive' });
									return;
								}
								const response = JSON.parse(responseText);
								const validationErrors = Object.keys(response.errors).map((key) => response.errors[key].join(',')).join(';');
								this._snackBar.open(`Ошибка валидации! ${validationErrors}`, 'закрыть', { duration: 15000, politeness: 'assertive' });
							}),
							mergeMap(() => of(error))
						);
				}
				if (error.status === 401) {
					localStorage.removeItem('JwtToken');
					return of(error);
				}
				if (error.status === 403) {
					this._snackBar.open('Ошибка! Недостаточно прав', 'закрыть', { duration: 15000, politeness: 'assertive' });
					return of(error);
				}
				this._snackBar.open('Ошибка! Произошла неизвестная ошибка', 'закрыть', { duration: 15000, politeness: 'assertive' });
				return of(error);
			})
		);
	}

	private _blobToText = (blob: any): Observable<string> => {
		return new Observable<string>((observer: any) => {
			if (!blob) {
				observer.next('');
				observer.complete();
			} else {
				const reader = new FileReader();
				reader.onload = event => {
					observer.next((<any>event.target).result);
					observer.complete();
				};
				reader.readAsText(blob);
			}
		});
	}
}
