import { Component } from '@angular/core';
import { MaterialModule } from 'src/app/commons/materials.module';
import { SidenavListComponent } from '../sidenav-list/sidenav-list.component';
import { NavigationHeaderComponent } from '../navigation-header/navigation-header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@Component({
  standalone: true,
  imports: [
    MaterialModule, 
    SidenavListComponent, 
    NavigationHeaderComponent, 
    BrowserAnimationsModule
  ],
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.scss',
})
export class NavigationComponent {

}
