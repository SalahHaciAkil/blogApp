import { HttpClient, HttpParams } from '@angular/common/http';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { getPaginationHeaders, getPaginationResult } from '../_helpers/PaginationRequest';
import { CreateComment } from '../_interfaces/createComment';
import { PaginationResult } from '../_interfaces/pagination';
import { Post } from '../_interfaces/Post';
import { User } from '../_interfaces/User';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  UserPostsChaches = new Map();
  postsChaches = new Map();

  baseUrl = environment.apiUrl;
  private postsThreadSource = new BehaviorSubject<Post[]>([]);
  postsThread$ = this.postsThreadSource.asObservable();
  posts: Post[] = [];

  private userPostsThreadSource = new BehaviorSubject<Post[]>([]);
  userPostsThread$ = this.userPostsThreadSource.asObservable();
  userPosts: Post[] = [];

  currentUser: User;

  constructor(private http: HttpClient, private accoutService: AccountService) {

    this.accoutService.currentUser$.subscribe(user => {
      this.currentUser = user;
    })

  }


  getPost(postId: number) {
    //search from all posts
    let postA = this.posts.find(p => p.id == postId);
    if (postA != undefined) return of(postA);


    //search from all current user posts
    let postB = this.userPosts.find(p => p.id == postId);
    if (postB != undefined) return of(postB);

    return this.http.get(`${this.baseUrl}posts/post-detail/${postId}`);

  }


  addPost(post) {
    return this.http.post<Post>(this.baseUrl + "posts", post).pipe(
      map(data => {
        debugger;
        const post = data;
        if (post) {
          this.posts.unshift(post);
          this.setCurrentPostSource(this.posts);

          if (this.UserPostsChaches.size > 0) {
            this.userPosts.unshift(post);
            this.setCurrentUserPostSource(this.userPosts);
          }

        }

        debugger;
        return data;
      })
    );
  }


  // https://localhost:5001/api/Posts/delete-post/7

  deletePost(postId) {
    return this.http.delete(`${this.baseUrl}posts/delete-post/${postId}`).pipe(
      map(() => {
        this.posts = this.posts.filter(x => x.id != postId);
        this.userPosts = this.userPosts.filter(x => x.id != postId);
        // this.deletePostFromUserPostChaces(postId);
        this.setCurrentPostSource(this.posts);
        this.setCurrentUserPostSource(this.userPosts);
      })
    )
  }

  getUserPosts(postrName: string, pageNumber: number, pageSize: number) {
    if(!postrName)return of({});

    let keyMap = `${postrName}_${pageNumber}_${pageSize}`;
    if (this.UserPostsChaches.has(keyMap)) {
      return of(this.UserPostsChaches.get(keyMap))
    }

    let httpPatams = getPaginationHeaders(pageNumber, pageSize);
    const paginationResult = getPaginationResult<Post[]>(this.http, `${this.baseUrl}posts/${postrName}`, httpPatams);

    return paginationResult.pipe(
      map(paginationResult => {
        if (this.currentUser?.userName == postrName) {
          this.userPosts = [...this.userPosts, ...paginationResult.result];
        }
        this.setCurrentUserPostSource(this.userPosts);
        this.UserPostsChaches.set(keyMap, paginationResult);
        return paginationResult;
      })
    );
  }

  // deletePostFromUserPostChaces(postId) {
  //   let userPosts: Post[] = [];
  //   this.UserPostsChaches.forEach((x: PaginationResult<Post[]>) => {
  //     let post = x.result.find(x => x.id == postId);
  //     if (post) {
  //       x.result = x.result.filter(x => x.id != postId);
  //     }


  //   });

  // }


  getPosts(pageNumber: number, pageSize: number) {

    let postsMapKey = this.getPostsMapKey(pageNumber, pageSize)
    if (this.postsChaches.has(postsMapKey)) {
      return of(this.postsChaches.get(postsMapKey));
    }
    debugger;
    let httpParams = getPaginationHeaders(pageNumber, pageSize);
    const paginationResult = getPaginationResult(this.http, `${this.baseUrl}posts`, httpParams);
    return paginationResult.pipe(
      map((paginationResult: PaginationResult<Post[]>) => {
        this.posts = [...this.posts, ...paginationResult.result];
        this.setCurrentPostSource(this.posts);
        this.postsChaches.set(this.getPostsMapKey(pageNumber, pageSize), paginationResult)
        return paginationResult;
      })
    )

  }
  getPostsMapKey(pageNumber: number, pageSize: number): string {
    let key = `${pageNumber}_${pageSize}`;
    return key;
  }

  // [HttpPost("add-like/{postId}")]
  // https://localhost:5001/api/Users/add-like/10


  likePost(postId: number) {
    return this.http.post(`${this.baseUrl}posts/add-like/${postId}`, {});
  }
  //https://localhost:5001/api/Posts/add-comment

  addComment(createComment: CreateComment) {
    return this.http.post(`${this.baseUrl}posts/add-comment`, createComment);
  }
  //https://localhost:5001/api/Posts/5
  deleteComment(commentId: number) {
    return this.http.delete(`${this.baseUrl}posts/delete-comment/${commentId}`);
  }


  //https://localhost:5001/api/Posts/like-activity

  loadLikeUserActivities() {
    return this.http.get(`${this.baseUrl}posts/like-activity`);
  }

  //https://localhost:5001/api/Posts/comment-activities

  loadCommentUserActivities() {
    return this.http.get(`${this.baseUrl}posts/comment-activities`);
  }

  setCurrentPostSource(posts: Post[]) {
    this.postsThreadSource.next(posts);
  }

  setCurrentUserPostSource(posts: Post[]) {
    this.userPostsThreadSource.next(posts);
  }
  unSetCurrentPostSource(posts: Post[]) {
    this.postsThreadSource.next(null);
  }

  //https://localhost:5001/api/Posts/edit-comment?commentId=6&newComment=something
  editComment(commentId: number, newComment: string) {
    return this.http.put(`${this.baseUrl}posts/edit-comment?commentId=${commentId}&newComment=${newComment}`, {});

  }
  //https://localhost:5001/api/Posts/edit-post

  editPost(formData: FormData) {
    return this.http.put(`${this.baseUrl}posts/edit-post`, formData).pipe(
      map((photo: Object) => {
        debugger;
        let post = this.posts.find(x => x.id == Number(formData.get("id")))
        post.photo = photo["photo"];
        return photo;
      })
    );
  }





}


