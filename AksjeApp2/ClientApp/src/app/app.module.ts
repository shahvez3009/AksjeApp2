import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { Meny } from './meny/meny';
import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { Kjop } from './kjop/kjop';
import { Selg } from './selg/selg';

import { AppRoutingModule } from './app-routing.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
//import { Modal } from './liste/sletteModal';

@NgModule({
    declarations: [
        AppComponent,
        Meny,
        Hjem,
        Portfolio,
        Kjop,
        Selg
    ],
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        AppRoutingModule,
        NgbModule
    ],
    providers: [],
    bootstrap: [AppComponent],
    entryComponents: [Modal]
})
export class AppModule { }
