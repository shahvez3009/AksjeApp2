import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from "../../shared/shared.service";

@Component({
	selector: 'app-meny',
	templateUrl: './meny.html',
	styleUrls: ['./meny.css']
})

export class Meny {
	isExpanded = false;
	brukernavn: string;

	constructor(
		private router: Router,
		private shared: SharedService
	) { }

	collapse() {
		this.isExpanded = false;
	}

	toggle() {
		this.isExpanded = !this.isExpanded;
	}

	loggUt() {
		this.shared.loggUt();
	}
}

