import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Pagination, PaginationResult } from 'src/app/_interfaces/pagination';
import { Post } from 'src/app/_interfaces/Post';
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

  posts: Array<Post> = [];
  pagination: Pagination;
  // paginationResult:PaginationResult<Post[]> = new PaginationResult<Post[]>() ;

  constructor(private http: HttpClient, public postService: PostService) { }

  ngOnInit(): void {
    this.loadPosts();
  }

  loadPosts() {
    this.postService.getPosts(++this.pageNumber, this.pageSize).subscribe((paginationResult: PaginationResult<Post[]>) => {
      this.posts = [...this.posts, ...paginationResult.result];
      this.pagination = paginationResult.pagination;

      for (const post of this.posts) {
        console.log(post.id);

      }
    })
  }

}
