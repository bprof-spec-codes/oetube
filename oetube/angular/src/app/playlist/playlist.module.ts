import { NgModule } from '@angular/core';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { DxButtonModule, DxDataGridModule, DxTabPanelModule, DxTemplateModule, DxTreeListModule } from 'devextreme-angular';

import { PlaylistViewComponent } from './playlist-view/playlist-view.component';
import { PlaylistEditorComponent, PlaylistUpdateComponent,PlaylistCreateComponent } from './playlist-editor/playlist-editor.component';
import { PlaylistContentsComponent } from './playlist-explore/playlist-contents/playlist-contents.component';
import { CollectorModule } from '../collector.module';
import { VideoModule } from '../video/video.module';
import { PlaylistExploreComponent } from './playlist-explore/playlist-explore.component';
import { PlaylistSearchComponent } from './playlist-explore/playlist-search/playlist-search.component';
import { PlaylistDataSourceComponent } from './playlist-explore/playlist-data-source/playlist-data-source.component';
import { PlaylistListItemComponent } from './playlist-explore/playlist-contents/playlist-list-item/playlist-list-item.component';
import { PlaylistYourListsComponent } from './playlist-your-lists/playlist-your-lists.component';
import { PlaylistScrollViewModule } from './playlist-scroll-view.module';
import { VideoScrollViewModule } from '../video/video-scroll-view.module';
import { PlaylistDetailsComponent } from './playlist-details/playlist-details.component';

@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistYourListsComponent,
    PlaylistViewComponent,
    PlaylistEditorComponent,
    PlaylistUpdateComponent,
    PlaylistCreateComponent,
    PlaylistDetailsComponent,
    PlaylistExploreComponent,
  ],
  imports: [
    PlaylistRoutingModule,
    CollectorModule,
    VideoScrollViewModule,
    PlaylistScrollViewModule
  ]
})
export class PlaylistModule {}
