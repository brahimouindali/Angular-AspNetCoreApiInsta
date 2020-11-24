import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { MediaService } from 'src/app/services/media.service';
import { Urls } from 'src/app/SETTINGS/URLS';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  imgSrc: string = '';
  ums$;
  gutterSize = '1%';

  constructor(
    private mediaService: MediaService,
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private router: Router
  ) { }
  
  ngOnInit(): void {
    this.route.params.subscribe(routeParams => {
      this.usermedias(routeParams.id);
    })
    // let username = this.route.snapshot.params.id;
    // this.usermedias(username);
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

  onResize(event) {
    if (event.target.innerWidth < 689) {
      this.gutterSize = '-12px';
    } else {
      this.gutterSize = '1%';
    }
    // console.log(event.target.innerWidth);    
  }

  getImageProfilePath(url) {
    if (url == null)
      return Urls.noImg;
    return `${Urls.profilePath}${url}`;
  }

  getVideoPath(url) {
    return `${Urls.videoPath}${url}`;
  }

  getImageMediaPath(url) {
    return `${Urls.imagePath}${url}`;
  }

  onOpenModel() {
    // console.log(data);
  }
}
