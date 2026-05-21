using BankApi.Domain.Entities;

namespace BankApi.Application.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    DateTime GetExpiration();
}
