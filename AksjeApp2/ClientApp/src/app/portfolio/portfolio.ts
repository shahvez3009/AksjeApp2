import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { PortfolioRad } from "../PortfolioRad"; 

@Component({
	templateUrl: "portfolio.html"
})

export class Portfolio {
	laster: boolean;
	helePortfolio: Array<PortfolioRad>;

	constructor(
		private http: HttpClient,
		private router: Router
	){}

	//Blir kjørt når vi kaller på denne komponenten
	ngOnInit() {
		this.laster = true;
		this.hentPortfolio();
	}

	hentPortfolio() {
		this.http.get<PortfolioRad[]>("api/aksje/hentportfolio") 
			.subscribe(portfolioRadene => {
				this.helePortfolio = portfolioRadene;
				this.laster = false;
				console.log("Hentet portfolio")
			},
			(error) => console.log(error)
		);
	};
}





