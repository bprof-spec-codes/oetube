import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { VideoDto } from '@proxy/application/dtos/videos';
import { Observable, Subject } from 'rxjs';
import { CurrentUserService } from 'src/app/auth/current-user/current-user.service';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private connection?: HubConnection;
  private datas: Subject<VideoDto> = new Subject<VideoDto>();
  constructor(private currentUserService: CurrentUserService) {}

  public async startConnection() {
    if (this.currentUserService.getCurrentUser().isAuthenticated) {
      this.connection = new HubConnectionBuilder()
        .withUrl(environment.apis.default.url + '/signalr-hubs/notify', {
          headers: {
          },transport:HttpTransportType.WebSockets,accessTokenFactory:()=>this.currentUserService.getAccessToken()
        })
        .build();
      try {
        await this.connection.start();
        console.log('signalR connection started');
      } catch (e) {
        console.log(e);
      }
    }
  }
  public async stopConnection() {
    if (this.connection) {
      try {
        await this.connection.stop();
        console.log('signalR connection stopped');
      } catch (e) {
        console.log(e);
      }
    }
  }
  public observe(): Observable<VideoDto> {
    return this.datas.asObservable();
  }

  public onUploadCompleted() {
    if (this.connection) {
      this.connection.on('UploadCompleted', data => {
        this.datas.next(data);
      });
    }
  }
}
