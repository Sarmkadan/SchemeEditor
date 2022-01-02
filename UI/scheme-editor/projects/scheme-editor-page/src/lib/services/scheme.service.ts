import { Injectable } from '@angular/core';
import { SchemeView } from '../../../../rest/src/lib/rest.service';

@Injectable({
	providedIn: 'root'
})
export class SchemeService {
	public lastScheme: SchemeView;
}
