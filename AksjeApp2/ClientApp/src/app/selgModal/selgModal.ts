import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { PortfolioRad } from "../PortfolioRad";
import { Aksje } from "../Aksje";

@Component({
  templateUrl: "selgModal.html",
  styleUrls: ["./selgModal.css"],
})

export class SelgModal {
  laster: boolean;
  navn: string;
  pris: number;
  antall: number;
  aksjeId: number;
  portfolioId: number;
  selgAntall: number;
  solgtId: number;

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.laster = true;
    this.hentAllInfo();
  }

  hentAllInfo() {
    this.http
        .get<PortfolioRad>(
            "api/aksje/hentetportfoliorad/" + Number(this.aksjeId)
      )
      .subscribe(
        (portfolioRad) => {
          this.navn = portfolioRad.aksjeNavn;
          this.pris = portfolioRad.aksjePris;
          this.antall = portfolioRad.antall;
          this.laster = false;
          console.log("Hentet rad");
        },
        (error) => console.log(error)
      );
  }

  selgAksje() {
    console.log(Number(this.selgAntall));
    console.log(Number(this.aksjeId));
    const innPortfolio = new PortfolioRad();
    //innPortfolio.aksjeid = this.aksjeId;
    innPortfolio.antall = this.selgAntall;
    //innPortfolio.aksjeNavn = "Hanji";
    //innPortfolio.brukerid = 1;
    //innPortfolio.aksjePris = 200;
    innPortfolio.id = this.aksjeId;
    console.log(innPortfolio);
    this.http.post("api/aksje/selg/", innPortfolio).subscribe((retur) => {
      this.router.navigate(["/portfolio"]);
    });
  }
}
