using BankApi.Application.DTOs.Account;

namespace BankApi.Application.Services.Interfaces;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAccountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AccountResponseDto> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
}
