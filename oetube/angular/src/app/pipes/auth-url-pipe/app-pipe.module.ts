import { NgModule } from "@angular/core";
import { AuthUrlPipe } from "./auth-url.pipe";
import { TimePipe } from "./time.pipe";

@NgModule({
    declarations: [
     TimePipe,
     AuthUrlPipe
    ],
 
    exports: [TimePipe,AuthUrlPipe],
  })
  export class AppPipeModule {}
  