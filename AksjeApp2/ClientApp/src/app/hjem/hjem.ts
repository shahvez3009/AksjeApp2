import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Aksje } from "../Aksje";

@Component({
    templateUrl: "hjem.html"
})


ngOnInit() {
    this.laster = true;
    this.hentAlleAksjer();
}

hentAlleAksjer() {
    this.http.get<Aksje[]>("api/aksje/")
        .subscribe(aksjene => {
            this.alleAksjer = aksjene;
            this.laster = false;
        },
            error => console.log(error)
        );
};
