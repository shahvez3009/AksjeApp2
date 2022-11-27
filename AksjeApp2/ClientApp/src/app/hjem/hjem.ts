import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Aksje } from '../Aksje';
import { Local } from "protractor/built/driverProviders";

@Component({
    templateUrl: "hjem.html"
})

export class Hjem {
    laster: boolean;
    alleAksjer: Array<Aksje>;

    constructor(
        private http: HttpClient,
        private router: Router
    ){}

    ngOnInit() {
        this.laster = true;
        setTimeout(() => { this.hentAllInfo(); }, 200);
    }

    hentAllInfo() {
        this.http.get<Aksje[]>("api/aksje/hentaksjer")
            .subscribe(aksjene => {
                console.log(localStorage.getItem("brukernavn"));
                this.alleAksjer = aksjene;
                this.laster = false;
                console.log("hjem - hentAksjer");
                console.log(aksjene);
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

}
