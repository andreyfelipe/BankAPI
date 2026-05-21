using BankApi.Application.DTOs.Transaction;
using FluentValidation;

namespace BankApi.Application.Validators;

public class TransactionRequestValidator : AbstractValidator<TransactionRequestDto>
{
    public TransactionRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}
