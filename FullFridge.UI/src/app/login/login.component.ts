import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  myForm: FormGroup;

  readonly ROOT_URL = 'https://localhost:7040/api'

  constructor(private http: HttpClient, private formBuilder: FormBuilder, private cookieService: CookieService, private router: Router) 
  {
    this.myForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
   }


  ngOnInit(): void {
  }

  submitForm(): void {

    if (this.myForm.valid) {
      const formData = this.myForm.value;

      const params = {
        email: formData.email,
        password: formData.password
      };

      this.http.get<any>(this.ROOT_URL + `/User/Login`, { params }).subscribe(
        {
          next: response => {
            console.log(response);
            this.cookieService.set("token", response.token);
            this.cookieService.set("userId", response.id);
            this.cookieService.set("userName", response.name);
            this.router.navigate([``]);
          },
          error: error => console.error('Error:', error)
        }
    );
    }
  }

}
