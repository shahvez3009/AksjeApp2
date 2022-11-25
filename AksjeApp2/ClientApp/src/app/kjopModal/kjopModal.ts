import { Component } from "@angular/core";
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Aksje } from "../Aksje";
import { Bruker } from "../Bruker";
import { PortfolioRad } from '../PortfolioRad';

@Component({
	templateUrl: "kjopModal.html",
	styleUrls: ['./kjopModal.css']
})

export class KjopModal {
	laster: boolean;
	status: boolean;
	navn: string;
	pris: number;
	ledige: number;
	brukerId: number;
	aksjeId: number;
	skjema: FormGroup;

	validering = {
		id: [""],
		antall: [
			null, Validators.compose([Validators.required, Validators.pattern("[0-9]{3}")])
		]
	}

	constructor(
		private http: HttpClient,
		private router: Router,
		private fb: FormBuilder
	){
		this.skjema = fb.group(this.validering);
	}

	ngOnInit() {
		this.laster = true;
		this.hentAllInfo();
	}

	onSubmit() {
		this.bekreftKjop();
	}

	bekreftKjop() {
		const nyttKjop = new PortfolioRad();

		nyttKjop.antall = Number(this.skjema.value.antall);
		nyttKjop.aksjeId = this.aksjeId;
		nyttKjop.aksjeNavn = this.navn;
		nyttKjop.aksjePris = this.pris;
		nyttKjop.brukerId = this.brukerId;

		console.log(nyttKjop);
		this.http.post("api/aksje/kjop/", nyttKjop)
			.subscribe(retur => {
				console.log("etter subscribe");
			},
			error => console.log(error)
		);
	}

	hentAllInfo() {
		this.http.get<PortfolioRad>("api/aksje/hentetportfoliorad/" + Number(this.aksjeId))
			.subscribe(hentetRad => {
				if (hentetRad.antall == 0) {
					this.status = false;
					console.log(this.status);
				}
				else {
					this.status = true;
					console.log(this.status);
				}
				console.log("kjopModal - hentEtPortfolioRad");
			},
				(error) => console.log(error)
			);

		this.http.get<Aksje>("api/aksje/hentenaksje/" + Number(this.aksjeId))
			.subscribe(hentetAksje => {
				console.log(hentetAksje);
				this.navn = hentetAksje.navn;
				this.pris = hentetAksje.pris;
				this.ledige = hentetAksje.antallLedige;
				this.laster = false;
				console.log("kjopModal - hentEnAksje");
			},
				(error) => console.log(error)
			);
	}
}