import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
	templateUrl: 'kjopModal.html'
})

export class KjopModal {
	constructor(
		public kjopmodal: NgbActiveModal
	) { }
}