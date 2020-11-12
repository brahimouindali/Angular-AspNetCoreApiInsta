import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MediaDetailComponent } from './components/dialogs/media-detail/media-detail.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { ProfileComponent } from './components/profile/profile.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: 'accounts/signup', component: SignupComponent },
  { path: 'accounts/signin', component: SigninComponent },
  { path: 'accounts/logout', redirectTo: '/accounts/signin' },
  { path: 'home', canActivate: [AuthGuard], component: HomeComponent },
  { path: ':id', canActivate: [AuthGuard], component: ProfileComponent },
  { path: 'p/:id', component: MediaDetailComponent },
  { path: 'p/not-found', component: NotFoundComponent },
  { path: '', canActivate: [AuthGuard], component: HomeComponent },
  { path: '**', redirectTo: 'p/not-found' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
