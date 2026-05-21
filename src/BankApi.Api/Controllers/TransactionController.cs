using System.Security.Claims;
using BankApi.Application.Common;
using BankApi.Application.DTOs.Transaction;
using BankApi.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

[ApiController]
[Route("api/transaction")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IValidator<TransactionRequestDto> _transactionValidator;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(
        ITransactionService transactionService,
        IValidator<TransactionRequestDto> transactionValidator,
        ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _transactionValidator = transactionValidator;
        _logger = logger;
    }

    [HttpPost("deposit")]
    [ProducesResponseType(typeof(ApiResponse<TransactionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequestDto request, CancellationToken cancellationToken)
    {
        var validation = await _transactionValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation failed.", validation.Errors.Select(e => e.ErrorMessage)));

        var userId = GetUserId();
        _logger.LogInformation("Deposit of {Amount} for user: {UserId}", request.Amount, userId);

        var result = await _transactionService.DepositAsync(userId, request.Amount, cancellationToken);
        return Ok(ApiResponse<TransactionResponseDto>.Ok(result, "Deposit completed successfully."));
    }

    [HttpPost("withdraw")]
    [ProducesResponseType(typeof(ApiResponse<TransactionResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Withdraw([FromBody] TransactionRequestDto request, CancellationToken cancellationToken)
    {
        var validation = await _transactionValidator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return BadRequest(ApiResponse<object>.Fail("Validation failed.", validation.Errors.Select(e => e.ErrorMessage)));

        var userId = GetUserId();
        _logger.LogInformation("Withdrawal of {Amount} for user: {UserId}", request.Amount, userId);

        var result = await _transactionService.WithdrawAsync(userId, request.Amount, cancellationToken);
        return Ok(ApiResponse<TransactionResponseDto>.Ok(result, "Withdrawal completed successfully."));
    }

    [HttpGet("statement")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StatementItemDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatement(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await _transactionService.GetStatementAsync(userId, cancellationToken);
        return Ok(ApiResponse<IEnumerable<StatementItemDto>>.Ok(result));
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new UnauthorizedAccessException("User identity not found in token.");
        return Guid.Parse(claim);
    }
}
