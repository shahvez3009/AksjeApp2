﻿import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Transaksjon } from "../Transaksjon";

@Component({
	templateUrl: "transaksjon.html"
})

export class Transaksjon {
	laster: boolean;
	alleTransaksjoner: Array<Transaksjon>;

	constructor(
		private http: HttpClient,
		private router: Router
	) { }

	//Blir kjørt når vi kaller på denne komponenten  
	ngOnInit() {
		this.laster = true;
		this.hentTransaksjoner();
	}

	hentTransaksjoner() {
		this.http.get<Transaksjon[]>("api/aksje/henttransaksjoner")
			.subscribe(transaksjonene => {
				this.alleTransaksjoner = transaksjonene;
				this.laster = false;
				console.log("transaksjon - hentTransaksjoner")
			},
				(error) => console.log(error)
			);
	};
}