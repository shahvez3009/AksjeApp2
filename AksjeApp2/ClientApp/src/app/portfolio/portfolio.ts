import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { PortfolioRad } from "../PortfolioRad";
import { SharedService } from "../shared/shared.service";


@Component({
	templateUrl: "portfolio.html"
})

export class Portfolio {
	laster: boolean;
	helePortfolio: Array<PortfolioRad>;
	brukernavn: string;
	aksje: number;

	constructor(
		private http: HttpClient,
		private router: Router,
		private shared: SharedService
	){}

	//Blir kjørt når vi kaller på denne komponenten  
	ngOnInit() {
		this.laster = true;
		this.brukernavn = localStorage.getItem("brukernavn");
		setTimeout(() => { this.hentAllInfo(); }, 200);
	}

	hentAllInfo() {
		if (this.brukernavn.length == 0) {

			this.router.navigate(["/logginn"])
		} else {
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
		}
		
	};

	tilKjop(aksjeId) {
		this.aksje = aksjeId;
		this.shared.setAksjeId(this.aksje);
		this.router.navigate(["/kjop"]);
	}

	tilSelg(aksjeId) {
		this.aksje = aksjeId;
		this.shared.setAksjeId(this.aksje);
		this.router.navigate(["/selg"]);
	}
	loggUt() {
		this.http.get("api/aksje/loggut").subscribe(retur => {
			this.router.navigate(["/logginn"])
		}
		);

	}
}





