import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  icon: any;
  input: any;
  value = '';
  data$: any;
  d: any;
  imagePath = 'https://localhost:44398/profile/';
  no_img = '../../../assets/no-img.png'

  constructor(
    private accountService: AccountService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initDomElement();
    this.data$ = this.accountService.users();
  }

  getImagePath(path) {
    if (path != null)
      return this.imagePath + path;
    return this.no_img;
  }

  keyPress(event: KeyboardEvent) {
    // this.value += event.key;
    // console.log(this.value);
  }

  initDomElement() {
    this.icon = document.getElementById('icon');
    this.input = document.getElementById('_input');
  }

  onInputFocus() {
    if (this.value == '') {
      this.input.classList.add('br-0');
    } else {
      this.input.classList.remove('br-0');
    }
    this.icon.classList.add('w-12');
    this.input.classList.add('w-88');
  }

  onInputBlur() {
    if (this.value == '') {
      this.icon.classList.remove('w-12');
      this.input.classList.remove('w-88');
      this.input.classList.remove('br-0');
    } else {
      this.input.classList.add('br-0');
    }
  }

  onClearInput() {
    this.value = '';
    this.input.classList.remove('br-0');
  }

  logout() {
    this.authService.logout();
  }

}
