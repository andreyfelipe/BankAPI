import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { TransactionService } from '../../core/services/transaction.service';

@Component({
  selector: 'app-withdraw',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatIconModule,
    MatToolbarModule
  ],
  templateUrl: './withdraw.html',
  styleUrl: './withdraw.scss'
})
export class WithdrawComponent {
  form: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private transactionService: TransactionService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.form = this.fb.group({
      amount: ['', [Validators.required, Validators.min(0.01)]]
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.transactionService.withdraw({ amount: this.form.value.amount }).subscribe({
      next: (res) => {
        this.loading = false;
        this.snackBar.open(
          `Withdrawal of ${res.data.amount.toFixed(2)} successful! Remaining: ${res.data.balanceAfter.toFixed(2)}`,
          'Close',
          { duration: 4000, panelClass: 'snack-success' }
        );
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open(err.error?.message ?? 'Withdrawal failed.', 'Close', { duration: 4000, panelClass: 'snack-error' });
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
