import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MediaService } from 'src/app/services/media.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  medias: any[];
  user: any;
  now: any = new Date()

  constructor(
    private mediaService: MediaService
  ) { }

  ngOnInit(): void {
    this.mediaService.medias().subscribe(result => {
      this.medias = result;
    }, err =>
      console.error(err)
    );
  }


}
