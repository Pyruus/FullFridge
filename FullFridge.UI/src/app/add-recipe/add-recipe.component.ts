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

      const requestBody = {
        title: formData.name,
        description: formData.description,
        createdById: this.cookieService.get("userId") || null
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

}
