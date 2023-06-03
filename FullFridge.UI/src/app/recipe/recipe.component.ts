import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { FileUploadService } from '../file-upload.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { CookieService } from 'ngx-cookie-service';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { Router } from '@angular/router';


@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css'],
})
export class RecipeComponent implements OnInit {

  recipeId: any;
  readonly ROOT_URL = 'https://localhost:7040/api'
  recipe: any;
  fileUrl: string | null = null;
  sanitizedFileUrl: SafeUrl | null = null;
  faTrash = faTrash;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private fileService: FileUploadService,
    private sanitizer: DomSanitizer,
    private cookieService: CookieService,
    private router: Router
  ) { }


  ngOnInit(): void {
    this.recipeId = this.route.snapshot.paramMap.get('id');
    this.http.get(this.ROOT_URL + `/Recipe/${this.recipeId}`).subscribe(
      {
        next: response => {
          this.recipe = response;
          this.getFile(this.recipe.image);
        },
        error: error => console.error('Error:', error)
      }
    );
  }

  getFile(fileName: string): void {
    this.fileService.getFile(fileName).subscribe(
      (file: Blob) => {
        this.fileUrl = URL.createObjectURL(file);
        this.sanitizedFileUrl = this.sanitizer.bypassSecurityTrustUrl(this.fileUrl);
      },
      (error) => {
        console.error(error);
      }
    );
  }

  isUserAdmin() {
    return this.cookieService.get("userRole") == "admin";
  }

  deleteRecipe() {
    const token = this.cookieService.get("token");
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    this.http.delete(this.ROOT_URL + `/Recipe/${this.recipeId}`, { headers }).subscribe(
      {
        next: response => {
          this.router.navigate([``]);
        },
        error: error => console.error('Error:', error)
      }
    );
  }
}
