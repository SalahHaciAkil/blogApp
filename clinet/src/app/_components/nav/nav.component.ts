import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Like, Post } from 'src/app/_interfaces/Post';
import { User } from 'src/app/_interfaces/User';
import { AccountService } from 'src/app/_services/account.service';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  user: User;

  //Activities
  likes: Array<Like> = [];
  comments: Array<Comment> = [];
  activityCount: number = 0;
  menuFlag: boolean = false;
  @ViewChild('menu') menuNotifications: ElementRef;

  constructor(public accountService: AccountService, private router: Router, private postService: PostService) {

  }

  ngOnInit(): void {
    this.getCurrentUser();
    this.setUser();
    this.loadActivities();

  }
  getCurrentUser() {
    this.accountService.currentUser$.subscribe((user: User) => {
      this.user = user;
    })
  }

  setUser() {
    this.accountService.currentUser$.subscribe(user => {
      this.user = user
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/");


  }

  displayMenuNotification() {
    if (!this.menuFlag) {
      this.menuFlag = true;
      this.menuNotifications.nativeElement.style.visibility = "visible";
      this.menuNotifications.nativeElement.style.transform = "rotateY(0deg)";
    } else {
      this.menuFlag = false;
      this.menuNotifications.nativeElement.style.visibility = "hidden";
      this.menuNotifications.nativeElement.style.transform = "rotateY(90deg)";
    }

    this.activityCount = 0;


  }

  loadActivities() {
    let likeActivies = this.postService.loadLikeUserActivities();
    let commentActivies = this.postService.loadCommentUserActivities();

    forkJoin([likeActivies, commentActivies]).subscribe(results => {
      this.likes = results[0] as Like[];
      this.comments = results[1] as Comment[];
      debugger;
      this.activityCount = (this.likes ? this.likes.length : 0) + (this.comments ? this.comments.length : 0);


    });


  }

  //  getUserPost(postId: number) {
  //   this.postService.getPost(postId).subscribe((post: Post) => {
  //     this.post = post
  //     this.userComments = post.comments;
  //   });
  // }


}
