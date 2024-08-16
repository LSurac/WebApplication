import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { MaterialModule } from 'src/app/commons/materials.module';
import { BreakpointService } from 'src/app/services/breakpoint.service';
import { ENVIRONMENT } from 'src/environments/environment';

@Component({
  standalone: true,
  imports: [MaterialModule, CommonModule, RouterModule],
  selector: 'app-sidenav-list',
  templateUrl: './sidenav-list.component.html',
  styleUrls: ['./sidenav-list.component.scss']
})
export class SidenavListComponent implements OnInit {
  @Output() sidenavClose = new EventEmitter();
  private readonly breakpointService: BreakpointService;
  private breakpointSubscription: Subscription | undefined;
  private windowHeight: number | undefined;
  public currentBreakpoint: number | undefined;
  public customBreakpoint: number | undefined;
  public sidebarHeight: string | undefined;

  constructor() { 
    this.breakpointService = inject(BreakpointService);
  }

  public ngOnInit() {
    this.customBreakpoint = ENVIRONMENT.customBreakpoints.sm;
    this.windowHeight = window.innerHeight;
    this.currentBreakpoint = window.innerWidth;
    this.breakpointSubscription = this.breakpointService.getBreakpoint().subscribe(breakpoint => {
      this.currentBreakpoint = breakpoint;
      if (this.customBreakpoint && this.currentBreakpoint && this.currentBreakpoint > this.customBreakpoint) {
        this.onSidenavClose();
      }
    });

    this.setSidebarHeight();
    window.addEventListener('resize', () => {
      this.setSidebarHeight();
    });
  }

  public ngOnDestroy() {
    if (this.breakpointSubscription) {
      this.breakpointSubscription.unsubscribe();
    }
  }

  public onSidenavClose = () => this.sidenavClose.emit();

  private setSidebarHeight() {
    let footer = document.querySelector('#footer');
    if (footer != undefined) {
      this.windowHeight = window.innerHeight;
      const footerHeight = footer.clientHeight;
      const sidebarHeight = this.windowHeight - footerHeight;
      this.sidebarHeight = `${sidebarHeight}px`;
    }
  }
}
