import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  url = 'https://localhost:44398/api/Medias/media'
  constructor(private http: HttpClient) { }

  addMedia(newMedia) {
    return this.http.post(this.url, newMedia);
  }
}
