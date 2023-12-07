import { Component, Input, ViewChild, Output, EventEmitter, OnInit } from '@angular/core';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import { UserDto } from '@proxy/application/dtos/oe-tube-users';
import { DxBulletComponent, DxButtonComponent, DxDataGridComponent, NestedOptionHost } from 'devextreme-angular';
import { DxoPagerComponent } from 'devextreme-angular/ui/nested';
import { LoadOptions } from 'devextreme/data';
import DataSource from 'devextreme/data/data_source';
import { DataGridPredefinedColumnButton } from 'devextreme/ui/data_grid';
import { Observable, lastValueFrom } from 'rxjs';
import { NgModule } from '@angular/core';
import { DxTabsModule } from 'devextreme-angular';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { TabbedItem } from 'devextreme/ui/form';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.scss'],
})

export class PlaylistComponent {
}
