import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MediaService } from 'src/app/services/media.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  // medias$;
  medias;
  user: any;
  now: any = new Date()
  showSpinner = true;

  constructor(private mediaService: MediaService) { }

  ngOnInit(): void {
    console.log(window.innerWidth);
    let title = document.getElementsByTagName('title')[0]
    title.innerHTML = 'Instagram(copy)';
    
    // this.medias$ = this.mediaService.medias();
     this.mediaService.medias().subscribe(res => {
       this.showSpinner = false
       this.medias = res
     })
  }

  onResize(e) {
    console.log(e.target.innerWidth)
  }
}
