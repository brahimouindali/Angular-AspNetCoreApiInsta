import { Component, Input, OnInit } from '@angular/core';
import { MediaService } from 'src/app/services/media.service';

@Component({
  selector: 'app-medialist',
  templateUrl: './medialist.component.html',
  styleUrls: ['./medialist.component.css']
})
export class MedialistComponent implements OnInit {

  @Input() m: any;
  user: any;
  now: any = new Date()
  imgPath: string = 'https://localhost:44398/img';

  constructor(
    private mediaService: MediaService
  ) { }

  ngOnInit(): void {
  }

  
  LikeImage(id) {
    let media = { id: id };
    if (!this.m.isLiked) {
      this.mediaService.likeMedia(media).subscribe(() => {
        this.m.isLiked = true;
      });
    }
  }

  LikeOrDesLikeImage(id) {
    if (this.m.isLiked) {
      this.mediaService.deslikeMedia(id).subscribe(() => {
        this.m.isLiked = false;
      });
    } else {
      this.LikeImage(id);
    }
  }

  dateDiff(date1) {
    let oneDay = 1 * 1000 * 60 * 60 * 24 * 365;
    let dt1 = new Date(date1);
    let date = (Date.UTC(this.now.getFullYear(), this.now.getMonth(), this.now.getDate(), this.now.getHours(), this.now.getMinutes(), this.now.getSeconds())
      - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate(), dt1.getHours(), dt1.getMinutes(), dt1.getSeconds()));


    if (Math.floor(date / oneDay) > 0) {
      return dt1.toDateString()
    } else {
      oneDay /= 365;
      if (Math.floor(date / oneDay) > 0) {
        return Math.floor(date / oneDay) > 1
          ? 'IL Y A ' + Math.floor(date / oneDay) + ' JOURS'
          : 'IL Y A ' + Math.floor(date / oneDay) + ' JOUR';
      } else {
        oneDay /= 24;
        if (Math.floor(date / oneDay) > 0) {
          return 'IL Y A ' + Math.floor(date / oneDay) + ' h';
        } else {
          oneDay /= 60;
          if (Math.floor(date / oneDay) > 0) {
            return 'IL Y A ' + Math.floor(date / oneDay) + ' min';
          } else {
            oneDay /= 60;
            return 'IL Y A ' + Math.floor(date / oneDay) + ' s';
          }
        }
      }
    }
  }
}
