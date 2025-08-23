import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user.component';
import { AuthGuard } from '../../core/auth.guard';

const routes: Routes = [
  { path: '', component: UserComponent, canActivate: [AuthGuard] } // protect /dashboard
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
