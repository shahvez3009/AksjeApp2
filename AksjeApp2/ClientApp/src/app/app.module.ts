import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from "@angular/forms"; 

import { AppComponent } from './app.component';
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { Meny } from './components/meny/meny';
import { Hjem } from './components/hjem/hjem';
import { Portfolio } from './components/portfolio/portfolio';
import { Transaksjonshistorikk } from './components/transaksjonshistorikk/transaksjonshistorikk';
import { Kjop } from './components/kjop/kjop';
import { KjopModal } from './components/kjop/kjopModal';
import { Selg } from './components/selg/selg';
import { SelgModal } from './components/selg/selgModal';
import { BeskjedModal } from './components/beskjedModal/beskjedModal';
import { Logginn } from './components/logginn/logginn';
import { LagBruker } from './components/lagBruker/lagBruker';

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
        BeskjedModal,
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
    entryComponents: [KjopModal, SelgModal, BeskjedModal]
})
export class AppModule { }
