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
  formData = new FormData();

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
      confirmPassword: ['', [Validators.required, this.matchValue("password")]],
      photo: ['', Validators.required]
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


  onFileChange(files) {
    console.log("change");
    let fileToUpload = <File>files[0];
    this.registerForm.patchValue({
      photo: fileToUpload
    });


  }

  register() {
    this.addToForm();
    this.accountService.register(this.formData).subscribe(user => {
      this.route.navigateByUrl("/home");
      this.dialogRef.close();

    }, error => {
      console.log(error);

    })
  }

  private addToForm() {
    this.formData.append('photo', this.registerForm.get('photo').value);
    this.formData.append('confirmPassword', this.registerForm.get('confirmPassword').value);
    this.formData.append('password', this.registerForm.get('password').value);
    this.formData.append('knownAs', this.registerForm.get('knownAs').value);
    this.formData.append('email', this.registerForm.get('email').value);
    this.formData.append('userName', this.registerForm.get('userName').value);
  }

  printPassword(pass) {

  }

}