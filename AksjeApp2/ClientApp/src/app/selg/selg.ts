﻿import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SelgModal } from './selgModal';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { PortfolioRad } from "../PortfolioRad";
import { Aksje } from "../Aksje";

@Component({
	templateUrl: "selg.html",
	styleUrls: ["./selg.css"],
})

export class Selg {
	laster: boolean;
	aksjenavn: string;
	aksjepris: number;
	portfolioantall: number;
	aksjeId: number;
	brukerId: number;
	skjema: FormGroup;

	constructor(
		private http: HttpClient,
		private router: Router,
		private modalService: NgbModal,
		private fb: FormBuilder
	) {
		this.skjema = fb.group(this.validering);
	}

	validering = {
		id: [""],
		antall: [
			null, Validators.compose([Validators.required, Validators.pattern("[0-9]{2}")])
		]
	}

	ngOnInit() {
		this.laster = true;
		this.aksjeId = 1;
		this.brukerId = 1;
		this.hentAllInfo();
	}

	hentAllInfo() {
		this.http.get<PortfolioRad>("api/aksje/hentetportfoliorad/" + Number(this.aksjeId))
			.subscribe((portfolioRad) => {
				this.aksjenavn = portfolioRad.aksjeNavn;
				this.aksjepris = portfolioRad.aksjePris;
				this.portfolioantall = portfolioRad.antall;
				this.laster = false;
				console.log("Hentet rad");
			},
				(error) => console.log(error)
			);
	}

	onSubmit() {
		this.visModal();
	}

	visModal() {
		const modalRef = this.modalService.open(SelgModal);

		modalRef.componentInstance.aksjenavn = this.aksjenavn;
		modalRef.componentInstance.antall = Number(this.skjema.value.antall);
		modalRef.componentInstance.portfolioantall = this.portfolioantall;
		modalRef.componentInstance.aksjepris = this.aksjepris;
		modalRef.componentInstance.sum = this.aksjepris * Number(this.skjema.value.antall);

		modalRef.result.then(retur => {
			if (retur == "Bekreft") {
				const innPortfolio = new PortfolioRad();

				innPortfolio.antall = Number(this.skjema.value.antall);
				innPortfolio.aksjeId = this.aksjeId;
				console.log(innPortfolio);

				this.http.post("api/aksje/selg/", innPortfolio)
					.subscribe((retur) => {
						console.log("Da har du solgt!");
					},
						(error) => console.log(error)
				);
				this.skjema.reset();
				setTimeout(() => { this.hentAllInfo(); }, 200);
			}
		});
	}
}
