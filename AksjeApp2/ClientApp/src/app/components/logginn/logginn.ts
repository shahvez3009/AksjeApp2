import { Component, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { SharedService } from "../../shared/shared.service";
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
} from "@angular/forms";
import { Bruker } from "../../Models/Bruker";

@Component({
	selector: "app-logginn",
	templateUrl: "./logginn.html",
	styleUrls: ["./logginn.css"],
})
export class Logginn {
  Skjema: FormGroup;
  valid;
  invalidBruker: boolean;

  constructor(
	private fb: FormBuilder,
	private http: HttpClient,
	private router: Router,
	private shared: SharedService
  ) {
	this.Skjema = fb.group(this.validering);
  }

  validering = {
	brukernavn: [
	  null,
	  Validators.compose([
		Validators.required,
		  Validators.pattern("[0-9a-zA-ZøæåØÆÅ\\-. ]{2,30}"),
	  ]),
	],
	passord: [
	  null,
	  Validators.compose([
		Validators.required,
		  Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.{8,})/),
	  ]),
	],
  };

  onSubmit() {
	this.logginn();
  }

  logginn() {
	const send = new Bruker();

	send.brukernavn = this.Skjema.value.brukernavn;
	send.passord = this.Skjema.value.passord;

	  this.http.post("api/aksje/LoggInn", send).subscribe((retur) => {
		  this.valid = retur;
		if (this.valid) {
		  this.shared.setBrukernavn(send.brukernavn.toLowerCase());
			this.router.navigate(["/hjem"]);
		}
		  if (!this.valid) {
			  this.invalidBruker = true;
		  }
	  (error) => {
		this.invalidBruker = true;
		console.log("Ikke logget inn");
	  };
	});
  }
}
