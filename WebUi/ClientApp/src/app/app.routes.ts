import { Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { CompanyComponent } from './components/pages/company/company.component';
import { ProductsComponent } from './components/pages/products/products.component';
import { CustomersComponent } from './components/pages/customers/customers.component';
import { CareerComponent } from './components/pages/career/career.component';


export const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'company', component: CompanyComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'customers', component: CustomersComponent },
  { path: 'career', component: CareerComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home' },
];