import { HttpClient } from '@angular/common/http';
import { HtmlAstPath } from '@angular/compiler';
import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { delay, windowTime } from 'rxjs/operators';
import { Pagination, PaginationResult } from 'src/app/_interfaces/pagination';
import { Like, Post } from 'src/app/_interfaces/Post';
import { BusyService } from 'src/app/_services/busy.service';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  navFlag: boolean = false;
  searchFlag: boolean = false;

  pageNumber: number = 0;
  pageSize: number = 20;

  user: any;

  // posts: Array<Post> = [];
  pagination: Pagination;

  //Activities
  likes: Array<Like> = [];
  comments: Array<Comment> = [];
  constructor(private http: HttpClient,
    public postService: PostService, private router: Router) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false }
  }


  ngOnInit(): void {
    this.loadPosts();
  }


  loadPosts() {

    this.postService.getPosts(++this.pageNumber, this.pageSize).subscribe((paginationResult: PaginationResult<Post[]>) => {
      // this.posts = [...this.posts, ...paginationResult.result];
      this.pagination = paginationResult.pagination;
    })
  }

}
