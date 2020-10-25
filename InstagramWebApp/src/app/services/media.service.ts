import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  url = 'https://localhost:44398/api/Medias'
  constructor(private http: HttpClient) { }

  
  medias() {
    return this.http.get<any[]>(this.url);
  }

  addMedia(newMedia) {
    return this.http.post(`${this.url}/media`, newMedia);
  }

  likeMedia(media) {
    return this.http.post(`${this.url}/likemedia`, media);
  }

  deslikeMedia(id) {
    return this.http.delete(`${this.url}/deslikemedia/${id}`);
  }

  updateMedia(media) {
    return this.http.patch(`${this.url}/media`, media);
  }
}
