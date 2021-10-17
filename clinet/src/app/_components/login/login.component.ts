import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_interfaces/User';
import { AccountService } from 'src/app/_services/account.service';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  userName: string;
  password: string;
  wid: string = "50%"
  height:string ="auto"
  constructor(public dialog: MatDialog, private accountService: AccountService,
    private route: Router, private toast: ToastrService) { }

  openDialog() {
    if (window.innerWidth <= 633) { this.wid = "100%";this.height="400px" }
    else this.wid = "50%";
    console.log(window.innerWidth, this.wid, window.outerWidth);
    const dialogRef = this.dialog.open(RegisterComponent, { width: this.wid, height:this.height});
    return;

    // dialogRef.afterClosed().subscribe(result => {
    //   console.log(`Dialog result: ${result}`);
    // });
  }

  ngOnInit(): void {

  }

  login() {
    this.accountService.login({ 'userName': this.userName, 'password': this.password }).subscribe((user: User) => {
      this.route.navigateByUrl("/home");
    }, error => {
    })
  }

}

