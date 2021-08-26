import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup
  constructor(private fb: FormBuilder, public accountService: AccountService,
    private route: Router, public dialogRef: MatDialogRef<RegisterComponent>) { }

  ngOnInit(): void {
    this.initRegisterFrom()

    this.registerForm.controls["password"].valueChanges.subscribe(() => {
      this.registerForm.controls["confirmPassword"].updateValueAndValidity();
    })
  }

  initRegisterFrom() {
    this.registerForm = this.fb.group({
      userName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      knownAs: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(12)]],
      confirmPassword: ['', [Validators.required, this.matchValue("password")]]
    })
  }

  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }

  print() {
    console.log(this.registerForm.value);
    console.log(this.registerForm);

  }
  register() {
    this.accountService.register(this.registerForm.value).subscribe(user => {
      this.route.navigateByUrl("/home");
      this.dialogRef.close();

    }, error => {
      console.log(error);

    })
  }

  printPassword(pass) {

  }

}
