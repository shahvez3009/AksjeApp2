import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { Transaksjonshistorikk } from './transaksjonshistorikk/transaksjonshistorikk';
import { Logginn } from './logginn/logginn';
import { LagBruker } from './lagBruker/lagBruker';

const appRoots: Routes = [
    { path: 'hjem', component: Hjem },
    { path: 'portfolio', component: Portfolio },
    { path: 'transaksjonshistorikk', component: Transaksjonshistorikk },
    { path: 'logginn', component: Logginn },
    { path: 'lagBruker', component: LagBruker },
    { path: '', redirectTo: 'hjem', pathMatch: 'full' }
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