import { Component, OnInit, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  readonly ROOT_URL = 'https://localhost:7040/api';
  recipes: any;
  searchValue: string = '';

  constructor(private router: Router, private http: HttpClient) { }

  redirectToRecipe(recipeId: string){
    this.router.navigate([`recipe/${recipeId}`]);
  }

  ngOnInit(): void {
    this.http.get(this.ROOT_URL + `/Recipe/Top`).subscribe(
      (response: any) => {
        this.recipes = response;
        console.log(this.recipes);
      },
      (error: any) => {
        console.error(error);
      }
    );
  }

  @HostListener('document:keydown.enter', ['$event'])
  onEnterKeyPress(event: KeyboardEvent) {
    this.searchRecipes(this.searchValue);
  }

  searchRecipes(_searchString: string): void {
    const params = {
      searchString: _searchString
    }
    this.http.get(this.ROOT_URL + `/Recipe/Search`, { params }).subscribe(
      (response: any) => {
        this.recipes = response;
        console.log(this.recipes);
      },
      (error: any) => {
        console.error(error);
      }
    );
  }
}
