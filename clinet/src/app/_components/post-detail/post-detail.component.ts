import { AfterViewChecked, AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Pagination, PaginationResult } from 'src/app/_interfaces/pagination';
import { Post } from 'src/app/_interfaces/Post';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.scss']
})
export class PostDetailComponent implements OnInit, AfterViewInit, AfterViewChecked {
  post: Post;
  userPosts: Post[] = [];

  clickedPostId: number;
  postId: number;
  postrName: string;

  //pagination
  pageNumber:number = 0; // it's update to 1 in the function ++this.pageNUmber
  pageSize:number = 5;
  pagination:Pagination;

  @ViewChild("postContent") postContentRef: ElementRef;

  constructor(private psotService: PostService, private route: ActivatedRoute,
    private router: Router) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false };
  }
  ngAfterViewChecked(): void {
    this.postContentRef.nativeElement.innerHTML = this.post?.postContent;

  }
  ngAfterViewInit(): void {

  }

  ngOnInit(): void {


    this.route.paramMap.subscribe(params => {
      this.postId = +params.get("id");
      this.postrName = params.get("userName");
    });

    this.getUserPost(this.postId);
    this.getUserPosts(this.postrName);




  }





  private getUserPosts(postrName: string) {
    debugger;
    this.psotService.getUserPosts(this.postrName, ++this.pageNumber, this.pageSize).subscribe((paginationResult: PaginationResult<Post[]>) => {
      let result = paginationResult.result.filter(x => {
        return x.id != this.postId;
      });
      this.userPosts = [...this.userPosts, ...result];
      this.pagination = paginationResult.pagination;
    })
  }
  private getUserPost(postId: number) {
    this.psotService.getPost(postId).subscribe((post: Post) => {
      this.post = post
    });
  }

  onPostClicked(postrName: string, postId: number) {
    this.router.navigateByUrl(`/post-detail/${postrName}/${postId}`);
  }

}
