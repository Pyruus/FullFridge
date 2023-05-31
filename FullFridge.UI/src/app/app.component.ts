import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'FullFridge';
  faUser = faUser;
  faRightFromBracket = faRightFromBracket;

  constructor(private cookieService: CookieService, private router: Router){}

  isUserLogged(){
    return this.cookieService.check("userId");
  }

  redirectToLogin(){
    this.router.navigate([`login`]);
  }

  logout(){
    this.cookieService.delete("userId");
    this.cookieService.delete("userName");
    this.cookieService.delete("token");
  }
}
