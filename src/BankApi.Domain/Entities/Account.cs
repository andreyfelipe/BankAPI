namespace BankApi.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UserId { get; private set; }

    public User? User { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

    protected Account() { }

    public Account(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AccountNumber = GenerateAccountNumber();
        Balance = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Deposit amount must be greater than zero.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Withdrawal amount must be greater than zero.");

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;
    }

    private static string GenerateAccountNumber()
    {
        var random = new Random();
        return random.Next(10000000, 99999999).ToString();
    }
}
