import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';
import { AuthService } from 'src/app/services/auth.service';
import { Urls } from 'src/app/SETTINGS/URLS';
import { FormControl } from '@angular/forms';


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
  usersList;
  filteredUsers: Observable<User[]>

  constructor(
    private accountService: AccountService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.initDomElement();
    this.data$ = this.accountService.users();
    this.accountService.users().subscribe((res: any) => {
      this.usersList = res.appUsers;
      this.filteredUsers = this.control.valueChanges.pipe(
        startWith(''),
        map(val => this._filter(val))
      );
    });
  }

  getImagePath(path) {
    if (path != null)
      return Urls.profilePath + path;
    return Urls.noImg;
  }

  keyDown(event: KeyboardEvent) {
    this.value += event.key;
    // console.log(this.value);
  }


  control = new FormControl();

  private _filter(value: string): User[] {
    const filterValue = this._normalizeValue(value);
    return this.usersList.filter(user => this._normalizeValue(user.userName).includes(filterValue));
  }

  private _normalizeValue(value: string): string {
    return value != null ? value.toLowerCase().replace(/\s/g, '') : '';
  }


  initDomElement() {
    this.icon = document.getElementById('icon');
    this.input = document.getElementById('_input');
  }

  
  getUserPath(url) {
    
  }

  onInputFocus() {
    if (this.control.value == '') {
      this.input.classList.add('br-0');
    } else {
      this.input.classList.remove('br-0');
    }
    this.icon.classList.add('w-12');
    this.input.classList.add('w-88');
  }

  onInputBlur() {
    if (this.control.value == '') {
      this.icon.classList.remove('w-12');
      this.input.classList.remove('w-88', 'br-0');
    } else {
      this.input.classList.add('br-0');
    }
  }

  onClearInput() {
    this.control.reset()
    this.input.classList.remove('br-0');
  }

  logout() {
    this.authService.logout();
  }

}
