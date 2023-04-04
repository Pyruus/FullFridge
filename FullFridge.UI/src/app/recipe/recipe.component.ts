import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrls: ['./recipe.component.css'],
})
export class RecipeComponent implements OnInit{

  recipeId: string | null | undefined;
  constructor(
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
      this.recipeId = this.route.snapshot.paramMap.get('id');
  }

}
