import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from '../_interfaces/Post';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl = environment.apiUrl;
  constructor(private http:HttpClient) { }


  addPost(post : Post){
    return this.http.post<Post>(this.baseUrl + "posts", post);
  }
}
