import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { FileUploadService } from '../file-upload.service';

@Component({
  selector: 'app-add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.css']
})
export class AddRecipeComponent {
  myForm: FormGroup;
  selectedFile: File | undefined | null;
  postedRecipeId: any = -1;
  chosenProductsNames: string[] = [];
  chosenProductsIds: number[] = [];
  searchTerm: string = '';
  results: any[] = [];

  readonly ROOT_URL = 'https://localhost:7040/api'

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private cookieService: CookieService, private fileUploadService: FileUploadService) {
    this.myForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
    });
  }

  onFileSelected(event: any): void {
    const files = event.target.files;
    if (files && files.length > 0) {
      this.selectedFile = files.item(0);
    }
  }

  submitForm(): void {

    if (this.myForm.valid) {
      const formData = this.myForm.value;

      const token = this.cookieService.get("token");
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });

      let productsRecipes: { productId: number; }[] = [];
      this.chosenProductsIds.forEach(element => {
        productsRecipes.push({productId: element});
      });

      const requestBody = {
        title: formData.name,
        description: formData.description,
        createdById: this.cookieService.get("userId") || null,
        productsRecipes: productsRecipes
      };

      this.http.post(this.ROOT_URL + `/Recipe`, requestBody, { headers }).subscribe(
        {
          next: response => {
            this.postedRecipeId = response;
            if (this.selectedFile) {
              this.fileUploadService.uploadFile(this.selectedFile, this.postedRecipeId)
                .then(response => {
                  console.log(response);
                })
                .catch(error => {
                  console.error(error);
                });
            }
          },
          error: error => console.error(error)
        }
      );
    }
  }

  search() {
    const params = {
      searchString: this.searchTerm
    }

    if (this.searchTerm.length >= 3) {
      this.http.get<any>(this.ROOT_URL + `/Product/Search`, { params }).subscribe(
        {
            next: response => {
            console.log(response);
            this.results = response;
          },
          error: error => console.error('Error:', error)
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
}
