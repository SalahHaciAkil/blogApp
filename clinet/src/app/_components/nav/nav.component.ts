import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_interfaces/User';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  user:User;
  constructor(public accountService:AccountService) { }

  ngOnInit(): void {
    this.getCurrentUser();
  }
  getCurrentUser() {
    this.accountService.currentUser$.subscribe((user:User)=>{
      this.user = user;
    })
  }

  logout(){
    this.accountService.logout();
  }

}
