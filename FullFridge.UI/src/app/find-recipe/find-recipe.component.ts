import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, NavigationExtras } from '@angular/router';

@Component({
  selector: 'app-find-recipe',
  templateUrl: './find-recipe.component.html',
  styleUrls: ['./find-recipe.component.css']
})
export class FindRecipeComponent {
  readonly ROOT_URL = 'https://localhost:7040/api';
  chosenProductsNames: string[] = [];
  chosenProductsIds: number[] = [];
  searchTerm: string = '';
  results: any[] = [];

  

  constructor(private http: HttpClient, private router: Router) {}

  search() {
    const params = {
      searchString: this.searchTerm
    }

    if (this.searchTerm.length >= 3) {
      this.http.get<any>(this.ROOT_URL + `/Product/Search`, { params }).subscribe(
        {
            next: response => {
            this.results = response;
          },
          error: error => {console.error('Error:', error)}
        }
      );
    } else {
      this.results = [];
    }
  }

  selectProduct(product: any) {
    this.searchTerm = '';
    this.results = [];
    if (this.chosenProductsIds.includes(product.id)){
      return;
    }
    this.chosenProductsNames.push(product.name);
    this.chosenProductsIds.push(product.id);
  }
  
  searchByProducts(){
    const params = {
      productIds: this.chosenProductsIds
    }

    if (this.chosenProductsIds.length > 0) {
      this.http.get<any>(this.ROOT_URL + `/Recipe/Products`, { params }).subscribe(
        {
          next: response => {
            const navigationExtras: NavigationExtras = {
              state: {
                foundRecipes: response
              }
            };
            this.router.navigate(['home'], navigationExtras);
          },
          error: error => console.error('Error:', error)
        }
      );
    }
  }
}
