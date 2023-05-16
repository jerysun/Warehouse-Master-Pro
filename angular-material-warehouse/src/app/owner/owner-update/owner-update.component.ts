import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as alertify from 'alertifyjs'
import { RepositoryService } from '../../shared/repository.service';

@Component({
  selector: 'app-owner-update',
  templateUrl: './owner-update.component.html',
  styleUrls: ['./owner-update.component.css']
})
export class OwnerUpdateComponent implements OnInit {
  editdata: any;
  toIncrease = true;

  updateForm = new FormGroup({
    productId: new FormControl({value: "", disabled: true}),
    amount: new FormControl(null, [Validators.required])
  })

  constructor(private service: RepositoryService, @Inject(MAT_DIALOG_DATA) public data: any,
    private ref: MatDialogRef<OwnerUpdateComponent>) {
      this.toIncrease = data.toIncrease;
    }

  ngOnInit(): void {
    this.getExistdata(this.data.productId);
  }

  saveProductStock() {
    if (this.updateForm.valid) {
      const update = this.toIncrease ? 'add-stocks' : 'remove-stocks';
      const id = this.editdata.id;
      this.service.updateStock(`Products/${id}/${update}`, this.updateForm.getRawValue())
        .subscribe({
          next: res => {
          const item: any = res;
          alertify.success("Updated successfully.");
        },
        error: err => {
          alertify.error(err.error.message);
        }
      });
    }
  }

  getExistdata(id: number) {
    console.log('getExistdata-id: ', id);
    this.service.getData(`products/${id}`)
      .subscribe(res => {
        console.log(JSON.stringify(res));
        this.editdata = res.body;
        if (this.editdata) {
          this.updateForm.patchValue({productId: this.editdata.id});
        }
      });
  }
}
