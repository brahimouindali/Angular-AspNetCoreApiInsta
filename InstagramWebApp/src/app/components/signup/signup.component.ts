import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { $ } from 'protractor';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  userForm: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.initForm();
  }
  initForm() {
    this.userForm = this.fb.group({
      firstName: [null, Validators.required],
      password: [null, Validators.required],
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
