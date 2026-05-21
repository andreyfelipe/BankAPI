using BankApi.Application.Services;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BankApi.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _accountRepoMock = new Mock<IAccountRepository>();
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _service = new TransactionService(_accountRepoMock.Object, _transactionRepoMock.Object);
    }

    [Fact]
    public async Task Deposit_WithValidAmount_ShouldIncreaseBalance()
    {
        var userId = Guid.NewGuid();
        var account = new Account(userId);
        _accountRepoMock.Setup(r => r.GetByUserIdAsync(userId, default)).ReturnsAsync(account);

        var result = await _service.DepositAsync(userId, 500m);

        result.Should().NotBeNull();
        result.Amount.Should().Be(500m);
        result.BalanceAfter.Should().Be(500m);
        result.Type.Should().Be("Deposit");
        _transactionRepoMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Transaction>(), default), Times.Once);
        _accountRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Withdraw_WithSufficientBalance_ShouldDecreaseBalance()
    {
        var userId = Guid.NewGuid();
        var account = new Account(userId);
        account.Deposit(1000m);
        _accountRepoMock.Setup(r => r.GetByUserIdAsync(userId, default)).ReturnsAsync(account);

        var result = await _service.WithdrawAsync(userId, 300m);

        result.Should().NotBeNull();
        result.Amount.Should().Be(300m);
        result.BalanceAfter.Should().Be(700m);
        result.Type.Should().Be("Withdrawal");
    }

    [Fact]
    public async Task Withdraw_WithInsufficientBalance_ShouldThrowInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var account = new Account(userId);
        account.Deposit(100m);
        _accountRepoMock.Setup(r => r.GetByUserIdAsync(userId, default)).ReturnsAsync(account);

        var act = async () => await _service.WithdrawAsync(userId, 500m);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Insufficient funds.");
    }

    [Fact]
    public async Task Deposit_WithNonExistentAccount_ShouldThrowInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        _accountRepoMock.Setup(r => r.GetByUserIdAsync(userId, default)).ReturnsAsync((Account?)null);

        var act = async () => await _service.DepositAsync(userId, 100m);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Account not found for this user.");
    }

    [Fact]
    public async Task GetStatement_WithExistingAccount_ShouldReturnOrderedTransactions()
    {
        var userId = Guid.NewGuid();
        var account = new Account(userId);
        account.Deposit(1000m);

        var transactions = new List<Domain.Entities.Transaction>
        {
            new Domain.Entities.Transaction(account.Id, Domain.Enums.TransactionType.Deposit, 1000m, 1000m),
            new Domain.Entities.Transaction(account.Id, Domain.Enums.TransactionType.Withdrawal, 300m, 700m)
        };

        _accountRepoMock.Setup(r => r.GetByUserIdAsync(userId, default)).ReturnsAsync(account);
        _transactionRepoMock.Setup(r => r.GetByAccountIdAsync(account.Id, default)).ReturnsAsync(transactions);

        var result = await _service.GetStatementAsync(userId);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Deposit_WithZeroAmount_ShouldThrowInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var account = new Account(userId);
        _accountRepoMock.Setup(r => r.GetByUserIdAsync(userId, default)).ReturnsAsync(account);

        var act = async () => await _service.DepositAsync(userId, 0m);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Deposit amount must be greater than zero.");
    }
}
