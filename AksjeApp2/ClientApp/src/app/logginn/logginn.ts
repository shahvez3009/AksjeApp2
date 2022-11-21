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
export class Logginn {}
/*

export class Logginn implements OnInit {

    skjema: FormGroup;

    laster = false;
    success = false; 




    constructor(private fb: FormBuilder) {
        this.Skjema = fb.group({
            brukernavn: ["", Validators.required],
            passord: ["", Validators.pattern("[a-zA-ZæøåÆØÅ\.\-]{2,20}")]
        });
    }

    onSubmit() {
        console.log("Modellbasert skjema submitted:");
        console.log(this.Skjema);
        console.log(this.Skjema.value.brukernavn);
        console.log(this.Skjema.touched);

        this.laster = true; 

        const skjemaVerdi = this.skjema.value;

        try {
            this.loggInn;
            this.validerBrukernavn;
            this.validerPassord; 
        }
        catch(error) {
            console.log(error);
        }

    }

 
    loggInn() {
        const brukernavnOK = validerBrukernavn($("#brukernavn").val());
        const passordOK = validerPassord($("#passord").val());

        if (brukernavnOK && passordOK) {
            const bruker = {
                brukernavn: $("#brukernavn").val(),
                passord: $("#passord").val(),
            };
            $.post("Kunde/LoggInn", bruker, function (OK) {
                if (OK) {
                    window.location.href = "index.html";
                } else {
                    $("#feil").html("Feil brukernavn eller passord");
                }
            }).fail(function () {
                $("#feil").html("Feil på server - prøv igjen senere");
            });
        }
    }

  

  
    validerBrukernavn(bnavn) {
        const regexp = /^[a-zA-ZæøåÆØÅ\.\-]{2,20}$/;
        const ok = regexp.test(bnavn);
        if (!ok) {
            $("ugyldigBrukernavn").html("Skriv et ordentlig brukernavn");
            return false;
        }
        else {
            $("#ugyldigBrukernavn").html("");
            return true;
        }
    }


    validerPassord(passord) {
        const regexp = /^[a-zA-ZæøåÆØÅ\.\-]{2,20}$/;
        const ok = regexp.test(passord);
        if (!ok) {
            $("ugyldigPassord").html("Skriv et ordentlig passord");
            return false;
        }
        else {
            $("#ugyldigPassord").html("");
            return true;
        }
    }

}

*/




   
  



