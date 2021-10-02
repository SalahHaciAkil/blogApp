import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_interfaces/User';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private toast: ToastrService, private router:Router) {

  }


  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user: User) => {
        if (user) return true;
        this.toast.info("You need to login to create a post").onTap.subscribe(()=>{
          this.router.navigateByUrl("/login");
        })
        return false;
      })
    )
  }

}
