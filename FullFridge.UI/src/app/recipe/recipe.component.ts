import { Component, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { FileUploadService } from '../file-upload.service';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';


@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css'],
})
export class RecipeComponent implements OnInit{

  recipeId: any;
  readonly ROOT_URL = 'https://localhost:7040/api'
  recipe: any;
  fileUrl: string | null = null;
  sanitizedFileUrl: SafeUrl | null = null;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private fileService: FileUploadService,
    private sanitizer: DomSanitizer
  ) {}
  

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
}
