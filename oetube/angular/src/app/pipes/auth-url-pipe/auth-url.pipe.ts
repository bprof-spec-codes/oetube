import { Pipe, PipeTransform,Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { Component, Input, OnDestroy, ChangeDetectorRef } from '@angular/core';
import {
  Subscription,
  distinctUntilChanged,
  BehaviorSubject,
  tap,
  filter,
  switchMap,
  map,
} from 'rxjs';
import { HttpResponse } from '@angular/common/http';

 @Injectable( {providedIn: 'root'
}) @Pipe({ name: 'auth', pure: false })
export class AuthUrlPipe implements PipeTransform, OnDestroy {
  private subscription = new Subscription();
  private transformValue = new BehaviorSubject<string>('');
  private latestValue!: string | SafeUrl;
  constructor(
    private httpClient: HttpClient,
    private domSanitizer: DomSanitizer,
    private cdr: ChangeDetectorRef
  ) {
    this.setUpSubscription();
  }

  transform(imagePath: string): string | SafeUrl {
    this.transformValue.next(imagePath);
    return this.latestValue;
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  private setUpSubscription(): void {
    const transformSubscription = this.transformValue
      .asObservable()
      .pipe(
        filter((v): v is string => !!v),
        distinctUntilChanged(),
        switchMap((imagePath: string) =>
          this.httpClient.get(imagePath, { observe: 'response', responseType: 'blob' }).pipe(
            map((response: HttpResponse<Blob>) => URL.createObjectURL(response.body)),
            map((unsafeBlobUrl: string) => this.domSanitizer.bypassSecurityTrustUrl(unsafeBlobUrl)),
            filter(blobUrl => blobUrl !== this.latestValue)
          )
        ),
        tap((imagePath: String | SafeUrl) => {
          this.latestValue = imagePath;
          this.cdr.markForCheck();
        })
      )
      .subscribe();
    this.subscription.add(transformSubscription);
  }
}
