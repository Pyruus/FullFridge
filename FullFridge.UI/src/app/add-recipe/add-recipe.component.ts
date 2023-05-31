import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-add-recipe',
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.css']
})
export class AddRecipeComponent {
  myForm: FormGroup;

  readonly ROOT_URL = 'https://localhost:7040/api'

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private cookieService: CookieService) 
  {
    this.myForm = this.formBuilder.group({
      name: ['', [Validators.required]],
      description: ['', Validators.required],
    });
  }

  submitForm(): void {

    if (this.myForm.valid) {
      const formData = this.myForm.value;

      const token = this.cookieService.get("token");
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });

      const requestBody = {
        title: formData.name,
        description: formData.description,
        createdById: this.cookieService.get("userId") || null
      };

      this.http.post(this.ROOT_URL + `/Recipe`, requestBody, { headers }).subscribe(
        (response: any) => {
          console.log(requestBody); // Print the fetched data
        },
        (error: any) => {
          console.error(error);
        }
    );
    }
  }

}
