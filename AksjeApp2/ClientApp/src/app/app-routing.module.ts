import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { KjopModal } from './kjopModal/kjopModal';
import { Selg } from './selg/selg';
import { Logginn } from './logginn/logginn';

const appRoots: Routes = [
    { path: 'hjem', component: Hjem },
    { path: 'portfolio', component: Portfolio },
    { path: 'logginn', component: Logginn }, 
    { path: 'kjop', component: KjopModal },
    { path: 'selg', component: Selg },
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