import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css'],
})
export class RecipeComponent implements OnInit{

  recipeId: any;
  readonly ROOT_URL = 'https://localhost:7040/api'
  recipe: any;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}
  

  ngOnInit(): void {
      this.recipeId = this.route.snapshot.paramMap.get('id');
      this.http.get(this.ROOT_URL + `/Recipe/${this.recipeId}`).subscribe(
        {
          next: response => {
            this.recipe = response;
            console.log(this.recipe); // Print the fetched data
          },
          error: error => console.error('Error:', error)
        }
      );
  }

}
