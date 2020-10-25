import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  url: string = 'https://localhost:44398/api/comments'
  constructor(private http: HttpClient) { }

  addComment(comment) {
    return this.http.post(this.url, comment);
  }
}
