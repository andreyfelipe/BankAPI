using BankApi.Application.DTOs.Account;
using BankApi.Application.Services.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;

namespace BankApi.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;

    public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
    }

    public async Task<AccountResponseDto> CreateAccountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userExists = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (userExists is null)
            throw new InvalidOperationException("User not found.");

        var alreadyHasAccount = await _accountRepository.ExistsByUserIdAsync(userId, cancellationToken);
        if (alreadyHasAccount)
            throw new InvalidOperationException("User already has a bank account.");

        var account = new Account(userId);

        await _accountRepository.AddAsync(account, cancellationToken);
        await _accountRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(account);
    }

    public async Task<AccountResponseDto> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var account = await _accountRepository.GetByUserIdAsync(userId, cancellationToken);
        if (account is null)
            throw new InvalidOperationException("Account not found for this user.");

        return MapToDto(account);
    }

    private static AccountResponseDto MapToDto(Account account) => new()
    {
        AccountId = account.Id,
        AccountNumber = account.AccountNumber,
        Balance = account.Balance,
        CreatedAt = account.CreatedAt
    };
}
