import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Post, Comment, NewComment } from '../models/posts.model';
import { Router } from '@angular/router';
import { AppComponent } from '../app.component';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.css']
})
export class PostDetailsComponent implements OnInit {
  readonly ROOT_URL = 'https://localhost:7040/api'
  postId: any;
  post: Post | undefined;
  comments: Comment[] | undefined;
  newComment: NewComment | undefined;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router,
    protected appComponent: AppComponent,
    private cookieService: CookieService
  ) { }

  ngOnInit(): void {
    this.postId = this.route.snapshot.paramMap.get('id');
    this.fetchPostDetails();
    this.newComment = {
      content: "",
      postId: this.postId,
      createdBy: this.cookieService.get("userId")
    }
  }

  fetchPostDetails(): void {
    this.http.get<Post>(this.ROOT_URL + `/Forum/${this.postId}`).subscribe(post => {
      this.post = post;
    });

    this.http.get<Comment[]>(this.ROOT_URL + `/Forum/${this.postId}/Comments`).subscribe(comments => {
      this.comments = comments;
    });
  }

  redirectToRecipe(): void{
    this.router.navigate([`recipe/${this.post?.recipeId}`])
  }

  commentPost(): void{
    this.http.post(this.ROOT_URL+ '/Forum/Comment', this.newComment).subscribe(() => {
      this.fetchPostDetails();
      if(this.newComment){
        this.newComment.content = "";
      }
    });
  }
}
