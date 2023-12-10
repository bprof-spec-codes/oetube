import { NgModule } from '@angular/core';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { DxButtonModule, DxDataGridModule, DxTabPanelModule, DxTemplateModule, DxTreeListModule } from 'devextreme-angular';

import { PlaylistViewComponent } from './playlist-view/playlist-view.component';
import { PlaylistEditorComponent } from './playlist-editor/playlist-editor.component';
import { PlaylistContentsComponent } from './playlist-explore/playlist-contents/playlist-contents.component';
import { CollectorModule } from '../collector.module';
import { VideoModule } from '../video/video.module';
import { PlaylistExploreComponent } from './playlist-explore/playlist-explore.component';
import { PlaylistSearchComponent } from './playlist-explore/playlist-search/playlist-search.component';
import { PlaylistDataSourceComponent } from './playlist-explore/playlist-data-source/playlist-data-source.component';
import { PlaylistListItemComponent } from './playlist-explore/playlist-contents/playlist-list-item/playlist-list-item.component';
import { PlaylistYourListsComponent } from './playlist-your-lists/playlist-your-lists.component';

@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistYourListsComponent,
    PlaylistCreateComponent,
    PlaylistViewComponent,
    PlaylistEditorComponent,
    PlaylistContentsComponent,
    PlaylistSearchComponent,
    PlaylistDataSourceComponent,
    PlaylistExploreComponent,
    PlaylistListItemComponent,
  ],
  imports: [
    PlaylistRoutingModule,
    CollectorModule,
    VideoModule
  ],
  exports: [PlaylistContentsComponent, PlaylistSearchComponent, PlaylistDataSourceComponent],
})
export class PlaylistModule {}
