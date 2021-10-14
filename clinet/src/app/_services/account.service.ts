import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators'
import { ReplaySubject } from 'rxjs';
import { User } from '../_interfaces/User';
@Injectable({
  providedIn: 'root'
})
export class AccountService {


  baseUrl: string = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient) { }


  register(model: any) {
    return this.http.post(this.baseUrl + "account/" + "register", model);
    // .pipe(
    //   map((respones: User) => {
    //     const user = respones
    //     if (user) {
    //       this.setCurrentUser(user)
    //     }

    //     return user;
    //   })
    // )
  }
  login(model: any) {
    return this.http.post(this.baseUrl + "account/" + "login", model).pipe(
      map((respones: User) => {
        const user = respones
        if (user) {
          this.setCurrentUser(user)
        }

        return user;
      })
    )
  }


  confirmEmail(model: any) {
    debugger;
    return this.http.post(this.baseUrl + "account/confirmemail", model);
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user)
  }

  logout() {
    this.currentUserSource.next(null);
    localStorage.removeItem('user');
  }


  //https://localhost:5001/api/Account/reset-password
  resetPassword(model) {
    debugger;
    return this.http.put(`${this.baseUrl}account/reset-password`, model);

  }

  //https://localhost:5001/api/Account/manage-password/qwd
  sendLinkToEmail(email: string) {
    debugger;
    return this.http.put(`${this.baseUrl}account/manage-password/${email}`, {});
  }
}
