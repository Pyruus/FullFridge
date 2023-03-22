import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AddRecipeComponent } from './add-recipe/add-recipe.component';
import { FindRecipeComponent } from './find-recipe/find-recipe.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'add-recipe', component: AddRecipeComponent },
  { path: 'find-recipe', component: FindRecipeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
