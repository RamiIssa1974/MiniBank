import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [{ 
  path: 'login', loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule) 
},{
    path: 'dashboard',
    loadChildren: () =>
      import('./features/user/user.module').then(m => m.UserModule),
  },{ path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },];

@NgModule({
  imports: [RouterModule.forRoot(routes,{ bindToComponentInputs: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
