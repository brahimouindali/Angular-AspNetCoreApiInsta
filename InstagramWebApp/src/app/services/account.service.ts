import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  url: string = 'https://localhost:44398/api/accounts';
  constructor(private http: HttpClient) { }

  followOrUnfollow(id) {
    return this.http.post(`${this.url}/followOrUnfollow`, { id: id })
  }

  users() {
    return this.http.get<any[]>(`${this.url}/users`);
  }
}
