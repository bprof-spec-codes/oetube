import { APP_ROUTE_PROVIDER } from './route.provider';
import { AbpOAuthModule } from '@abp/ng.oauth';
import { AccountConfigModule } from '@abp/ng.account/config';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { CoreModule } from '@abp/ng.core';
import { FeatureManagementModule } from '@abp/ng.feature-management';
import { IdentityConfigModule } from '@abp/ng.identity/config';
import { NgModule } from '@angular/core';
import { SettingManagementConfigModule } from '@abp/ng.setting-management/config';
import { TenantManagementConfigModule } from '@abp/ng.tenant-management/config';
import { ThemeBasicModule } from '@abp/ng.theme.basic';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { environment } from '../environments/environment';
import { registerLocale } from '@abp/ng.core/locale';
import { GroupComponent } from './group/group.component';
import { GroupCreateComponent } from './group/group-create/group-create.component';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CoreModule.forRoot({
      environment,
      registerLocaleFn: registerLocale(),
    }),
    AbpOAuthModule.forRoot(),
    ThemeSharedModule.forRoot(),
    ThemeBasicModule.forRoot(),
    AccountConfigModule.forRoot(),
    IdentityConfigModule.forRoot(),
    TenantManagementConfigModule.forRoot(),
    SettingManagementConfigModule.forRoot(),
    FeatureManagementModule.forRoot(),
  ],
  declarations: [AppComponent],
  providers: [APP_ROUTE_PROVIDER],
  bootstrap: [AppComponent],
})
export class AppModule {}
