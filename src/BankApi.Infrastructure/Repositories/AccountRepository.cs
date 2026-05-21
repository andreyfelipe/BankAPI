using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Accounts.FindAsync(new object[] { id }, cancellationToken);

    public async Task<Account?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, cancellationToken);

    public async Task<bool> ExistsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _context.Accounts.AnyAsync(a => a.UserId == userId, cancellationToken);

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        => await _context.Accounts.AddAsync(account, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
