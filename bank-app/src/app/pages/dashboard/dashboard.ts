import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatToolbarModule } from '@angular/material/toolbar';
import { AccountService } from '../../core/services/account.service';
import { AuthService } from '../../core/services/auth.service';
import { AccountResponse } from '../../core/models/account.models';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDividerModule,
    MatToolbarModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  account: AccountResponse | null = null;
  loading = true;
  creatingAccount = false;
  userName = '';

  constructor(
    private accountService: AccountService,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.userName = this.authService.getUserName();
    this.loadBalance();
  }

  loadBalance(): void {
    this.loading = true;
    this.accountService.getBalance().subscribe({
      next: (res) => {
        this.account = res.data;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        if (err.status === 400) {
          this.account = null;
        }
      }
    });
  }

  createAccount(): void {
    this.creatingAccount = true;
    this.accountService.createAccount().subscribe({
      next: (res) => {
        this.account = res.data;
        this.creatingAccount = false;
        this.snackBar.open('Account created successfully!', 'Close', { duration: 3000, panelClass: 'snack-success' });
      },
      error: (err) => {
        this.creatingAccount = false;
        this.snackBar.open(err.error?.message ?? 'Failed to create account.', 'Close', { duration: 4000, panelClass: 'snack-error' });
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
