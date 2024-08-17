import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicationClient, ApplicationListGetQuery, ApplicationListGetQueryResult } from './Web_Application_Client';

@Injectable({
  providedIn: 'root'
})
export class ApplicationService {

  constructor(private applicationClient: ApplicationClient) { }

  public ApplicationListGet(): Observable<ApplicationListGetQueryResult> {
    let query = new ApplicationListGetQuery();
    return this.applicationClient.applicationListGet(query);
  }
}
