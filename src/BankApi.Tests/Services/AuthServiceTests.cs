using BankApi.Application.DTOs.Auth;
using BankApi.Application.Services;
using BankApi.Application.Services.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BankApi.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtServiceMock = new Mock<IJwtService>();
        _service = new AuthService(_userRepoMock.Object, _passwordHasherMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnRegisterResponse()
    {
        var request = new RegisterRequestDto
        {
            Name = "John Doe",
            Email = "john@example.com",
            Password = "Secret123"
        };

        _userRepoMock.Setup(r => r.ExistsByEmailAsync(request.Email, default)).ReturnsAsync(false);
        _passwordHasherMock.Setup(h => h.Hash(request.Password)).Returns("hashed_password");

        var result = await _service.RegisterAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>(), default), Times.Once);
        _userRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldThrowInvalidOperationException()
    {
        var request = new RegisterRequestDto
        {
            Name = "John Doe",
            Email = "existing@example.com",
            Password = "Secret123"
        };

        _userRepoMock.Setup(r => r.ExistsByEmailAsync(request.Email, default)).ReturnsAsync(true);

        var act = async () => await _service.RegisterAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("A user with this email already exists.");
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        var request = new LoginRequestDto
        {
            Email = "john@example.com",
            Password = "Secret123"
        };

        var user = new User("John Doe", request.Email, "hashed_password");

        _userRepoMock.Setup(r => r.GetByEmailAsync(request.Email, default)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.Verify(request.Password, user.PasswordHash)).Returns(true);
        _jwtServiceMock.Setup(j => j.GenerateToken(user)).Returns("jwt_token");
        _jwtServiceMock.Setup(j => j.GetExpiration()).Returns(DateTime.UtcNow.AddHours(1));

        var result = await _service.LoginAsync(request);

        result.Should().NotBeNull();
        result.Token.Should().Be("jwt_token");
        result.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldThrowUnauthorizedAccessException()
    {
        var request = new LoginRequestDto
        {
            Email = "john@example.com",
            Password = "WrongPassword"
        };

        var user = new User("John Doe", request.Email, "hashed_password");

        _userRepoMock.Setup(r => r.GetByEmailAsync(request.Email, default)).ReturnsAsync(user);
        _passwordHasherMock.Setup(h => h.Verify(request.Password, user.PasswordHash)).Returns(false);

        var act = async () => await _service.LoginAsync(request);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials.");
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ShouldThrowUnauthorizedAccessException()
    {
        var request = new LoginRequestDto
        {
            Email = "ghost@example.com",
            Password = "Secret123"
        };

        _userRepoMock.Setup(r => r.GetByEmailAsync(request.Email, default)).ReturnsAsync((User?)null);

        var act = async () => await _service.LoginAsync(request);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid credentials.");
    }
}
