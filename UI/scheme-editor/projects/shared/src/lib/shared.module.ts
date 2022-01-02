import { NgModule } from '@angular/core';
import { MomentPipe } from './pipes/moment.pipe';

@NgModule({
	declarations: [MomentPipe],
	imports: [],
	exports: [MomentPipe]
})
export class SharedModule {}
