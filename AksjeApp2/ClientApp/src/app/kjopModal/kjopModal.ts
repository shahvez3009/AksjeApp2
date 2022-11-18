import { Component } from "@angular/core";
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: "kjopModal.html"
})

export class KjopModal {
    constructor(public modal: NgbActiveModal) { }

    ngOnInit() {
        console.log(this.brukerId);
    }
}

