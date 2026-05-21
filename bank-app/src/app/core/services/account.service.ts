import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountResponse } from '../models/account.models';
import { ApiResponse } from '../models/api-response.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private readonly apiUrl = `${environment.apiUrl}/account`;

  constructor(private http: HttpClient) {}

  createAccount(): Observable<ApiResponse<AccountResponse>> {
    return this.http.post<ApiResponse<AccountResponse>>(`${this.apiUrl}/create`, {});
  }

  getBalance(): Observable<ApiResponse<AccountResponse>> {
    return this.http.get<ApiResponse<AccountResponse>>(`${this.apiUrl}/balance`);
  }
}
