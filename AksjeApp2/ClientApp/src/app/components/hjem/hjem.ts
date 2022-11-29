import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { SharedService } from "../../shared/shared.service";

import { Aksje } from '../../Models/Aksje';
import { Bruker } from '../../Models/Bruker';


@Component({
    templateUrl: "hjem.html",
    styleUrls: ['./hjem.css']
})

export class Hjem {
    laster: boolean;
    alleAksjer: Array<Aksje>;

    brukernavn: string;
    fornavnEtternavn: string;
    saldo: number;

    constructor(
        private http: HttpClient,
        private router: Router,
        private shared: SharedService
    ){}

    ngOnInit() {
        this.laster = true;
        this.brukernavn = this.shared.getBrukernavn();

        setTimeout(() => { this.hentAllInfo(); }, 200);
    }

    hentAllInfo() {
        this.http.get<Aksje[]>("api/aksje/hentaksjer")
            .subscribe(aksjene => {
                this.laster = false;
                this.alleAksjer = aksjene;
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

    tilKjop(aksjeId) {
        this.shared.setAksjeId(aksjeId);
        this.router.navigate(["/kjop"]);
    }

}


