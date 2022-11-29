import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
	templateUrl: 'beskjedModal.html'
})

export class BeskjedModal {
	constructor(
		public beskjedmodal: NgbActiveModal
	) { }
}
