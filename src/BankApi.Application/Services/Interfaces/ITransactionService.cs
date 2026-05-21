using BankApi.Application.DTOs.Transaction;

namespace BankApi.Application.Services.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponseDto> DepositAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default);
    Task<TransactionResponseDto> WithdrawAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default);
    Task<IEnumerable<StatementItemDto>> GetStatementAsync(Guid userId, CancellationToken cancellationToken = default);
}
