import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  readonly ROOT_URL = 'https://localhost:7040/api'; // Replace with your API endpoint

  constructor(private http: HttpClient) { }

  uploadFile(file: File, recipeId: number): Promise<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<any>(this.ROOT_URL + '/Recipe/File/' + recipeId, formData).toPromise();
  }
}
