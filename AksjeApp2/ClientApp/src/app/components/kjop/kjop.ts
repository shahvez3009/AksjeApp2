import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SharedService } from "../../shared/shared.service";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { KjopModal } from './kjopModal';
import { BeskjedModal } from '../beskjedModal/beskjedModal';

import { Aksje } from "../../Models/Aksje";
import { PortfolioRad } from '../../Models/PortfolioRad';
import { Bruker } from '../../Models/Bruker';

@Component({
	templateUrl: "kjop.html",
	styleUrls: ['./kjop.css']
})

export class Kjop {
	laster: boolean;
	status: boolean;
	aksjenavn: string;
	aksjepris: number;
	aksjeledige: number;
	aksjemax: number;

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
			null, Validators.compose([Validators.required, Validators.pattern("[1-9][0-9]*")])
		]
	}

	ngOnInit() {
		this.laster = true;
		this.brukernavn = this.shared.getBrukernavn();
		this.aksjeId = this.shared.getAksjeId();
		setTimeout(() => { this.hentAllInfo(); }, 200);
	}

	hentAllInfo() {
	
		this.http.get<Aksje>("api/aksje/hentenaksje/" + this.aksjeId)
			.subscribe(hentetAksje => {
				this.laster = false;
				this.aksjenavn = hentetAksje.navn;
				this.aksjepris = hentetAksje.pris;
				this.aksjeledige = hentetAksje.antallLedige;
				this.aksjemax = hentetAksje.maxAntall;
			},
				(error) => {
					if (error.status == 401) { this.router.navigate(["/logginn"]) }
					else if (error.status == 500) { this.shared.loggUt(); }
					else { console.log(error); }
				} 
		);

		this.http.get<Bruker>("api/aksje/hentenbruker/" + this.brukernavn)
			.subscribe(bruker => {
				this.laster = false;
				this.fornavnEtternavn = bruker.fornavn + " " + bruker.etternavn;
				this.saldo = bruker.saldo;
			},
				(error) => {
					if (error.status == 401) { this.router.navigate(["/logginn"]) }
					else if (error.status == 500) { this.shared.loggUt(); }
					else { console.log(error); }
				} 
		);
	}

	onSubmit() {
		this.visModal();
	}

	visModal() {
		if ((this.aksjepris * Number(this.skjema.value.antall)) > this.saldo) {
			const modalRef = this.modalService.open(BeskjedModal);

			modalRef.componentInstance.beskjed = "Summen for transaksjonen du prøver å utføre er: " + (this.aksjepris * Number(this.skjema.value.antall)) + " NOK";
			modalRef.componentInstance.beskjed2 = "Din saldo er: " + this.saldo + " NOK";
			modalRef.componentInstance.beskjed3 = "Du har ikke råd til denne transaksjonen!";
		}

		else {
			const modalRef = this.modalService.open(KjopModal);

			modalRef.componentInstance.aksjenavn = this.aksjenavn;
			modalRef.componentInstance.antall = Number(this.skjema.value.antall);
			modalRef.componentInstance.aksjepris = this.aksjepris;
			modalRef.componentInstance.sum = this.aksjepris * Number(this.skjema.value.antall);
			modalRef.componentInstance.aksjeledige = this.aksjeledige;
			modalRef.componentInstance.aksjemax = this.aksjemax;

			modalRef.result.then(retur => {

				if (retur == "Bekreft") {

					let innPortfolio = new PortfolioRad();
					innPortfolio.brukernavn = this.brukernavn;
					innPortfolio.aksjeId = this.aksjeId;
					innPortfolio.antall = Number(this.skjema.value.antall);

					this.http.post("api/aksje/kjop/", innPortfolio)
						.subscribe(retur => { }, error => console.log(error)
						);
					this.skjema.reset();
					setTimeout(() => { this.hentAllInfo(); }, 200);
				}
			});
		}
	}
}