import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'FullFridge';
  faUser = faUser;
  faRightFromBracket = faRightFromBracket;
  isMobile = false;

  constructor(private cookieService: CookieService, private router: Router){}

  ngOnInit(): void {
    if (window.screen.width === 360) { // 768px portrait
      window.onresize = () => this.isMobile= window.innerWidth <= 991;
    }
  }

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
    this.router.navigate([`login`]);
  }
}
