import { Component, EventEmitter, OnDestroy, OnInit, Output, inject } from '@angular/core';
import { Subscription } from 'rxjs';
import { MaterialModule } from 'src/app/commons/materials.module';
import { CommonModule } from '@angular/common';
import { BreakpointService } from 'src/app/services/breakpoint.service';
import { ENVIRONMENT } from 'src/environments/environment';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { filter } from 'rxjs/operators';

type NavigationItem = { label: string; link: string };

@Component({
  standalone: true,
  imports: [MaterialModule, CommonModule, RouterModule],
  selector: 'app-navigation-header',
  templateUrl: './navigation-header.component.html',
  styleUrls: ['./navigation-header.component.scss']
})
export class NavigationHeaderComponent implements OnInit, OnDestroy {
  @Output() public sidenavToggle = new EventEmitter();
  private readonly breakpointService: BreakpointService;
  private breakpointSubscription: Subscription | undefined;
  private routerSubscription: Subscription | undefined;
  public customBreakpoint: number | undefined;
  public currentBreakpoint: number | undefined;
  public activeLink: NavigationItem | undefined;
  public navigationItem: NavigationItem[] | undefined;

  constructor(private router: Router) { 
    this.breakpointService = inject(BreakpointService);
  }

  public ngOnInit() {
    this.navigationItem = [
      { label: 'Company', link: 'company' },
      { label: 'Products', link: 'products' },
      { label: 'Customers', link: 'customers' },
      { label: 'Career', link: 'career' },
    ];
    
    this.routerSubscription = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => this.setActiveLink());

    this.setActiveLink();
    
    this.customBreakpoint = ENVIRONMENT.customBreakpoints.sm;
    this.currentBreakpoint = window.innerWidth;
    this.breakpointSubscription = this.breakpointService.getBreakpoint().subscribe(breakpoint => this.currentBreakpoint = breakpoint);
  }

  public ngOnDestroy() {
    if (this.breakpointSubscription) {
      this.breakpointSubscription.unsubscribe();
    }
    if (this.routerSubscription) {
      this.routerSubscription.unsubscribe();
    }
  }

  public onToggleSidenav = () => this.sidenavToggle.emit();

  private setActiveLink() {
    const currentUrl = this.router.url.slice(1);
    this.activeLink = this.navigationItem?.find(item => item.link === currentUrl);
  }
}
