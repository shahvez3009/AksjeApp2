import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class SharedService {
    aksjeId: number = 0;
    brukernavn: string= "$";
   
    constructor(
        private http: HttpClient,
        private router: Router
    ) {
    }

    setAksjeId(data) {
        this.aksjeId = data;
    }

    getAksjeId() {
        return this.aksjeId;
    }

    setBrukernavn(data) {
        this.brukernavn = data;
    }

    getBrukernavn() {
        return this.brukernavn;
    }

    loggUt() {
        this.http.get("api/aksje/loggut").subscribe(retur => {
            this.router.navigate(["/logginn"])
        });
    }
}