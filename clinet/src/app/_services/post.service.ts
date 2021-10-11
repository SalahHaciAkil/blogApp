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

  constructor(private http: HttpClient) { }


  getPost(postId: number) {
    let post = this.posts.find(p => p.id == postId);
    if (post != undefined) return of(post);

    return this.http.get(`${this.baseUrl}posts/post-detail/${postId}`);

  }


  addPost(post) {
    return this.http.post<Post>(this.baseUrl + "posts", post).pipe(
      map(data => {
        const post = data;
        if (post) {
          this.posts.unshift(post);
          this.setCurrentPostSource(this.posts);
        }

        debugger;
        return data;
      })
    );
  }

  deletePost(postId) {
    return this.http.delete(`${this.baseUrl}posts/delete-post/${postId}`).pipe(
      map(() => {
        this.posts = this.posts.filter(x => x.id != postId);
        this.setCurrentPostSource(this.posts);
      })
    )
  }

  getUserPosts(postrName: string, pageNumber: number, pageSize: number) {
    let keyMap = `${postrName}_${pageNumber}_${pageSize}`;
    if (this.UserPostsChaches.has(keyMap)) {
      return of(this.UserPostsChaches.get(keyMap))
    }

    let httpPatams = getPaginationHeaders(pageNumber, pageSize);
    const paginationResult = getPaginationResult<Post[]>(this.http, `${this.baseUrl}posts/${postrName}`, httpPatams);

    return paginationResult.pipe(
      map(paginationResult => {
        this.UserPostsChaches.set(keyMap, paginationResult);
        return paginationResult;
      })
    );
  }


  getPosts(pageNumber: number, pageSize: number) {

    let postsMapKey = this.getPostsMapKey(pageNumber, pageSize)
    if (this.postsChaches.has(postsMapKey)) {
      return of(this.postsChaches.get(postsMapKey));
    }

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
  unSetCurrentPostSource(posts: Post[]) {
    this.postsThreadSource.next(null);
  }

  //https://localhost:5001/api/Posts/edit-comment?commentId=6&newComment=something
  editComment(commentId: number, newComment: string) {
    return this.http.put(`${this.baseUrl}posts/edit-comment?commentId=${commentId}&newComment=${newComment}`, {});

  }

  // https://localhost:5001/api/Posts/delete-post/7


}
