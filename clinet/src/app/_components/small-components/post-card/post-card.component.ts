import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
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



  constructor(public accountService:AccountService) { }
  ngAfterViewInit(): void {
    this.lead.nativeElement.innerHTML = this.post.postContent?.substr(0, 30);
  }

  ngOnInit(): void {

  }


}
