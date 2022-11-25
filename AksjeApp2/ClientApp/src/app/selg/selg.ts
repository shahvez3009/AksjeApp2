﻿import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { PortfolioRad } from "../PortfolioRad";
import { Aksje } from "../Aksje";

@Component({
	templateUrl: "selg.html",
	styleUrls: ["./selg.css"],
})

export class Selg {
	laster: boolean;
	navn: string;
	pris: number;
	antall: number;
	aksjeId: number;
	brukerId: number;
	selgAntall: number;
	skjema: FormGroup;

	constructor(
		private http: HttpClient,
		private router: Router,
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
				this.navn = portfolioRad.aksjeNavn;
				this.pris = portfolioRad.aksjePris;
				this.antall = portfolioRad.antall;
				this.laster = false;
				console.log("Hentet rad");
			},
				(error) => console.log(error)
			);
	}

	onSubmit() {
		this.bekreftSalg();
		this.hentAllInfo();
		this.router.navigate(['/portfolio']);
	}

	bekreftSalg() {
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
	}
}