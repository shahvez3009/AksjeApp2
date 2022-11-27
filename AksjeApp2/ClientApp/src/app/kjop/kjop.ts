import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { KjopModal } from './kjopModal';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Aksje } from "../Aksje";
import { Bruker } from "../Bruker";
import { PortfolioRad } from '../PortfolioRad';

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
	aksjeId: number;
	brukernavn: string;
	skjema: FormGroup;

	constructor(
		private http: HttpClient,
		private router: Router,
		private modalService: NgbModal,
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
		this.brukernavn = localStorage.getItem("brukernavn");
		setTimeout(() => { this.hentAllInfo(); }, 200);
	}

	hentAllInfo() {
		/*
		this.http.get<PortfolioRad>("api/aksje/hentetportfoliorad/" + (this.brukernavn, this.aksjeId))
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
				(error) => {
					if (error.status == 401) {
						this.router.navigate(["/logginn"])
					} else {
						console.log(error);
					}

				} 
		);
		*/

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
				const nyttKjop = new PortfolioRad();

				nyttKjop.antall = Number(this.skjema.value.antall);
				nyttKjop.aksjeId = this.aksjeId;
				nyttKjop.brukernavn = this.brukernavn;
				console.log(nyttKjop);

				this.http.post("api/aksje/kjop/", nyttKjop)
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