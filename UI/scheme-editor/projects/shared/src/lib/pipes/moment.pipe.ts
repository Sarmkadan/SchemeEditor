import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';
import * as _ from 'lodash';

@Pipe({
	name: 'dateMoment'
})
export class MomentPipe implements PipeTransform {
	transform(value: string, formatFrom: string, formatTo: string = 'DD.MM.YY'): string {
		if (_.isEmpty(value)) {
			return '';
		}
		return moment(value, formatFrom).format(formatTo);
	}
}
