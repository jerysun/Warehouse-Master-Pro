import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { catchError } from 'rxjs';
import { Owner } from 'src/app/_interface/owner.model';
import { RepositoryService } from 'src/app/shared/repository.service';
import { OwnerUpdateComponent } from '../owner-update/owner-update.component';

@Component({
  selector: 'app-owner-list',
  templateUrl: './owner-list.component.html',
  styleUrls: ['./owner-list.component.css']
})
export class OwnerListComponent implements OnInit, AfterViewInit {
  isLoading = false;

  totalRows = 0;
  pageSize = 2;
  currentPage = 0;
  pageSizeOptions = [2, 4, 6, 10, 20];

  customResponseHeader: any;

  displayedColumns = ['id', 'name', 'quantityInStock', 'increaseStock', 'decreaseStock'];
  dataSource = new MatTableDataSource<Owner>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private repoService: RepositoryService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.loadData();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  public loadData = () => {
    this.isLoading = true;
    this.repoService.getData(`products?currentPage=${this.currentPage}&pageSize=${this.pageSize}`)
    .pipe(catchError(err => {
      this.isLoading = false;
      console.error(err);
      throw 'error in loadData():' + err;
    }))
    .subscribe(res => {
      this.customResponseHeader = JSON.parse(res.headers.get('x-pagination') ?? '');
      this.totalRows = this.customResponseHeader.TotalCount;
      this.dataSource.data = res.body as Owner[];
      setTimeout(() => {
        this.paginator.pageIndex = this.currentPage;
        this.paginator.length = this.customResponseHeader.TotalCount;
      });

      this.isLoading = false;
    });
  }

  pageChanged(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    this.loadData();
  }

  public customSort = (event: any) => {
    console.log(event);
    this.loadData();
  }

  public doFilter = (value: string) => {
    this.dataSource.filter = value.trim().toLocaleLowerCase();
    this.loadData();
  }

  public increaseStock = (id: number, toIncrease = true) => {
    const popup = this.dialog.open(OwnerUpdateComponent, {
      width: '400px',
      //height: '300px',
      exitAnimationDuration: '1000ms',
      enterAnimationDuration: '1000ms',
      data: {
        productId: id,
        toIncrease
      }
    });

    popup.afterClosed().subscribe(item => {
      this.loadData();
    });
  }

  public decreaseStock = (id: number) => {
    this.increaseStock(id, false);
  }
}
