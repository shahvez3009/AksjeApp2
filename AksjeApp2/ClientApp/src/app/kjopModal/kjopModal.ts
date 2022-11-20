import { Component } from "@angular/core";
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { Aksje } from "../Aksje";
import { Router } from '@angular/router';

@Component({
	templateUrl: "kjopModal.html",
	styleUrls: ['./kjopModal.css']
})

export class KjopModal {
	laster: boolean;
	navn: string;
	pris: number;
	antall: number;
	aksjeId: number;

	constructor(
		private http: HttpClient,
		private router: Router
	) { }
	ngOnInit() {
		this.laster = true;
		this.hentEnAksje();
	}



	hentEnAksje() {

		this.http.get<Aksje>("api/aksje/hentenaksje/" + Number(this.aksjeId))
			.subscribe(hentetAksje => {
				console.log(hentetAksje);				
				this.navn = hentetAksje.navn;
				this.pris = hentetAksje.pris;
				this.antall = hentetAksje.antallLedige;
				this.laster = false;
				console.log("Hentet rad");
				console.log(this.antall);
			},
				(error) => console.log(error)
			);
	}
}

