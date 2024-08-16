import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/commons/materials.module';
import { ApplicationstatePipe } from 'src/app/pipes/applicationstate.pipe';
import { ApplicantService } from 'src/app/services/applicant.service';
import { ApplicationService } from 'src/app/services/application.service';
import { SkillService } from 'src/app/services/skill.service';
import { ApplicantDto, ApplicationDto, EApplicationState, SkillDto } from 'src/app/services/Web_Application_Client';

@Component({
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule, MaterialModule, ApplicationstatePipe],
  selector: 'app-career',
  templateUrl: './career.component.html',
  styleUrls: ['./career.component.scss']
})
export class CareerComponent implements OnInit {
  applicantForm: FormGroup;
  applicantList: ApplicantDto[] = [];
  skillList: SkillDto[] = [];
  applicationList: ApplicationDto[] = [];
  selectedApplicantId: number | undefined;
  selectedApplicationId: number | undefined;
  displayedColumns: string[] = ['state', 'firstName', 'lastName', 'birthDate', 'skills'];
  applicationStates = Object.keys(EApplicationState)
    .filter((key) => isNaN(Number(key)))
    .map((key) => ({
      value: (EApplicationState as any)[key as keyof typeof EApplicationState],
      label: key
    }));

  constructor(
    private fb: FormBuilder,
    private applicantService: ApplicantService,
    private skillService: SkillService,
    private applicationService: ApplicationService
  ) {
    this.applicantForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      birthDate: [''],
      skills: [[]],
      applicationState: [null]
    });
  }

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.applicationService.ApplicationListGet().subscribe(result => {
      this.applicationList = result.applicationList;
      this.loadApplicants();
    });
  }

  loadApplicants(): void {
    if (this.selectedApplicationId) {
      let selectedApplication = this.applicationList.filter(application => application.id == this.selectedApplicationId);

      if (selectedApplication[0] != undefined && selectedApplication[0].applicantList != undefined) {
        this.applicantList = selectedApplication[0].applicantList;
      }
      
      if (this.applicantList.length == 0) {
        this.applicantService.ApplicantListGet(this.selectedApplicationId).subscribe(result => {
          this.applicantList = result.applicantList;
        });
      }

      this.loadSkills(undefined);
    }
  }

  loadSkills(applicantId: number | undefined): void {
    this.skillService.SkillListGet(applicantId).subscribe(result => {
      this.skillList = result.skillList;
    });
  }

  onApplicationChange(applicationId: number | undefined): void {
    this.selectedApplicationId = applicationId;
    this.resetForm();
    this.loadApplicants();
  }

  onSave(): void {
    let applicant = new ApplicantDto();
    let formValues = this.applicantForm.value;

    applicant.id = this.selectedApplicantId || 0;
    applicant.applicationState = formValues.applicationState;
    applicant.firstName = formValues.firstName;
    applicant.lastName = formValues.lastName;
    let birthDateValue = formValues.birthDate;
    applicant.birthDate = birthDateValue ? new Date(birthDateValue) : new Date();

    applicant.skillList = formValues.skills.map((skillId: number, isCurrent: boolean) => {
      let skill = this.skillList.find(s => s.id === skillId);
      return new SkillDto({
        id: skillId,
        description: skill?.description || 'Unknown',
        isCurrent: true
      });
    });

    if (this.selectedApplicationId != undefined) {
      this.applicantService.ApplicantSet(applicant, this.selectedApplicationId).subscribe(() => {
        this.loadApplications();
        this.resetForm();
      });
    }
  }

  onSelectApplicant(applicant: ApplicantDto): void {
    this.selectedApplicantId = applicant.id;
    this.loadSkills(applicant.id);
    
    let formattedDate = applicant.birthDate instanceof Date
    ? applicant.birthDate.toISOString().split('T')[0]
    : applicant.birthDate;

    this.applicantForm.patchValue({
      applicationState: applicant.applicationState,
      firstName: applicant.firstName,
      lastName: applicant.lastName,
      birthDate: formattedDate,
      skills: applicant.skillList?.map(skill => skill.id)
    });
  }

  onEdit(applicant: ApplicantDto): void {
    this.selectedApplicantId = applicant.id;
    this.applicantForm.patchValue(applicant);
    this.loadSkills(this.selectedApplicantId);
  }

  public resetForm(): void {
    this.applicantForm.reset();
    this.selectedApplicantId = undefined;
  }
}