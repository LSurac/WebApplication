import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/commons/materials.module';
import { ApplicationstatePipe } from 'src/app/pipes/applicationstate.pipe';
import { ApplicantService } from 'src/app/services/applicant.service';
import { ApplicationService } from 'src/app/services/application.service';
import { SkillService } from 'src/app/services/skill.service';
import { ApplicantDto, ApplicationDto, EApplicationState, SkillDto } from 'src/app/services/Web_Application_Client';
import { LoadingIndicatorComponent } from '../../widgets/loading-indicator/loading-indicator.component';
import { firstValueFrom, Observable } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule, MaterialModule, ApplicationstatePipe, LoadingIndicatorComponent],
  selector: 'app-career',
  templateUrl: './career.component.html',
  styleUrls: ['./career.component.scss']
})
export class CareerComponent implements OnInit {
  @ViewChild(FormGroupDirective) formDirective: FormGroupDirective | undefined;
  
  private readonly applicantService: ApplicantService;
  private readonly skillService: SkillService;
  private readonly applicationService: ApplicationService;
  
  private snackBar: MatSnackBar;
  private fb: FormBuilder;

  public applicantForm: FormGroup;
  public applicantList: ApplicantDto[] = [];
  public skillList: SkillDto[] = [];
  public applicationList: ApplicationDto[] = [];
  public selectedApplicantId: number | undefined;
  public selectedApplicationId: number | undefined;
  public isLoading: boolean = false;
  public isOverlay: boolean = true;
  public displayedColumns: string[] = ['state', 'firstName', 'lastName', 'birthDate', 'skills'];
  public applicationStates = Object.keys(EApplicationState)
    .filter((key) => isNaN(Number(key)))
    .map((key) => ({
      value: (EApplicationState as any)[key as keyof typeof EApplicationState],
      label: key
    }));

  constructor() {
    this.fb = new FormBuilder();
    this.applicantService = inject(ApplicantService);
    this.skillService = inject(SkillService);
    this.applicationService = inject(ApplicationService);
    this.snackBar = inject(MatSnackBar);
    this.applicantForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      birthDate: [''],
      skills: [[]],
      applicationState: [null]
    });
  }

  public ngOnInit(): void {
    this.loadApplicationsAsync();
  }

  private async loadApplicationsAsync(): Promise<void> {
    this.isLoading = true;
    await firstValueFrom(this.applicationService.ApplicationListGet()).then(result => {
      this.applicationList = result.applicationList;
      this.loadApplicants();
    }).finally(() => {
      this.isLoading = false;
    });
  }

  private loadApplicants(): void {
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

  private loadSkills(applicantId: number | undefined): void {
    this.skillService.SkillListGet(applicantId).subscribe(result => {
      this.skillList = result.skillList;
    });
  }

  public onApplicationChange(applicationId: number | undefined): void {
    this.selectedApplicationId = applicationId;
    this.resetForm();
    this.loadApplicants();
  }

  public async onSaveAsync(): Promise<void> {
    try {
      if (!this.applicantForm.valid) { 
        return;
      }
      this.isLoading = true;
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
        await firstValueFrom(this.applicantService.ApplicantSet(applicant, this.selectedApplicationId)).then(() => {
          this.loadApplicationsAsync();
          this.resetForm();

          this.snackBar.open('Applicant has been saved!', 'X', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: 'app-notification-success'
          });
        }).catch(() => {
          this.snackBar.open('Error while saving Applicant', 'X', {
            duration: 3000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
            panelClass: 'app-notification-error'
          });
        });
      }
    } finally {
      this.isLoading = false;
    }
  }

  public onSelectApplicant(applicant: ApplicantDto): void {
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

  public resetForm(): void {
    this.applicantForm.reset();
    this.formDirective?.resetForm();
    this.selectedApplicantId = undefined;
  }
}