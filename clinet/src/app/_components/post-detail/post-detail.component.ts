import { AfterViewChecked, AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { CreateComment } from 'src/app/_interfaces/createComment';
import { Pagination, PaginationResult } from 'src/app/_interfaces/pagination';
import { Comment, Post } from 'src/app/_interfaces/Post';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.scss']
})
export class PostDetailComponent implements OnInit, AfterViewChecked {
  post: Post;
  createComment: CreateComment = { postId: 0, comment: "" }
  userPosts: Post[] = [];
  userComments: Comment[] = [];

  clickedPostId: number;
  postId: number;
  postrName: string;

  //pagination
  pageNumber: number = 0; // it's update to 1 in the function ++this.pageNUmber
  pageSize: number = 5;
  pagination: Pagination;

  @ViewChild("postContent") postContentRef: ElementRef;

  constructor(private psotService: PostService, private route: ActivatedRoute,
    private router: Router) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => { return false };
  }
  ngAfterViewChecked(): void {
    this.postContentRef.nativeElement.innerHTML = this.post?.postContent;

  }

  ngOnInit(): void {


    this.route.paramMap.subscribe(params => {
      this.postId = +params.get("id");
      this.postrName = params.get("userName");
    });

    this.getUserPost(this.postId);
    this.getUserPosts();




  }

  addComment() {
    this.createComment.postId = this.postId;
    
    this.psotService.addComment(this.createComment).subscribe((comment:Comment)=>{
      this.post.comments.push(comment);
    })
  }



  private getUserPosts() {
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
      this.userComments = post.comments;
      debugger;
    });
  }

  onPostClicked(postrName: string, postId: number) {
    this.router.navigateByUrl(`/post-detail/${postrName}/${postId}`);
  }

}
