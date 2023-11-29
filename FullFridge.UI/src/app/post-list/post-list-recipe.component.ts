import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Post, Posts } from '../models/posts.model';
import { ActivatedRoute, Router } from '@angular/router';
import { PostListComponent } from './post-list.component';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css']
})
export class PostListRecipeComponent extends PostListComponent{
  recipeId : any;

  constructor(http: HttpClient, router: Router, private route: ActivatedRoute) {
    super(http, router);
  }

  override ngOnInit(): void {
    this.recipeId = this.route.snapshot.paramMap.get('id');
    this.http.get<Posts>(this.ROOT_URL + `/Forum/Recipe/${this.recipeId}?page=${this.currentPage}&pageSize=${this.pageSize}`).subscribe(
      {
        next: response => {
          this.posts = response.posts;
          this.pages = response.pages;
        },
        error: error=> console.error(error)
      }
    );
  }

  override addPost(): void{
    this.router.navigate([`add-post/recipe/${this.recipeId}`]);
  }

}
