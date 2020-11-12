import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { clear } from 'console';
import { Observable } from 'rxjs';
import { MediaService } from 'src/app/services/media.service';
import { MediaDetailComponent } from '../dialogs/media-detail/media-detail.component';

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
    public dialog: MatDialog,
    private router: Router
  ) { }

  ngOnInit(): void {  
    let card: any = document.getElementsByTagName('mat-card') ; console.log(card);    
    const username = this.route.snapshot.params.id;
    this.usermedias(username);
  }
  usermedias(username) {
    this.ums$ = this.mediaService.usermedias(username);
    this.ums$.subscribe(res => {
      let title: any = document.getElementsByTagName('title')[0];
      title.innerText = `${res.appUser.fullName} (@${username}) • Photos et vidéos Instagram`;
    },
      () => {
        this.router.navigateByUrl('p/not-found');
        console.clear()
      });
  }

  getImageProfilePath(url) {
    if (url == null)
    return '../../../assets/no-img.png';
  return `${this.profileImgPath}/${url}`;
  }

  getImageMediaPath(url) {
    if (url == null)
      return '../../../assets/no-img.png';
    return `${this.imgPath}/${url}`;
  }

  onOpenModel(data) {
    console.log(data);
  }
}
