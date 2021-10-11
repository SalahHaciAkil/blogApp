import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Post } from 'src/app/_interfaces/Post';
import { User } from 'src/app/_interfaces/User';
import { AccountService } from 'src/app/_services/account.service';
import { PostService } from 'src/app/_services/post.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.scss']
})
export class PostCardComponent implements OnInit, AfterViewInit {
  @Input() post: Post;
  @ViewChild("lead") lead: ElementRef;
  likeClass: string = "fa fa-heart-o"
  trashClass: string = "fa fa-trash"
  user: User;



  constructor(public accountService: AccountService, private postSerivce: PostService,
    private toast: ToastrService) { }
  ngAfterViewInit(): void {
    this.lead.nativeElement.innerHTML = this.post.postContent?.substr(0, 30);

  }

  ngOnInit(): void {
    this.getCurrentUser();

  }


  do(postId) {
    Swal.fire({
      title: `Are you sure you want to delete your post with id of ${postId}?`,
      showDenyButton: true,
      confirmButtonText: `Yes`,
      denyButtonText: `No`,
    }).then((result) => {
      /* Read more about isConfirmed, isDenied below */
      if (result.isConfirmed) {
        this.postSerivce.deletePost(postId).subscribe(() => {
          this.toast.success("you have delete the post successfully");

        })

      } else if (result.isDenied) {
      }
    })
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


    this.likeClass = arr.includes(this.user.id) ? "fa fa-heart" : "fa fa-heart-o";
  }


  likePost(postId: number) {
    this.postSerivce.likePost(postId).subscribe(() => {
      this.likeClass = "fa fa-heart";
      this.toast.success(`You liked ${this.post.postrName} post`);
    }, error => {
      this.toast.error(error.error)
    })
  }


}
