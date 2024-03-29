import { Component, OnInit } from '@angular/core';
import { Pagination, PaginationResult } from 'src/app/_interfaces/pagination';
import { Post } from 'src/app/_interfaces/Post';
import { User } from 'src/app/_interfaces/User';
import { AccountService } from 'src/app/_services/account.service';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-user-posts',
  templateUrl: './user-posts.component.html',
  styleUrls: ['./user-posts.component.scss']
})
export class UserPostsComponent implements OnInit {

  //============= pageSize nad pageNumber MUST be the same values
  // at UserPostComponent, PostDetaileComponent and in addPost function in postService =============
  pageNumber: number = 0;
  pageSize: number = 5;
  user: User;

  pagination: Pagination;
  // userPosts: Post[] = [];


  constructor(public postService: PostService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(user => {
      this.user = user;
      if (user) // in case of guest 
        this.getUserPosts(this.user.userName);
    })


  }
  getUserPosts(userName: string) {

    this.postService.getUserPosts(userName, ++this.pageNumber, this.pageSize)
      .subscribe((paginationResult: PaginationResult<Post[]>) => {
        this.pagination = paginationResult.pagination;
      })
  }

}
