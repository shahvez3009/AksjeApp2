import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { Transaksjon } from './transaksjon/transaksjon';
import { Logginn } from './logginn/logginn';
import { LagBruker } from './lagBruker/lagBruker';

const appRoots: Routes = [
    { path: 'hjem', component: Hjem },
    { path: 'portfolio', component: Portfolio },
    { path: 'transaksjon', component: Transaksjon },
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