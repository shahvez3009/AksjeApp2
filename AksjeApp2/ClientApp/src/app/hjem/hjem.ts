import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { KjopModal } from '../kjopModal/kjopModal';
import { Aksje } from '../Aksje';

@Component({
    templateUrl: "hjem.html"
})

export class Hjem {
    laster: boolean;
    alleAksjer: Array<Aksje>;

    constructor(
        private http: HttpClient,
        private router: Router,
        private modalService: NgbModal
    ){}

    ngOnInit() {
        this.laster = true;
        this.hentAlleAksjer();
    }

    visKjopModal(brukerId: number, aksjeId: number) {
        const modalRef = this.modalService.open(KjopModal);

        modalRef.componentInstance.brukerId = brukerId;
        modalRef.componentInstance.aksjeId = aksjeId;
    }

    hentAlleAksjer() {
        this.http.get<Aksje[]>("api/aksje/hentaksjer")
            .subscribe(aksjene => {
                this.alleAksjer = aksjene;
                this.laster = false;
                console.log("hjem - hentAksjer");
                console.log(aksjene);
            },
            (error) => console.log(error)
        );
    }

}
