import { HttpClient, HttpParams } from '@angular/common/http';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { getPaginationHeaders, getPaginationResult } from '../_helpers/PaginationRequest';
import { PaginationResult } from '../_interfaces/pagination';
import { Post } from '../_interfaces/Post';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  postsCaches = new Map();

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
        debugger;
        if (post) {
          this.posts.unshift(post);
          this.setCurrentPostSource(this.posts);
        }
        return data;
      })
    );
  }

  getUserPosts(postrName: string, pageNumber: number, pageSize: number) {
    let keyMap = `${postrName}_${pageNumber}_${pageSize}`;
    if (this.postsCaches.has(keyMap)) {
      return of(this.postsCaches.get(keyMap))
    }

    let httpPatams = getPaginationHeaders(pageNumber, pageSize);
    const paginationResult = getPaginationResult<Post[]>(this.http, `${this.baseUrl}posts/${postrName}`, httpPatams);
    debugger;
    return paginationResult.pipe(
      map(paginationResult => {
        this.postsCaches.set(keyMap, paginationResult);
        return paginationResult;
      })
    );
    // return this.http.get(`${this.baseUrl}/${postrName}`, { observe: 'response', params: httpPatams });
  }


  getPosts() {


    if (this.posts.length != 0) return of(this.posts);

    return this.http.get<Post[]>(this.baseUrl + "posts").pipe(
      map((data: Post[]) => {
        const posts = data;
        debugger;
        if (posts) {
          this.posts = posts
          this.setCurrentPostSource(posts);
        }

        return data;
      })
    )
  }


  setCurrentPostSource(posts: Post[]) {
    this.postsThreadSource.next(posts);
  }
  unSetCurrentPostSource(posts: Post[]) {
    this.postsThreadSource.next(null);
  }
}
