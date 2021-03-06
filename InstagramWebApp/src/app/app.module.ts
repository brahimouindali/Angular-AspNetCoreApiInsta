import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppMaterialModule } from './app-material/app-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { SigninComponent } from './components/signin/signin.component';
import { SignupComponent } from './components/signup/signup.component';
import { NavComponent } from './components/nav/nav.component';
import { ProfileComponent } from './components/profile/profile.component';
import { CommentformComponent } from './components/commentform/commentform.component';
import { MedialistComponent } from './components/medialist/medialist.component';

import { AuthInterceptor } from './services/auth.interceptor';
import { MediaService } from './services/media.service';
import { AuthService } from './services/auth.service';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { SubscribeusersComponent } from './components/dialogs/subscribeusers/subscribeusers.component';
import { AddMediaComponent } from './components/dialogs/add-media/add-media.component';
import { ManagePostComponent } from './components/dialogs/manage-post/manage-post.component';
import { MediaDetailComponent } from './components/dialogs/media-detail/media-detail.component';



@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SigninComponent,
    SignupComponent,
    NavComponent,
    ProfileComponent,
    CommentformComponent,
    MedialistComponent,
    MediaDetailComponent,
    NotFoundComponent,
    SubscribeusersComponent,
    AddMediaComponent,
    ManagePostComponent
  ],
  imports: [
    AppMaterialModule,
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule    
  ],
  providers: [
    AuthService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    MediaService
  ],
  entryComponents: [
    MedialistComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
