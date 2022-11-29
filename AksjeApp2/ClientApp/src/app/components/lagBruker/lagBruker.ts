import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

import { Bruker } from "../../Models/Bruker";

@Component({
    templateUrl: './lagbruker.html',
    styleUrls: ['./lagbruker.css']
})
export class LagBruker {
    skjema: FormGroup;
    tilbakemelding: string;
    passord: string;
    bekreftpassord: string;


    //Husk å endre, sånn at det blir riktig validering
    validering = {
        id: [""],
        fornavn: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        etternavn: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        mail: [
            null, Validators.compose([Validators.required, Validators.pattern(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/)])
        ],
        telefonnummer: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9]{8}")])
        ],
        brukernavn: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        //Må matche med backenden
        passord: [
            null, Validators.compose([Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.{8,})/)])
        ],
        bekreftPassord: [
            null, Validators.compose([Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.{8,})/)])
        ]
    }

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
        this.skjema = fb.group(this.validering);
    }

    onSubmit(){
        if (this.skjema.value.passord == this.skjema.value.bekreftPassord) {
            this.lagreBruker();
        }
        else {
            this.tilbakemelding = "Passordene matcher ikke!!";
        }
    }

    lagreBruker() {
        const nyBruker = new Bruker();

        nyBruker.fornavn = this.skjema.value.fornavn;
        nyBruker.etternavn = this.skjema.value.etternavn;
        nyBruker.mail = this.skjema.value.mail;
        nyBruker.telefonnummer = this.skjema.value.telefonnummer;
        nyBruker.brukernavn = this.skjema.value.brukernavn;
        nyBruker.passord = this.skjema.value.passord;

        this.http.post("api/aksje/lagrebruker/", nyBruker)
            .subscribe(retur => {
                this.router.navigate(['/logginn']);
            },
                error => {
                    console.log(error);

                    if (error.error == "Brukernavnet er opptatt" || error.error == "Mailen er opptatt") {
                        this.tilbakemelding = error.error;
                    }
                    else {
                        this.tilbakemelding = "Feil i inputvalidering";
                    }
                }
            );
    }
}
