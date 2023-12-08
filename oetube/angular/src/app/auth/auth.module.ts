import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthUrlPipe } from './auth-url-pipe/auth-url.pipe';



@NgModule({
  declarations: [AuthUrlPipe],
  imports: [
    CommonModule
  ],
  exports:[AuthUrlPipe]
})
export class AuthModule { }
