import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SharedService } from "../shared/shared.service";

import { Transaksjon } from "../Transaksjon";
import { Bruker } from '../Bruker';

@Component({
	templateUrl: "transaksjonshistorikk.html"
})

export class Transaksjonshistorikk {
	laster: boolean;
	alleTransaksjoner: Array<Transaksjon>;
	brukernavn: string;
	fornavnEtternavn: string;
	saldo: number;

	constructor(
		private http: HttpClient,
		private router: Router,
		private shared: SharedService
	) { }

	//Blir kjørt når vi kaller på denne komponenten  
	ngOnInit() {
		this.laster = true;
		this.brukernavn = this.shared.getBrukernavn();
		setTimeout(() => { this.hentAllInfo(); }, 200);
	}

	hentAllInfo() {
		this.http.get<Transaksjon[]>("api/aksje/henttransaksjoner/" + this.brukernavn)
			.subscribe(transaksjonene => {
				this.alleTransaksjoner = transaksjonene;
				this.laster = false;
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
			},
				(error) => {
					if (error.status == 401) {
						this.router.navigate(["/logginn"])
					} else {
						console.log(error);
					}
				}
		);
	};

	loggUt() {
		this.http.get("api/aksje/loggut").subscribe(retur => {
			this.router.navigate(["/logginn"])
		}
		);

	}
}