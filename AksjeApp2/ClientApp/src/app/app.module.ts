import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from "@angular/forms"; 

import { AppComponent } from './app.component';
import { Meny } from './meny/meny';
import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { Transaksjonshistorikk } from './transaksjonshistorikk/transaksjonshistorikk';
import { Kjop } from './kjop/kjop';
import { KjopModal } from './kjop/kjopModal';
import { Selg } from './selg/selg';
import { SelgModal } from './selg/selgModal';
import { Logginn } from './logginn/logginn';
import { LagBruker } from './lagBruker/lagBruker';
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { AppRoutingModule } from './app-routing.module';

@NgModule({
    declarations: [
        AppComponent,
        Meny,
        Hjem,
        Portfolio,
        Transaksjonshistorikk,
        Kjop,
        KjopModal,
        Selg,
        SelgModal,
        Logginn,
        LagBruker
    ],
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule,
        NgbModule,
        FormsModule
    ],
    providers: [],
    bootstrap: [AppComponent],
    entryComponents: [KjopModal, SelgModal]
})
export class AppModule { }
