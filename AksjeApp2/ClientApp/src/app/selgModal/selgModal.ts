import { Component } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PortfolioRad } from "../PortfolioRad";


@Component({
	templateUrl: "selgModal.html",
	styleUrls: ['./selgModal.css']
})

export class SelgModal {
	laster: boolean;
	navn: string;
	pris: number;
	antall: number;
	portfolioId: number;

	constructor(
		private http: HttpClient,
		private router: Router
	) { }

	ngOnInit() {
		this.laster = true;
		this.hentPortfolioRad();
	}



	hentPortfolioRad() {
		
		this.http.get<PortfolioRad>("api/aksje/hentetportfoliorad/" + Number(this.portfolioId))
			.subscribe(portfolioRad => {
				this.navn = portfolioRad.aksjeNavn;
				this.pris = portfolioRad.aksjePris;
				this.antall = portfolioRad.antall;
				this.laster = false;
				console.log("Hentet rad")
			},
				(error) => console.log(error)
			);
	}

}


