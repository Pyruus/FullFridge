import { NgModule } from '@angular/core';
import { TruncatePipe } from './truncate.pipe';

@NgModule({
  declarations: [
    TruncatePipe,
    // Other shared components, directives, and pipes go here
  ],
  exports: [
    TruncatePipe,
    // Other shared components, directives, and pipes go here
  ],
})
export class SharedModule { }