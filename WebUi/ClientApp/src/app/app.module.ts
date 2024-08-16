import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { routes } from './app.routes';
import { AppComponent } from './app.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { APP_BASE_HREF, CommonModule } from '@angular/common';
import { ENVIRONMENT } from 'src/environments/environment';
import { API_BASE_URL, ApplicantClient, ApplicationClient, SkillClient } from './services/Web_Application_Client';
import { NavigationComponent } from './components/layout/navigation/navigation/navigation.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { RouterModule, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LayoutModule } from '@angular/cdk/layout';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

@NgModule({ 
  declarations: [
    AppComponent,
  ],
  bootstrap: [AppComponent], 
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    RouterOutlet,
    NavigationComponent,
    FooterComponent,
    FormsModule,
    CommonModule,
    LayoutModule,
  ], 
  providers: [
    {
      provide: APP_BASE_HREF,
      useFactory: (locale: string) => ENVIRONMENT.appBaseHref + "/" + locale + "/",
      deps: [LOCALE_ID]
    },
    { provide: API_BASE_URL, useFactory: () => ENVIRONMENT.apiBaseUrl },
    ApplicationClient,
    ApplicantClient,
    SkillClient,
    provideHttpClient(withInterceptorsFromDi()),
    provideAnimationsAsync(),
] 
})

export class AppModule { }