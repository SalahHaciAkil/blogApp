import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { QuillModule } from 'ngx-quill';


import { MatButtonModule } from '@angular/material/button'
import { MatInputModule } from '@angular/material/input'
import { MatIconModule } from '@angular/material/icon'
import { MatDialogModule } from "@angular/material/dialog"
import { MatFormFieldModule } from '@angular/material/form-field'
import { HomeComponent } from './_components/home/home.component';
import { NavComponent } from './_components/nav/nav.component';
import { PostCardComponent } from './_components/small-components/post-card/post-card.component';
import { FooterComponent } from './_components/footer/footer.component';
import { CreatePostComponent } from './_components/create-post/create-post.component';
import { LoginComponent } from './_components/login/login.component';
import { RegisterComponent } from './_components/register/register.component';
import { SharedModule } from './_modules/shared.module';
import { AngularMaterialModule } from './_modules/angular-material.module';
import { AppTextInputComponent } from './_components/small-components/app-text-input/app-text-input.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { NgxSpinnerModule } from "ngx-spinner";
import { BusyInterceptor } from './_interceptors/busy.interceptor';
import { PostDetailComponent } from './_components/post-detail/post-detail.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    PostCardComponent,
    FooterComponent,
    CreatePostComponent,
    LoginComponent,
    RegisterComponent,
    AppTextInputComponent,
    PostDetailComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule,
    NgxSpinnerModule,
    AngularMaterialModule,
    SharedModule,

  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: BusyInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
