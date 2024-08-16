import { Injectable, inject } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ENVIRONMENT } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BreakpointService {
  private readonly breakpointObserver: BreakpointObserver;
  private readonly breakpointMap: Map<string, number>;
  private readonly breakpoints: string[];

  constructor() {
    this.breakpointObserver = inject(BreakpointObserver);

    this.breakpoints = [
      Breakpoints.XLarge,
      Breakpoints.Large, 
      Breakpoints.Medium,
      Breakpoints.Small, 
      `(min-width: ${ENVIRONMENT.customBreakpoints.xs}px) and ${Breakpoints.XSmall}`,
      `(min-width: ${ENVIRONMENT.customBreakpoints.xxs}px) and (max-width: ${ENVIRONMENT.customBreakpoints.xs}px)`,
      `(max-width: ${ENVIRONMENT.customBreakpoints.xxs}px)`
    ];

    this.breakpointMap = new Map([
      [Breakpoints.XLarge, ENVIRONMENT.customBreakpoints.xxl],
      [Breakpoints.Large, ENVIRONMENT.customBreakpoints.xl],
      [Breakpoints.Medium, ENVIRONMENT.customBreakpoints.lg],
      [Breakpoints.Small, ENVIRONMENT.customBreakpoints.md],
      [`(min-width: ${ENVIRONMENT.customBreakpoints.xs}px) and ${Breakpoints.XSmall}`, ENVIRONMENT.customBreakpoints.sm],
      [`(min-width: ${ENVIRONMENT.customBreakpoints.xxs}px) and (max-width: ${ENVIRONMENT.customBreakpoints.xs}px)`, ENVIRONMENT.customBreakpoints.xs],
      [`(max-width: ${ENVIRONMENT.customBreakpoints.xxs}px)`, ENVIRONMENT.customBreakpoints.xxs],
    ]);
  }

  public getBreakpoint(): Observable<number> {
    return this.breakpointObserver.observe(this.breakpoints).pipe(
      map(result => {
        for (let [key, value] of this.breakpointMap.entries()) {
          if (result.breakpoints[key]) {
            return value;
          }
        }
        
        return window.innerWidth;
      })
    );
  }
}
