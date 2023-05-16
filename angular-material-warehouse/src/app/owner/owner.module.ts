import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OwnerListComponent } from './owner-list/owner-list.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './../material/material.module';
import { OwnerRoutingModule } from './owner-routing/owner-routing.module';
import { OwnerUpdateComponent } from './owner-update/owner-update.component';


@NgModule({
  declarations: [
    OwnerListComponent,
    OwnerUpdateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    OwnerRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FlexLayoutModule
  ]
})
export class OwnerModule { }
