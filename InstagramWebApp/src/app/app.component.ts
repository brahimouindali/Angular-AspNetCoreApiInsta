import { AfterViewInit, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor() {  }

  isAuth() {
    if (localStorage.getItem('token') != null)
      return true;
    return false;
  }

  ngOnInit(): void {
  }
}