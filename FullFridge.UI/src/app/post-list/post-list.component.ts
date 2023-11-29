import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Post, Posts } from '../models/posts.model';
import { Router } from '@angular/router';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css']
})
export class PostListComponent implements OnInit{
  readonly ROOT_URL = 'https://localhost:7040/api';
  posts : Post[] | null;
  pages: number;
  pageSize = 10;
  currentPage = 1;

  constructor(protected http: HttpClient, protected router: Router, protected appComponent: AppComponent) {
    this.pages = 1;
    this.posts = null;
  }

  ngOnInit(): void {
    this.http.get<Posts>(this.ROOT_URL + `/Forum?page=${this.currentPage}&pageSize=${this.pageSize}`).subscribe(
      {
        next: response => {
          this.posts = response.posts;
          this.pages = response.pages;
        },
        error: error=> console.error(error)
      }
    );
  }

  setPage(pageNumber: number): void {
    this.currentPage = pageNumber;
    this.ngOnInit();
  }

  get totalPages(): number {
    if(this.posts == undefined) return 0;
    else return this.pages;
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

  redirectToPost(postId: string): void{
    this.router.navigate([`post/${postId}`]);
  }

  addPost(): void{
    this.router.navigate([`add-post`])
  }

}
