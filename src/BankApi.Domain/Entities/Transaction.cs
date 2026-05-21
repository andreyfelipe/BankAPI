using BankApi.Domain.Enums;

namespace BankApi.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public decimal BalanceAfter { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid AccountId { get; private set; }

    public Account? Account { get; private set; }

    protected Transaction() { }

    public Transaction(Guid accountId, TransactionType type, decimal amount, decimal balanceAfter)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        CreatedAt = DateTime.UtcNow;
    }
}
