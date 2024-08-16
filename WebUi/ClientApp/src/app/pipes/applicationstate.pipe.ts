import { Pipe, PipeTransform } from '@angular/core';
import { EApplicationState } from '../services/Web_Application_Client';

@Pipe({
  standalone: true,
  name: 'applicationstate'
})
export class ApplicationstatePipe implements PipeTransform {

  transform(value: EApplicationState): string {
    switch (value) {
      case EApplicationState.Applied:
        return 'how_to_reg';
      case EApplicationState.Interview:
        return 'question_answer';
      case EApplicationState.TechnicalInterview:
        return 'engineering';
      case EApplicationState.RecruitmentTest:
        return 'assignment';
      case EApplicationState.Hired:
        return 'thumb_up';
      case EApplicationState.Rejected:
        return 'thumb_down';
      default:
        return 'help_outline';
    }
  }

}
