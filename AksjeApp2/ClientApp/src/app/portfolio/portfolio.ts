import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { PortfolioRad } from "../PortfolioRad"; 

@Component({
	templateUrl: "portfolio.html"
})


export class Portfolio {
	laster: boolean;
	hentPortfolio: Array<PortfolioRad>;

	constructor(private http: HttpClient) { }

	//Blir kjørt når vi kaller på denne komponenten
	ngOnInit() {
		this.laster = true;
		this.hentPortfolio();
	}

	//S
	hentPortfolio() {
		this.http.get<PortfolioRad[]>("api/portfolio/hentportfolio") 
			.subscribe(portfolioene => {
				console.log("Inne i portfolio")
				this.hentPortfolio = portfolioene;
				this.laster = false;
			},
				error => console.log(error)
			);
	};
}





