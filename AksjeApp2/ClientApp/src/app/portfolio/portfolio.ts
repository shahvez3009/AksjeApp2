import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { KjopModal } from '../kjopModal/kjopModal';
import { SelgModal } from '../selgModal/selgModal';
import { PortfolioRad } from "../PortfolioRad"; 

@Component({
	templateUrl: "portfolio.html"
})

export class Portfolio {
	laster: boolean;
	helePortfolio: Array<PortfolioRad>;

	constructor(
		private http: HttpClient,
		private router: Router,
		private modalService: NgbModal
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

	
	visKjopModal(brukerId: number, aksjeId: number) {
		const modalRef = this.modalService.open(KjopModal);

		modalRef.componentInstance.brukerId = brukerId;
		modalRef.componentInstance.aksjeId = aksjeId;
	}
	
	visSelgModal(brukerId: number,aksjeId: number) {
		const modalRef = this.modalService.open(SelgModal);

		modalRef.componentInstance.brukerId = brukerId;
		//modalRef.componentInstance.portfolioId = portfolioId;
		modalRef.componentInstance.aksjeId = aksjeId;
	}
}





