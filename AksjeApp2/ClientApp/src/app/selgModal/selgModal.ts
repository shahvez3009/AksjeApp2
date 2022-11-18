import { Component } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
	templateUrl: "selgModal.html"
})

export class SelgModal {
	constructor(public modal: NgbActiveModal) { }
}


