import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Bruker } from "../Bruker";
import { FormBuilder, FormGroup, Validators, FormControl } from "@angular/forms";

@Component({
    selector: "app-logginn",
    templateUrl: "./logginn.html",
    styleUrls: ['./logginn.css']
})

export class Logginn {
    Skjema: FormGroup;
    valid;
    invalidBruker: boolean; 

    constructor(
        private fb: FormBuilder,
        private http: HttpClient,
        private router: Router) {
        this.Skjema = fb.group(this.validering)
     
    }

    validering = {
        brukernavn: [
            null,
            Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZæøåÆØÅ\ .-/<>!?#&()=]{2,40}")])
        ],
        passord: [
            null,
            Validators.compose([Validators.required, Validators.pattern("[0-9a-zA-ZæøåÆØÅ\ .-/<>!?#&()=]{5,16}")])
        ]
    };


    ngOnInit() {
        console.log(this.Skjema.valid);
        localStorage.setItem("brukernavn", "");
    }

    onSubmit() {

        console.log("Modelbasert skjema submitted");
        console.log(this.Skjema);
        console.log(this.Skjema.value.brukernavn);
        console.log(this.Skjema.value.passord);
        console.log(this.Skjema.touched);
        this.logginn();
    }



    logginn() {

        const send = new Bruker();

        send.brukernavn = this.Skjema.value.brukernavn;
        send.passord = this.Skjema.value.passord;
        console.log(send);

        this.http.post("api/aksje/UserIn", send)
            .subscribe(retur => {
                this.valid = retur;
                if (this.valid) {
                    localStorage.setItem("brukernavn", send.brukernavn);
                    console.log("Du er logget inn");
                    this.router.navigate(["/hjem"]);
                }
                error => { this.invalidBruker = true }   
            },
            );
    }
    
}


    


