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
	brukernavn: string;

	constructor(
		private http: HttpClient,
		private router: Router
	){}

	//Blir kjørt når vi kaller på denne komponenten  
	ngOnInit() {
		this.laster = true;
		this.brukernavn = localStorage.getItem("brukernavn");
		setTimeout(() => { this.hentAllInfo(); }, 200);
	}

	hentAllInfo() {
		this.http.get<PortfolioRad[]>("api/aksje/hentportfolio/" + this.brukernavn) 
			.subscribe(portfolioRadene => {
				this.helePortfolio = portfolioRadene;
				this.laster = false;
				console.log("portfolio - hentPortfolio")
			},
				(error) => {
					if (error.status == 401) {
						this.router.navigate(["/logginn"])
					} else {
						console.log(error);
					}

				} 
		);
	};

	loggUt() {
		this.http.get("api/aksje/loggut").subscribe(retur => {
			this.router.navigate(["/logginn"])
		}
		);

	}
}





