import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicantClient, ApplicantDto, ApplicantGetQuery, ApplicantGetQueryResult, ApplicantListGetQuery, ApplicantListGetQueryResult, ApplicantSetCommand, ApplicantSetCommandResult } from './Web_Application_Client';

@Injectable({
  providedIn: 'root'
})
export class ApplicantService {

  constructor(private applicantClient: ApplicantClient) { }

  public ApplicantGet(applicantId: number): Observable<ApplicantGetQueryResult> {
    let query = new ApplicantGetQuery({
      applicantId: applicantId
    });
    return this.applicantClient.applicantGet(query);
  }

  public ApplicantListGet(applicationId: number): Observable<ApplicantListGetQueryResult> {
    let query = new ApplicantListGetQuery({
      applicationId: applicationId
    });
    return this.applicantClient.applicantListGet(query);
  }
 
  public ApplicantSet(applicant: ApplicantDto, applicationId: number): Observable<ApplicantSetCommandResult> {
    let command = new ApplicantSetCommand({
      applicant: applicant,
      applicationId: applicationId
    });
    return this.applicantClient.applicantSet(command);
  }
}
