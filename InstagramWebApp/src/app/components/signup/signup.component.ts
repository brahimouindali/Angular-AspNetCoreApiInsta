import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { $ } from 'protractor';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  userForm: FormGroup;
  hide = true;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (localStorage.getItem('token') != null)
      this.router.navigateByUrl('/home')
    else
      this.initForm();
  }
  initForm() {
    this.userForm = this.fb.group({
      firstName: [null, Validators.required],
      password: [null, [Validators.required, Validators.pattern(/[0-9a-zA-Z]{6,}/)]],
      lastName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      userName: [null, Validators.required]
    });
  }

  onSubmitForm() {
    const formValue = this.userForm.value;
    this.authService.register(formValue)
      .subscribe(result => {
        this.userForm.reset();
        console.log(result);
      });
  }

}
