﻿import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { KjopModal } from './kjopModal/kjopModal';
import { SelgModal } from './selgModal/selgModal';
import { Logginn } from './logginn/logginn';

const appRoots: Routes = [
    { path: 'hjem', component: Hjem },
    { path: 'portfolio', component: Portfolio },
    { path: 'logginn', component: Logginn }, 
    { path: 'kjop', component: KjopModal },
    { path: 'selg', component: SelgModal },
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