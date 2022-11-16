import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Aksje } from "../Aksje";

@Component({
    templateUrl: "hjem.html"
})

export class Hjem {
    alleAksjer: Array<Aksje>;
    laster: boolean;

    constructor(
        private http: HttpClient,
        private router: Router,
        private modalService: NgbModal
    ) { }

ngOnInit() {
    this.laster = true;
    this.hentAlleAksjer();
}

hentAlleAksjer() {
    this.http.get<Aksje[]>("api/HentAksjer/")
        .subscribe(aksjene => {
            this.alleAksjer = aksjene;
            this.laster = false;
        },
            error => console.log(error)
        );
};
}
