import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  myForm: FormGroup;

  readonly ROOT_URL = 'https://localhost:7040/api'
  
  constructor(private http: HttpClient, private formBuilder: FormBuilder) 
  {
    this.myForm = this.formBuilder.group({
      name: ['', Validators.required],
      surname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
   }

  ngOnInit(): void {
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
  
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      if (confirmPassword.errors) {
        confirmPassword.setErrors({ ...confirmPassword.errors, 'passwordMismatch': true });
      } else {
        confirmPassword.setErrors({ 'passwordMismatch': true });
      }
    } else {
      if (confirmPassword) {
        confirmPassword.setErrors(null);
      }
    }
  }

  submitForm(): void {

    if (this.myForm.valid) {
      const formData = this.myForm.value;

      const requestBody = {
        email: formData.email,
        password: formData.password,
        details: {
          name: formData.name,
          surname: formData.surname
        }
      };

      this.http.post(this.ROOT_URL + `/User/Register`, requestBody).subscribe(
        {
          next: response => console.log(requestBody),
          error: error => console.error('Error:', error)
        }
    );
    }
  }
}
