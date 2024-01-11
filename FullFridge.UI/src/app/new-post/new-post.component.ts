import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Post } from '../models/new-post.model';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-new-post',
  templateUrl: './new-post.component.html',
  styleUrls: ['./new-post.component.css']
})
export class NewPostComponent {
  post = {} as Post; 
  readonly ROOT_URL = 'https://localhost:7040/api'

  constructor(protected http: HttpClient, protected router: Router, protected cookieService: CookieService) { }

  addNewPost(): void {
    this.post.createdBy = this.cookieService.get("userId");
    this.http.post(this.ROOT_URL+ '/Forum', this.post).subscribe(() => {
      this.router.navigate(['forum']);
    });
  }
}