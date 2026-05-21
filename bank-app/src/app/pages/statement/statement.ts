import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatChipsModule } from '@angular/material/chips';
import { TransactionService } from '../../core/services/transaction.service';
import { StatementItem } from '../../core/models/transaction.models';

@Component({
  selector: 'app-statement',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatIconModule,
    MatToolbarModule,
    MatChipsModule
  ],
  templateUrl: './statement.html',
  styleUrl: './statement.scss'
})
export class StatementComponent implements OnInit {
  displayedColumns: string[] = ['date', 'type', 'amount', 'balanceAfter'];
  transactions: StatementItem[] = [];
  loading = true;

  constructor(
    private transactionService: TransactionService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadStatement();
  }

  loadStatement(): void {
    this.loading = true;
    this.transactionService.getStatement().subscribe({
      next: (res) => {
        this.transactions = res.data;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open(err.error?.message ?? 'Failed to load statement.', 'Close', { duration: 4000, panelClass: 'snack-error' });
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }

  isDeposit(type: string): boolean {
    return type === 'Deposit';
  }
}
