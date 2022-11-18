﻿import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { KjopModal } from '../kjopModal/kjopModal';
import { SelgModal } from '../selgModal/selgModal';
import { PortfolioRad } from "../PortfolioRad"; 

@Component({
	templateUrl: "portfolio.html"
})

export class Portfolio {
	laster: boolean;
	helePortfolio: Array<PortfolioRad>;
	//modalVerdier: Array<>;

	constructor(
		private http: HttpClient,
		private router: Router,
		private modalService: NgbModule
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

	/*
	visKjopModal(brukerId: number, portfolioId: number) {
		const modalRef = this.modalService.open(KjopModal);

		modalVerdier = [brukerId, portfolioId];
		modalRef.componentInstance.brukerId = this.modalVerdier[0];
		modalRef.componentInstance.portfolioId = this.modalVerdier[1];
	}
	*/

	visKjopModal() {
		const modalRef = this.modalService.open(KjopModal);
	}
}





