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
    // return this.http.get(`${this.baseUrl}/${postrName}`, { observe: 'response', params: httpPatams });
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



    // if (this.posts.length != 0) return of(this.posts);

    // return this.http.get<PaginationResult<Post[]>>(this.baseUrl + "posts").pipe(
    //   map((paginationResult: PaginationResult<Post[]>) => {
    //     const posts = data;
    //     
    //     if (posts) {
    //       this.posts = posts
    //       this.setCurrentPostSource(posts);
    //     }

    //     return data;
    //   })
    // )
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
    return this.http.delete(`${this.baseUrl}posts/${commentId}`);
  }


  setCurrentPostSource(posts: Post[]) {
    this.postsThreadSource.next(posts);
  }
  unSetCurrentPostSource(posts: Post[]) {
    this.postsThreadSource.next(null);
  }
}
