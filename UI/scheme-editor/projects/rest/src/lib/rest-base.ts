import { RestConfig } from './rest-config';
import { Observable } from 'rxjs';
import { HttpResponseBase, HttpHeaders } from '@angular/common/http';
import * as _ from 'lodash';

export class RestBase {
	constructor(protected _config: RestConfig) {}

	protected getBaseUrl(base: string): string {
		return 'http://localhost:5000';
	}

	protected transformOptions(options: any) {
		if (!_.isEmpty(this._config.jwtToken)) {
			options = {
				body: options.body,
				observe: options.observe,
				responseType: options.responseType,
				headers: new HttpHeaders({
					'Content-Type': 'application/json',
					'Accept': 'application/json',
					'Authorization': `Bearer ${this._config.jwtToken}`
				})
			};
		}
		options['withCredentials'] = true;
		return Promise.resolve(options);
	}

	protected transformResult(url: string, response: HttpResponseBase, callback: (response: any) => Observable<any>) {
		const token = response.headers.get('JwtToken');
		if (!_.isEmpty(token)) {
			this._config.jwtToken = token;
		}
		return callback(response);
	}
}
