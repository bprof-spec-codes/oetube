import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private connection?: HubConnection;
  private datas:Subject<any>=new Subject<any>()
  constructor() {}

  public startConnection() {
    return new Promise((resolve, reject) => {
      this.connection = new HubConnectionBuilder().withUrl(environment.apis.default.url+'/signalr-hubs/notify').build();
      this.connection
        .start()
        .then(() => {
          console.log('signalr-connection started');
          return resolve(true);
        })
        .catch((error: any) => {
          console.log('signalr-connection error: ' + error);
          reject(error);
        });
    });
  }
  public observe():Observable<any>
  {
    return this.datas.asObservable()
  } 

  public subscribe(){
    this.connection.on("video-avaliable",(data)=>{
      debugger
      this.datas.next(data)
    })
  }

}
