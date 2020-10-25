import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  medias: any[];
  user: any;
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.user().subscribe(result => {
      this.user = result;
    });
    this.authService.medias().subscribe(result => {
      this.medias = result;
    }, err =>
      console.error(err)
    );
  }

}
