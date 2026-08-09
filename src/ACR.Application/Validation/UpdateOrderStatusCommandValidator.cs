using System;
using ACR.Application.Commands;
using ACR.Domain;
using ACR.Domain.Common;
using FluentValidation;

namespace ACR.Application.Validation;

public sealed class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(command => command.OrderId)
            .NotEqual(Guid.Empty)
            .WithErrorCode(ErrorCodes.Validation.ORDER_ID_REQUIRED)
            .WithMessage("Order ID is required.");

            RuleFor(command => command.OrderStatus)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Validation.INVALID_ORDER_STATUS)
            .WithMessage("Order status is required")
            .When(command => !string.IsNullOrWhiteSpace(command.OrderStatus))
            .Must(orderStatus => Enum.TryParse<OrderStatus>(orderStatus, ignoreCase: true, out var status))
            .WithErrorCode(ErrorCodes.Validation.INVALID_ORDER_STATUS)
            .WithMessage(command => $"Invalid order status '{command.OrderStatus}'. Expected {string.Join(", ", Enum.GetNames<OrderStatus>())}");
    }
}