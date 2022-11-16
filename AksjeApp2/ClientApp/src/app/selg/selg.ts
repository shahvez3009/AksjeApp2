import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { PortfolioRad } from "../PortfolioRad";
import { Bruker } from "../Bruker";

@Component({
    templateUrl: "selg.html"
})

export class Selg {

}

/*
ngOnInit() {
    this.laster = true;
    this.hentAlleAksjer();
}

bekreftSalg(){
    h
}

HentenPortfolioRad() {
    this._http.get<Portfolio[]>("api/portfolio/" + id)
        .subscribe(portfolioene => {
            this.helePortfolioRad = portfolioene;
            this.laster = false;
        },
            error => console.log(error)
        );
};


hentAllinfo(){

}
*/
