import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
    aksjeId: number; 
    constructor() { }

    setAksjeId(data) {
        this.aksjeId = data;

    }

    getAksjeId() {
        return this.aksjeId;
    }
}
