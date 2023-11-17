import { Component } from '@angular/core';
import { PlaylistService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { CreateUpdatePlaylistDto, PlaylistDto, PlaylistItemDto, PlaylistQueryDto } from '@proxy/application/dtos/playlists';
import { LoadOptions } from 'devextreme/data';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';

@Component({
  selector: 'app-playlist-pagination-grid',
  templateUrl: './playlist-pagination-grid.component.html',
  styleUrls: ['./playlist-pagination-grid.component.scss']
})
export class PlaylistPaginationGridComponent extends PaginationGridComponent<PlaylistQueryDto,PlaylistDto,PlaylistItemDto,CreateUpdatePlaylistDto>
{
  constructor(public playlistService:PlaylistService){
    super()
  }
  getList(): Observable<PaginationDto<PlaylistItemDto>> {
      return this.playlistService.getList(this.queryArgs)
  }

handleFilter(options: LoadOptions<any>): void {
    
}
}
