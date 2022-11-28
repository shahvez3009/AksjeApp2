﻿import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { SharedService } from "../shared/shared.service";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SelgModal } from './selgModal';

import { PortfolioRad } from "../PortfolioRad";
import { Bruker } from '../Bruker';

@Component({
	templateUrl: "selg.html",
	styleUrls: ["./selg.css"],
})

export class Selg {
	laster: boolean;
	aksjenavn: string;
	aksjepris: number;
	portfolioantall: number;

	brukernavn: string;
	fornavnEtternavn: string;
	saldo: number;
	aksjeId: number;

	skjema: FormGroup;

	constructor(
		private http: HttpClient,
		private router: Router,
		private modalService: NgbModal,
		private fb: FormBuilder,
		private shared: SharedService
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
		this.brukernavn = this.shared.getBrukernavn();
		this.aksjeId = this.shared.getAksjeId();
		setTimeout(() => { this.hentAllInfo(); }, 200);
	}

	hentAllInfo() {
		this.http.get<PortfolioRad>("api/aksje/hentetportfoliorad/" + this.brukernavn + "/" + this.aksjeId)
			.subscribe(retur => {
				this.aksjenavn = retur.aksjeNavn;
				this.aksjepris = retur.aksjePris;
				this.portfolioantall = retur.antall;
				this.laster = false;
				console.log("Hentet rad");
			},
				(error) => {
					if (error.status == 401) {
						this.router.navigate(["/logginn"])
					} else {
						console.log(error);
					}
				} 
		);

		this.http.get<Bruker>("api/aksje/hentenbruker/" + this.brukernavn)
			.subscribe(bruker => {
				this.laster = false;
				this.fornavnEtternavn = bruker.fornavn + " " + bruker.etternavn;
				this.saldo = bruker.saldo;
				console.log(bruker);
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

				let innPortfolio = new PortfolioRad();
				innPortfolio.brukernavn = this.brukernavn;
				innPortfolio.aksjeId = this.aksjeId;
				innPortfolio.antall = Number(this.skjema.value.antall);
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
