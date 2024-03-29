import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { SharedModule } from './shared/shared.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { AddRecipeComponent } from './add-recipe/add-recipe.component';
import { FindRecipeComponent } from './find-recipe/find-recipe.component';
import { RecipeComponent } from './recipe/recipe.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { PostListComponent } from './post-list/post-list.component';
import { PostDetailsComponent } from './post-details/post-details.component';
import { PostListRecipeComponent } from './post-list/post-list-recipe.component';
import { NewPostComponent } from './new-post/new-post.component';
import { NewPostRecipeComponent } from './new-post/new-post-recipe.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AddRecipeComponent,
    FindRecipeComponent,
    RecipeComponent,
    LoginComponent,
    RegisterComponent,
    PostListComponent,
    PostDetailsComponent,
    PostListRecipeComponent,
    NewPostComponent,
    NewPostRecipeComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    FontAwesomeModule,
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
