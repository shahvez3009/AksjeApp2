import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
    aksjeId: number = 0;
    brukernavn: string= "$";
   
    constructor() { }

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
}