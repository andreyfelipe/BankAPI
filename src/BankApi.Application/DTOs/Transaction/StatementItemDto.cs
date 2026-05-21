namespace BankApi.Application.DTOs.Transaction;

public class StatementItemDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
}
