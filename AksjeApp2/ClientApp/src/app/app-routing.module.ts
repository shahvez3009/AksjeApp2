import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Kjop } from './kjop/kjop';
import { Selg } from './selg/selg';
import { Portfolio } from './Portfolio';

const appRoots: Routes = [
    { path: 'portfolio', component: Portfolio },
    { path: 'selg', component: Selg },
    { path: 'kjop', component: Kjop },
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