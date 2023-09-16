import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DevExtremeComponent } from './dev-extreme.component';

const routes: Routes = [{ path: '', component: DevExtremeComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DevExtremeRoutingModule { }
