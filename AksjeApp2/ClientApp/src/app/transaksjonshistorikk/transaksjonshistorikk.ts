import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Transaksjon } from "../Transaksjon";

@Component({
	templateUrl: "transaksjonshistorikk.html"
})

export class Transaksjonshistorikk {
	laster: boolean;
	alleTransaksjoner: Array<Transaksjon>;
	brukernavn: string;

	constructor(
		private http: HttpClient,
		private router: Router
	) { }

	//Blir kjørt når vi kaller på denne komponenten  
	ngOnInit() {
		this.laster = true;
		this.brukernavn = localStorage.getItem("brukernavn");
		setTimeout(() => { this.hentAllInfo(); }, 200);
	}

	hentAllInfo() {
		if (this.brukernavn.length == 0) {
		
			this.router.navigate(["/logginn"])
		}
		else {this.http.get<Transaksjon[]>("api/aksje/henttransaksjoner/" + this.brukernavn)
			.subscribe(transaksjonene => {
				this.alleTransaksjoner = transaksjonene;
				this.laster = false;
				console.log("transaksjon - hentTransaksjoner")
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
		
	};


	loggUt() {
		this.http.get("api/aksje/loggut").subscribe(retur => {
			this.router.navigate(["/logginn"])
		}
		);

	}
}