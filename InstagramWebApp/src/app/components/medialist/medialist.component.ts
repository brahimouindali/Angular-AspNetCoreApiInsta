import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MediaService } from 'src/app/services/media.service';
import { ManagePostComponent } from '../dialogs/manage-post/manage-post.component';
import { SubscribeusersComponent } from '../dialogs/subscribeusers/subscribeusers.component';

@Component({
  selector: 'app-medialist',
  templateUrl: './medialist.component.html',
  styleUrls: ['./medialist.component.css']
})
export class MedialistComponent implements OnInit {

  @Input() m: any;
  user: any;
  now: any = new Date()
  imgPath: string = 'https://localhost:44398/img/';
  videoPath: string = 'https://localhost:44398/video/';
  imgProfilePath = 'https://localhost:44398/profile/';
  noImg = '../../../assets/no-img.png';

  constructor(
    private mediaService: MediaService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    // console.log(this.m.media.appUser.id); // follow or undollow   
  }


  LikeMedia(id) {
    if (!this.m.isLiked) {
      let media = { id: id };
      this.mediaService.likeMedia(media)
      this.m.isLiked = true;
      this.m.countLikes++;

      this.m.meFollowUsersList.push({
        appUser: this.m.appUser,
        isMe: true,
        isFollowedMe: false
      });
    }
  }

  LikeOrDesLikeMedia(id) {
    if (this.m.isLiked) {
      this.mediaService.deslikeMedia(id);
      this.m.isLiked = false;
      this.m.countLikes--;
      this.m.meFollowUsersList.pop({
        appUser: this.m.appUser,
        isMe: true,
        isFollowedMe: false
      });
    } else {
      this.LikeMedia(id);
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

  onNotifyComment(comment) {
    this.m.comments.push(comment)
  }

  getImagePath(url) {
    return url == null ? this.noImg : this.imgProfilePath + url;
  }

  // click more_horiz icon
  onModelOpen() {
    this.dialog.open(ManagePostComponent, {
      width: "460px",
      data: this.m.media.appUser.id
    })
  }

  // click likers
  onUserModel() {
    this.dialog.open(SubscribeusersComponent, {
      width: "460px",
      height: "auto",
      data: this.m.meFollowUsersList
    })
  }
}
