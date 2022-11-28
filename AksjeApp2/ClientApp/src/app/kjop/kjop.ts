import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SharedService } from "../shared/shared.service";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { KjopModal } from './kjopModal';

import { Aksje } from "../Aksje";
import { PortfolioRad } from '../PortfolioRad';
import { Bruker } from '../Bruker';

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
	
		this.http.get<Aksje>("api/aksje/hentenaksje/" + this.aksjeId)
			.subscribe(hentetAksje => {
				console.log(hentetAksje);
				this.aksjenavn = hentetAksje.navn;
				this.aksjepris = hentetAksje.pris;
				this.aksjeledige = hentetAksje.antallLedige;
				this.aksjemax = hentetAksje.maxAntall;
				this.laster = false;
				console.log("kjopModal - hentEnAksje");
			},
				(error) => console.log(error)
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
				console.log(innPortfolio);

				this.http.post("api/aksje/kjop/", innPortfolio)
					.subscribe(retur => {
						console.log("Da har du kjøpt!");
					},
						error => console.log(error)
				);
				this.skjema.reset();
				setTimeout(() => { this.hentAllInfo(); }, 200);
			}
		});
	}
}