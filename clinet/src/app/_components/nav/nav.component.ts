import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/_interfaces/User';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  user: User;
  constructor(public accountService: AccountService, private router:Router) { }

  ngOnInit(): void {
    this.getCurrentUser();
    this.setUser();
  }
  getCurrentUser() {
    this.accountService.currentUser$.subscribe((user: User) => {
      this.user = user;
    })
  }

  setUser(){
    this.accountService.currentUser$.subscribe(user =>{
      this.user = user
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/");


  }

}
