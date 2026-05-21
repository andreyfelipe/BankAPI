using BankApi.Application.DTOs.Transaction;
using BankApi.Application.Services.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;

namespace BankApi.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionResponseDto> DepositAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByUserIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("Account not found for this user.");

        account.Deposit(amount);

        var transaction = new Transaction(account.Id, TransactionType.Deposit, amount, account.Balance);

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _accountRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(transaction);
    }

    public async Task<TransactionResponseDto> WithdrawAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByUserIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("Account not found for this user.");

        account.Withdraw(amount);

        var transaction = new Transaction(account.Id, TransactionType.Withdrawal, amount, account.Balance);

        await _transactionRepository.AddAsync(transaction, cancellationToken);
        await _accountRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(transaction);
    }

    public async Task<IEnumerable<StatementItemDto>> GetStatementAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByUserIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("Account not found for this user.");

        var transactions = await _transactionRepository.GetByAccountIdAsync(account.Id, cancellationToken);

        return transactions
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new StatementItemDto
            {
                Date = t.CreatedAt,
                Type = t.Type.ToString(),
                Amount = t.Amount,
                BalanceAfter = t.BalanceAfter
            });
    }

    private static TransactionResponseDto MapToDto(Transaction transaction) => new()
    {
        TransactionId = transaction.Id,
        Type = transaction.Type.ToString(),
        Amount = transaction.Amount,
        BalanceAfter = transaction.BalanceAfter,
        CreatedAt = transaction.CreatedAt
    };
}
