import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ConfirmEmailComponent } from './_components/confirm-email/confirm-email.component';
import { CreatePostComponent } from './_components/create-post/create-post.component';
import { HomeComponent } from './_components/home/home.component';
import { LoginComponent } from './_components/login/login.component';
import { PostDetailComponent } from './_components/post-detail/post-detail.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'createPost/:postId', component: CreatePostComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'post-detail/:userName/:id', component: PostDetailComponent },
  { path: 'confirm-email', component: ConfirmEmailComponent },

  { path: '**', component: HomeComponent, pathMatch: "full" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
