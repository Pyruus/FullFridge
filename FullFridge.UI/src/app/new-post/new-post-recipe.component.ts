import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NewPostComponent } from './new-post.component';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-new-post',
  templateUrl: './new-post.component.html',
  styleUrls: ['./new-post.component.css']
})
export class NewPostRecipeComponent extends NewPostComponent implements OnInit {
  recipeId: any;

  constructor(http: HttpClient, router: Router, cookieService: CookieService, private route: ActivatedRoute) {
    super(http, router, cookieService);

  }

  ngOnInit(): void {
    this.recipeId = this.route.snapshot.paramMap.get('id');
  }

  override addNewPost(): void {
    this.post.recipeId = this.recipeId;
    this.post.createdBy = this.cookieService.get("userId");
    this.http.post(this.ROOT_URL + '/Forum', this.post).subscribe(() => {
      this.router.navigate(['forum']);
    });
  }
}