import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Portfolio } from "../Portfolio"; 

@Component({
    //Selector: Må legge inn hvilken component den skal erstatte
    templateUrl: "portfolio.html"
})

/*
export class Portfolio {
    laster: boolean;
    helePortfolio: Array<Portfolio>;

    constructor(private _http: HttpClient) { }

    //Blir kjørt når vi kaller på denne komponenten
    ngOnInit() {
        this.laster = true;
        this.hentPortfolio();
    }

    //S
    hentPortfolio() {
        this.http.get<Portfolio[]>("api/portfolio/") //Skjønner ikke hva "api/portfolio betyr"
            .subscribe(portfolioene => {
                this.helePortfolio = portfolioene;
                this.laster = false;
            },
                error => console.log(error)
            );
    };
}
*/




