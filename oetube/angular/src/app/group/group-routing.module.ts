import { NgModule } from '@angular/core';
import { RouterModule, Routes,provideRouter } from '@angular/router';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { GroupComponent } from './group.component';

const routes: Routes = [
  {path:'', component:GroupComponent},
  {path:':id', component:GroupDetailsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers:[provideRouter(routes)]
})
export class GroupRoutingModule {}
