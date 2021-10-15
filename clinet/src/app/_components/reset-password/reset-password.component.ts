import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { resetPasswordViewModel } from 'src/app/_interfaces/resetPasswordViewModel';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  registerForm: FormGroup

  resetPasswordViewModel: resetPasswordViewModel = { token: "", userId: "", newPassword: "" };
  resetPasswordFiels: boolean = false;

  constructor(private toast: ToastrService,
    private fb: FormBuilder,
    private router: ActivatedRoute,
    private accountService: AccountService,
    private route: Router) {
    this.route.routeReuseStrategy.shouldReuseRoute = () => { return false }
  }

  ngOnInit(): void {
    let isThereToken = this.router.snapshot.queryParamMap.has("token");
    if (isThereToken) {
      this.resetPasswordFiels = true;
      this.resetPasswordViewModel.userId = this.router.snapshot.queryParamMap.get("userId");
      this.resetPasswordViewModel.token = this.router.snapshot.queryParamMap.get("token");
    }

    this.initRegisterFrom();
    this.registerForm.controls["newPassword"].valueChanges.subscribe(() => {
      this.registerForm.controls["confirmPassword"].updateValueAndValidity();
    })
  }

  initRegisterFrom() {
    this.registerForm = this.fb.group({
      newPassword: ['', [Validators.pattern(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/), Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValue("newPassword")]],
      email: ['', [Validators.required, Validators.email]],
    })
  }

  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }

  resetPasswrod() {
    debugger;
    if (this.resetPasswordFiels)
      this.createNewPassword()
    else if (!this.resetPasswordFiels)
      this.sendLinkToEmail()
  }

  sendLinkToEmail() {
    let email: string = this.registerForm.get("email").value;
    this.accountService.sendLinkToEmail(email).subscribe(() => {
      this.toast.info(`We will send a reset-password link to ${email}, please click that link`, "Check your inbox")
    })
  }

  createNewPassword() {

    this.resetPasswordViewModel.newPassword = this.registerForm.get("newPassword").value;
    debugger;
    this.accountService.resetPassword(this.resetPasswordViewModel).subscribe(() => {
      this.toast.success("Your password was updated successfully");
      setTimeout(() => {
        this.route.navigateByUrl("/login");
      }, 1000)
    })
  }

}
