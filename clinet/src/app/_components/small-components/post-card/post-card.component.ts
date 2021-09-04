import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Post } from 'src/app/_interfaces/Post';
import { User } from 'src/app/_interfaces/User';
import { AccountService } from 'src/app/_services/account.service';
import { PostService } from 'src/app/_services/post.service';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.scss']
})
export class PostCardComponent implements OnInit, AfterViewInit {
  @Input() post: Post;
  @ViewChild("lead") lead: ElementRef;
  class: string = "fa fa-heart-o"
  user: User;



  constructor(public accountService: AccountService, private postSerivce: PostService,
    private toast: ToastrService) { }
  ngAfterViewInit(): void {
    this.lead.nativeElement.innerHTML = this.post.postContent?.substr(0, 30);

  }

  ngOnInit(): void {
    debugger
    this.getCurrentUser();

  }

  getCurrentUser() {
    this.accountService.currentUser$.subscribe((user: User) => {
      if (user != null) {
        this.user = user;
        this.checkLikedPost();

      }
    })
  }

  checkLikedPost() {

    let arr: number[] = [];
    for (const post of this.post.likedBy) {
      arr.push(post.userId);
    }


    this.class = arr.includes(this.user.id) ? "fa fa-heart" : "fa fa-heart-o";
  }


  likePost(postId: number) {
    this.postSerivce.likePost(postId).subscribe(() => {
      this.class = "fa fa-heart";
      this.toast.success(`You liked ${this.post.postrName} post`);
    }, error => {
      this.toast.error(error)
    })
  }


}
