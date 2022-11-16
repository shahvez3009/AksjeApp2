import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { Hjem } from './hjem/hjem'; //!!
import { Kjop } from './kjop/kjop'; //!!
import { Selg } from './selg/selg'; //!!

import { Portfolioo } from './portfolio/portfolio';

import { AppRoutingModule } from './app-routing.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Modal } from './liste/sletteModal';

@NgModule({
    declarations: [
        AppComponent,
        Hjem,
        Kjop,
        Selg,
        Portfolio
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
    entryComponents: [Modal] // merk!  
})
export class AppModule { }
