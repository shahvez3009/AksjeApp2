import { Component } from '@angular/core';

@Component({
	selector: 'app-meny',
	templateUrl: './meny.html',
	styleUrls: ['./meny.css']
})

export class Meny {
	isExpanded = false;

	collapse() {
		this.isExpanded = false;
	}

	toggle() {
		this.isExpanded = !this.isExpanded;
	}
}

