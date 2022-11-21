﻿import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from "@angular/forms"; 

import { AppComponent } from './app.component';
import { Meny } from './meny/meny';
import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { Logginn } from './logginn/logginn';
import { LagBruker } from './lagBruker/lagBruker';
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { KjopModal } from './kjopModal/kjopModal';
import { SelgModal } from './selgModal/selgModal';

import { AppRoutingModule } from './app-routing.module';

@NgModule({
    declarations: [
        AppComponent,
        Logginn,
        LagBruker,
        Meny,
        Hjem,
        Portfolio,
        KjopModal,
        SelgModal
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
