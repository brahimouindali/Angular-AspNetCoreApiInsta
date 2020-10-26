import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {

  error: string = '';

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null)
      this.router.navigate(['/home'])
  }

  myForm: FormGroup = this.fb.group({
    'email': ['', Validators.required],
    'password': ['', Validators.compose([Validators.minLength(5), Validators.required, Validators.pattern("[a-zA-Z]+")])]
  });

  login(user) {
    this.authService.login(user).subscribe((data: any) => {
      localStorage.setItem('token', data.token)
      this.router.navigateByUrl('/home')
    },
      err => {
        this.error = err.error.message;
        // console.log(err.error.message);
      }
    );
  }

}
