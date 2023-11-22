import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Post, Comment } from '../models/posts.model';

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

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    this.postId = this.route.snapshot.paramMap.get('id');
    this.fetchPostDetails();
    
  }

  fetchPostDetails(): void {
    this.http.get<Post>(this.ROOT_URL + `/Forum/${this.postId}`).subscribe(post => {
      this.post = post;
    });

    this.http.get<Comment[]>(this.ROOT_URL + `/Forum/${this.postId}/Comments`).subscribe(comments => {
      this.comments = comments;
    });
  }
}
