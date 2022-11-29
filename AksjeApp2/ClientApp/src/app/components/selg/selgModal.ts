import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
	templateUrl: 'selgModal.html',
	styleUrls: ['./selgModal.css']
})

export class SelgModal {
	constructor(
		public selgmodal: NgbActiveModal
	) { }
}