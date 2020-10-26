import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ProfileComponent } from './components/profile/profile.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: 'accounts/signup', component: SignupComponent },
  { path: 'accounts/signin', component: SigninComponent },
  { path: ':id', component: ProfileComponent },
  { path: '', canActivate: [AuthGuard], component: HomeComponent },
  { path: 'home', canActivate: [AuthGuard], component: HomeComponent },
  { path: 'inbox', canActivate: [AuthGuard], redirectTo: 'home' },
  { path: ':id', canActivate: [AuthGuard], component: ProfileComponent },
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
