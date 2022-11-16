import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Hjem } from './hjem/hjem';
import { Portfolio } from './portfolio/portfolio';
import { Kjop } from './kjop/kjop';
import { Selg } from './selg/selg';

const appRoots: Routes = [
    { path: 'hjem', component: Hjem },
    { path: 'portfolio', component: Portfolio },
    { path: 'kjop', component: Kjop },
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