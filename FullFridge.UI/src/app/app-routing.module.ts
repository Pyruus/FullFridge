import { NgModule } from '@angular/core';
import { RouterModule, Routes, ActivatedRoute, ParamMap } from '@angular/router';
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

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'add-recipe', component: AddRecipeComponent },
  { path: 'find-recipe', component: FindRecipeComponent },
  { path: 'recipe/:id', component: RecipeComponent},
  { path: 'login', component: LoginComponent},
  { path: 'register', component: RegisterComponent},
  { path: 'forum', component: PostListComponent},
  { path: 'post/:id', component: PostDetailsComponent },
  { path: 'forum/recipe/:id', component: PostListRecipeComponent},
  { path: 'add-post', component: NewPostComponent},
  { path: 'add-post/recipe/:id',component: NewPostRecipeComponent},
  { path: '**', component: HomeComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
