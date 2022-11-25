import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Bruker } from "../Bruker";

@Component({
    templateUrl: './lagbruker.html'
})
export class LagBruker {
    skjema: FormGroup;

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
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        telefonnummer: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9]{8}")])
        ],
        brukernavn: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ],
        passord: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}")])
        ]
    }

    constructor(private http: HttpClient, private fb: FormBuilder, private router: Router) {
        this.skjema = fb.group(this.validering);
    }

    Submit() {
        this.lagreBruker();
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
                error => console.log(error)
            );
    }
}