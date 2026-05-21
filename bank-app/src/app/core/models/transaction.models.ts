export interface TransactionRequest {
  amount: number;
}

export interface TransactionResponse {
  transactionId: string;
  type: string;
  amount: number;
  balanceAfter: number;
  createdAt: string;
}

export interface StatementItem {
  date: string;
  type: string;
  amount: number;
  balanceAfter: number;
}
