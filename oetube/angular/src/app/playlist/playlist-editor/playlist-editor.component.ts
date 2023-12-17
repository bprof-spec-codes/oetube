import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { PlaylistService } from '@proxy/application';
import { CreateUpdatePlaylistDto, PlaylistDto } from '@proxy/application/dtos/playlists';
import { DxButtonComponent } from 'devextreme-angular';

@Component({
  selector: 'app-playlist-editor',
  templateUrl: './playlist-editor.component.html',
  styleUrls: ['./playlist-editor.component.scss'],
})
export class PlaylistEditorComponent {
  @Input() submitButtonOptions: Partial<DxButtonComponent>;
  get inputModel(){
    return undefined
  }
  model: CreateUpdatePlaylistDto;
  @Input() title: string;
  @Output() submitted = new EventEmitter<PlaylistDto>();
  defaultImgUrl: string;
  constructor(protected playlistService: PlaylistService) {}
  onSubmit(event: Event) {}
  modelToJson() {
    return JSON.stringify(this.model, null, 4);
  }
  protected delete() {}
}
@Component({
  selector: 'app-playlist-create',
  templateUrl: './playlist-editor.component.html',
  styleUrls: ['./playlist-editor.component.scss'],
})
export class PlaylistCreateComponent extends PlaylistEditorComponent implements OnInit {
  @Input() submitButtonOptions: Partial<DxButtonComponent> = {
    useSubmitBehavior: true,
    text: 'Submit',
  };
  @Input() title = 'Create a playlist';

  model: CreateUpdatePlaylistDto = {
    name: '',
    description: '',
    items: [],
    image: null,
  };
  ngOnInit(): void {
    this.playlistService.getDefaultImage().subscribe(r => {
      this.defaultImgUrl = URL.createObjectURL(r);
    });
  }

  async onSubmit(event: Event) {
    this.playlistService.create(this.model).subscribe({
      next: v => {
        this.submitted.emit(v);
      },
      error: e => {
        console.log(e);
      },
    });
  }
}
@Component({
  selector: 'app-playlist-update',
  templateUrl: './playlist-editor.component.html',
  styleUrls: ['./playlist-editor.component.scss'],
})
export class PlaylistUpdateComponent extends PlaylistEditorComponent {
  @Input() submitButtonOptions: Partial<DxButtonComponent> = {
    useSubmitBehavior: true,
    text: 'Update',
  };
  @Input() title: string = 'Edit your playlist';

  model: CreateUpdatePlaylistDto;
  _inputModel: PlaylistDto;
  get inputModel() {
    return this._inputModel;
  }
  @Input() set inputModel(v: PlaylistDto) {
    if (v) {
      this._inputModel = v;
      this.model = {
        name: v.name,
        items: v.items,
        description: v.description,
        image: null,
      };
      this.defaultImgUrl = v.image;
    }
  }

  @Output() deleted: EventEmitter<PlaylistDto> = new EventEmitter();

  async onSubmit(event: Event) {
    this.playlistService.update(this.inputModel.id, this.model).subscribe({
      next: v => {
        this.submitted.emit(v);
      },
      error: e => {
        console.log(e);
      },
    });
  }

  public async delete() {
    this.playlistService.delete(this.inputModel.id).subscribe({
      next: () => {
        this.deleted.emit(this.inputModel);
      },
      error: e => console.log(e),
    });
  }
}
