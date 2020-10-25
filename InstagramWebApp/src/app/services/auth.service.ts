import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  url: string = 'https://localhost:44398/api/accounts'
  constructor(private http: HttpClient) { }

  login(data) {
    return this.http.post(`${this.url}/login`, data);
  }

  register(data) {
    return this.http.post(`${this.url}/register`, data);
  }

  logout() {
    localStorage.removeItem('token');
  }

  user() {
    return this.http.get(`${this.url}/user`);
  }

}
