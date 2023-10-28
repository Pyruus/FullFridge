import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  readonly ROOT_URL = 'https://localhost:7040/api'; // Replace with your API endpoint

  constructor(private http: HttpClient) { }

  uploadFile(file: File, recipeId: number, headers: HttpHeaders): Promise<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<any>(this.ROOT_URL + '/Recipe/File/' + recipeId, formData, { headers }).toPromise();
  }

  getFile(fileName: string): Observable<Blob> {
    let url;
    let headers;
    if(fileName.includes("themealdb")){
      url = fileName; 
    }
    else{
      url = `${this.ROOT_URL}/Recipe/File/${fileName}`;
      headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'responseType': 'blob'
      });
    }
    

    return this.http.get(url, {
      headers: headers,
      responseType: 'blob'
    });
  }
}
