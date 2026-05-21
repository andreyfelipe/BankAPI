import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StatementItem, TransactionRequest, TransactionResponse } from '../models/transaction.models';
import { ApiResponse } from '../models/api-response.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private readonly apiUrl = `${environment.apiUrl}/transaction`;

  constructor(private http: HttpClient) {}

  deposit(request: TransactionRequest): Observable<ApiResponse<TransactionResponse>> {
    return this.http.post<ApiResponse<TransactionResponse>>(`${this.apiUrl}/deposit`, request);
  }

  withdraw(request: TransactionRequest): Observable<ApiResponse<TransactionResponse>> {
    return this.http.post<ApiResponse<TransactionResponse>>(`${this.apiUrl}/withdraw`, request);
  }

  getStatement(): Observable<ApiResponse<StatementItem[]>> {
    return this.http.get<ApiResponse<StatementItem[]>>(`${this.apiUrl}/statement`);
  }
}
