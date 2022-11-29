import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SharedService } from "../../shared/shared.service";

import { Transaksjon } from "../../Models/Transaksjon";
import { Bruker } from '../../Models/Bruker';

@Component({
	templateUrl: "transaksjonshistorikk.html",
	styleUrls: ['./transaksjonshistorikk.css']
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

	ngOnInit() {
		this.laster = true;
		this.brukernavn = this.shared.getBrukernavn();
		setTimeout(() => { this.hentAllInfo("Alle"); }, 200);
	}

	hentAllInfo(status) {
		this.http.get<Transaksjon[]>("api/aksje/henttransaksjoner/" + this.brukernavn + "/" + status)
			.subscribe(transaksjonene => {
				this.alleTransaksjoner = transaksjonene;
				this.laster = false;
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
	};

	loggUt() {
		this.http.get("api/aksje/loggut").subscribe(retur => {
			this.router.navigate(["/logginn"])
		}
		);

	}
}