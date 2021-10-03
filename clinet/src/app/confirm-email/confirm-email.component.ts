import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfirmEmailModel } from '../_interfaces/confirmEmailModel';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  confrimEmailModel:ConfirmEmailModel = {token:"", userId:""};
  state: string = "An activation link sent to your account, please click it to activate your account"
  constructor(private route: ActivatedRoute, private accountService: AccountService,
    private toast: ToastrService, private router: Router) { }

  ngOnInit(): void {
    if(!this.route.snapshot.queryParamMap.has('token'))return;
    this.confrimEmailModel.token = this.route.snapshot.queryParamMap.get('token');
    this.confrimEmailModel.userId = this.route.snapshot.queryParamMap.get('userId');
    this.confirmEmail()
  }
  confirmEmail() {
    this.accountService.confirmEmail(this.confrimEmailModel).subscribe(() => {
      debugger;
      this.toast.success("You account has been sunccessfully activated");
      this.state = "Your account has been successfully acivated";
      setTimeout(() => {
        this.router.navigateByUrl("/login");
      }, 2000)
    }, error => {
      this.toast.error("Error while confirming your account");
    })
  }

}
