namespace BankApi.Application.DTOs.Transaction;

public class TransactionResponseDto
{
    public Guid TransactionId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime CreatedAt { get; set; }
}
