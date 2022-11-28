import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Hjem } from './components/hjem/hjem';
import { Portfolio } from './components/portfolio/portfolio';
import { Transaksjonshistorikk } from './components/transaksjonshistorikk/transaksjonshistorikk';
import { Kjop } from './components/kjop/kjop';
import { Selg } from './components/selg/selg';
import { Logginn } from './components/logginn/logginn';
import { LagBruker } from './components/lagBruker/lagBruker';

const appRoots: Routes = [
    { path: 'hjem', component: Hjem },
    { path: 'portfolio', component: Portfolio },
    { path: 'transaksjonshistorikk', component: Transaksjonshistorikk },
    { path: 'kjop', component: Kjop },
    { path: 'selg', component: Selg },
    { path: 'logginn', component: Logginn },
    { path: 'lagBruker', component: LagBruker },
    { path: '', redirectTo: 'logginn', pathMatch: 'full' }
]

@NgModule({
    imports: [
        RouterModule.forRoot(appRoots)
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }