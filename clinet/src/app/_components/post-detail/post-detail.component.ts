import { AfterViewChecked, AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from 'src/app/_interfaces/Post';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.scss']
})
export class PostDetailComponent implements OnInit, AfterViewInit, AfterViewChecked {
  post: Post;
  @ViewChild("postContent") postContentRef: ElementRef;

  constructor(private psotService: PostService, private router: ActivatedRoute) { }
  ngAfterViewChecked(): void {
    this.postContentRef.nativeElement.innerHTML = this.post?.postContent;

  }
  ngAfterViewInit(): void {

  }

  ngOnInit(): void {
    let postId = +this.router.snapshot.params['id'];
    this.psotService.getPost(postId).subscribe((post: Post) => {
      this.post = post
    })
  }

}
