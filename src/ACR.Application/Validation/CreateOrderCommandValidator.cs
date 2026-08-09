using ACR.Application.Commands;
using ACR.Domain.Common;
using FluentValidation;

namespace ACR.Application.Validation;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(command => command.CustomerId)
        .NotEmpty()
        .WithErrorCode(ErrorCodes.Validation.CUSTOMER_ID_REQUIRED)
        .WithMessage("A customer ID is required.");

        RuleFor(command => command.TotalAmount)
        .GreaterThanOrEqualTo(0m)
        .WithErrorCode(ErrorCodes.Validation.INVALID_AMOUNT)
        .WithMessage("Total amount must be greater than or equal to zero.");

        RuleFor(command => command.CurrencyCode)
        .NotEmpty()
        .WithErrorCode(ErrorCodes.Validation.INVALID_CURRENCY_CODE)
        .WithMessage("A currency code is required.")
        .When(command => !string.IsNullOrWhiteSpace(command.CurrencyCode))
        .Must(currencyCode => currencyCode.Trim().Length == 3)
        .WithErrorCode(ErrorCodes.Validation.INVALID_CURRENCY_CODE)
        .WithMessage("Currency code must be exactly three characters.");
    }
}