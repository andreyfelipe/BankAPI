using System.Security.Claims;
using BankApi.Application.Common;
using BankApi.Application.DTOs.Account;
using BankApi.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<AccountResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccount(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        _logger.LogInformation("Creating account for user: {UserId}", userId);

        var result = await _accountService.CreateAccountAsync(userId, cancellationToken);
        return CreatedAtAction(nameof(GetBalance), ApiResponse<AccountResponseDto>.Ok(result, "Account created successfully."));
    }

    [HttpGet("balance")]
    [ProducesResponseType(typeof(ApiResponse<AccountResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBalance(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _accountService.GetBalanceAsync(userId, cancellationToken);
        return Ok(ApiResponse<AccountResponseDto>.Ok(result));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User identity not found in token.");
        return Guid.Parse(claim);
    }
}
