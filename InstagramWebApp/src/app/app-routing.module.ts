import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ProfileComponent } from './components/profile/profile.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: 'accounts/signup', component: SignupComponent },
  { path: 'accounts/signin', component: SigninComponent },
  { path: '', canActivate: [AuthGuard], component: HomeComponent },
  { path: 'home', canActivate: [AuthGuard], component: HomeComponent },  
  { path: 'inbox', redirectTo: 'home' },
  // { path: 'inbox',  component: MediaComponent },
  { path: ':id', canActivate: [AuthGuard], component: ProfileComponent },
  { path: 'p/not-found', component: NotFoundComponent },
  { path: '**', redirectTo: 'not-found' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
