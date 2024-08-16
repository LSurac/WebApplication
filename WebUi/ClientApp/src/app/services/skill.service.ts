import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SkillClient, SkillDto, SkillListGetQuery, SkillListGetQueryResult, SkillSetCommand, SkillSetCommandResult } from './Web_Application_Client';

@Injectable({
  providedIn: 'root'
})
export class SkillService {

  constructor(private skillClient: SkillClient) { }

  public SkillListGet(applicantId: number | undefined): Observable<SkillListGetQueryResult> {
    let query = new SkillListGetQuery({
      applicantId: applicantId,
    });
    return this.skillClient.skillListGet(query);
  }

  public SkillSet(applicantId: number, skill: SkillDto): Observable<SkillSetCommandResult> {
    let command = new SkillSetCommand({
      applicantId: applicantId,
      skill: skill
    });
    return this.skillClient.skillSet(command);
  }
}
