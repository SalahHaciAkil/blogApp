import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { config } from "../../node_modules/@fortawesome/fontawesome-free/"
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'clinet';


  constructor(private accountService: AccountService) {
  }
  ngOnInit(): void {
    this.setCurrentUser()
  }

  setCurrentUser() {
    const user = JSON.parse(localStorage.getItem('user'));
    if (user) {
      this.accountService.setCurrentUser(user);
    } else {
      this.accountService.setCurrentUser(null);

    }
  }
}
