import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

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

  

  constructor(private http: HttpClient) {}

  search() {
    const params = {
      searchString: this.searchTerm
    }

    if (this.searchTerm.length >= 3) {
      this.http.get<any>(this.ROOT_URL + `/Product/Search`, { params }).subscribe(
        (data) => {
          console.log(data);
          this.results = data;
        },
        (error) => {
          console.error('Error:', error);
        }
      );
    } else {
      this.results = [];
    }
  }

  selectProduct(product: any) {
    this.chosenProductsNames.push(product.name);
    this.chosenProductsIds.push(product.id);
    console.log(this.chosenProductsIds);
    console.log(this.chosenProductsNames);
  }
}
