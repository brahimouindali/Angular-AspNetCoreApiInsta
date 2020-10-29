import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { MediaService } from 'src/app/services/media.service';
import { MediaDetailComponent } from '../media-detail/media-detail.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  profileImgPath: string = 'https://localhost:44398/profile';
  imgPath: string = 'https://localhost:44398/img';
  imgSrc: string = '';
  ums$;

  constructor(
    private mediaService: MediaService,
    private route: ActivatedRoute,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    const username = this.route.snapshot.params.id;
    this.usermedias(username);
  }
  usermedias(username) {
    this.ums$ = this.mediaService.usermedias(username);
    this.ums$.subscribe(ums => {
      let title: any = document.getElementsByTagName('title')[0];
      title.innerText = `${ums.appUser.fullName} (@${username}) • Photos et vidéos Instagram`;
      if (ums.appUser.imagePath == null) {
        this.profileImgPath = '../../../assets';
        ums.appUser.imagePath = 'profile.png';
        this.imgSrc = `${this.profileImgPath}/${ums.appUser.imagePath}`
      } else {
        this.imgSrc = `${this.profileImgPath}/${ums.appUser.imagePath}`
      }
    });
  }

  openDialog(){
    this.dialog.open(MediaDetailComponent);
  }

  onOpenModel(data, index) {
    let media = data.medias[index];
    const dialogRef = this.dialog.open(MediaDetailComponent, {
      width: '300px',
      data: { user: data.appUser, media: media }
    })

    dialogRef.afterClosed().subscribe(() => {
      console.log(`The dialog was closed`);
    });
  }


}
