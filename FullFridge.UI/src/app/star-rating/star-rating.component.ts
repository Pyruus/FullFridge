import { Component } from '@angular/core';
import { faStar } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-star-rating',
  templateUrl: './star-rating.component.html',
  styleUrls: ['./star-rating.component.css']
})
export class StarRatingComponent {
  selectedStars = 0;
  faStar = faStar;

  selectStar(star: number) {
    this.selectedStars = star;
  }
}
