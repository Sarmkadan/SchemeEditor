import { NgModule } from '@angular/core';
import { SchemeEditorPageComponent } from './scheme-editor-page/scheme-editor-page.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatButtonModule, MatFormFieldModule, MatInputModule } from '@angular/material';

@NgModule({
	declarations: [SchemeEditorPageComponent],
	imports: [FormsModule, CommonModule, MatButtonModule, MatFormFieldModule, MatInputModule],
	exports: [SchemeEditorPageComponent]
})
export class SchemeEditorPageModule { }
