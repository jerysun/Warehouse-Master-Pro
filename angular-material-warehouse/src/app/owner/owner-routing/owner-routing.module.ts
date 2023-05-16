import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OwnerListComponent } from '../owner-list/owner-list.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: 'owners', component: OwnerListComponent}
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class OwnerRoutingModule { }
