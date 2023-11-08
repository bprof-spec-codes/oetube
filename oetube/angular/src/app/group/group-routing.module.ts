import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { GroupComponent } from './group.component';
import { GroupCreateComponent } from './group-create/group-create.component';

const routes: Routes = [
  {
    path: '',
    component: GroupComponent,
  },
  {
    path: 'create',
    component: GroupCreateComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupRoutingModule {}
