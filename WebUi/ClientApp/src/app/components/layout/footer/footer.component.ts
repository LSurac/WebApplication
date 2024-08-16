import { Component, inject } from '@angular/core';
import { Subscription } from 'rxjs';
import { MaterialModule } from 'src/app/commons/materials.module';
import { BreakpointService } from 'src/app/services/breakpoint.service';
import { ENVIRONMENT } from 'src/environments/environment';

@Component({
  standalone: true,
  imports: [MaterialModule],
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss',
})
export class FooterComponent {
  private readonly breakpointService: BreakpointService;
  private breakpointSubscription: Subscription | undefined;
  public currentBreakpoint: number | undefined;
  public customBreakpoint: number | undefined;
  public currentYear: number = new Date().getFullYear();

  constructor() { 
    this.breakpointService = inject(BreakpointService);
  }

  public ngOnInit() {
    this.customBreakpoint = ENVIRONMENT.customBreakpoints.sm;
    this.currentBreakpoint = window.innerWidth;
    this.breakpointSubscription = this.breakpointService.getBreakpoint().subscribe(breakpoint => this.currentBreakpoint = breakpoint);
  }

  public ngOnDestroy() {
    if (this.breakpointSubscription) {
      this.breakpointSubscription.unsubscribe();
    }
  }
}
