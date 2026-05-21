using BankApi.Application.DTOs.Auth;
using BankApi.Application.Services.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;

namespace BankApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var emailExists = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
        if (emailExists)
            throw new InvalidOperationException("A user with this email already exists.");

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = new User(request.Name, request.Email, passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return new RegisterResponseDto
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var passwordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!passwordValid)
            throw new UnauthorizedAccessException("Invalid credentials.");

        var token = _jwtService.GenerateToken(user);
        var expiresAt = _jwtService.GetExpiration();

        return new LoginResponseDto
        {
            Token = token,
            Name = user.Name,
            Email = user.Email,
            ExpiresAt = expiresAt
        };
    }
}
