using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ACR.Application.Common;
using ACR.Domain;
using ACR.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ACR.Application.Commands;

public sealed class CreateOrderHandler
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly IValidator<CreateOrderCommand> _validator;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(
        IOrderRepository repository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IValidator<CreateOrderCommand> validator,
        ILogger<CreateOrderHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Order>> ExecuteAsync(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command, cancellationToken);
        if(!validation.IsValid)
        {
            var result = validation.Errors.First();
            return Result<Order>.Fail(result.ErrorCode, result.ErrorMessage);
        }

        if(!string.IsNullOrWhiteSpace(command.ExternalReference))
        {
            return await GetByExternalReferenceAsync(command, cancellationToken);
        }

        Order order;
        try
        {
            var customerId = CustomerId.Create(command.CustomerId);
            var currency = Currency.Create(command.TotalAmount, command.CurrencyCode);
            order = Order.Create(customerId, currency, _timeProvider.GetUtcNow().UtcDateTime);
        }
        catch (NegativeCurrencyAmountException ex)
        {
            return Result<Order>.Fail(ErrorCodes.Validation.INVALID_AMOUNT, ex.Message);
        }
        catch (UnsupportedCurrencyException ex)
        {
            return Result<Order>.Fail(ErrorCodes.Order.CURRENCY_CODE_NOT_SUPPORTED, ex.Message);
        }
        catch(InvalidCustomerIdException ex)
        {
            return Result<Order>.Fail(ErrorCodes.Validation.INVALID_CUSTOMER_ID, ex.Message);
        }

        try
        {
            await _repository.StageAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch(Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Failed to persist a new order for customer {CustomerId}", command.CustomerId);
            return Result<Order>.Fail(ErrorCodes.Database.ORDER_SAVE_FAILED, "The order could not be saved. Please try again later.");
        }

        return Result<Order>.Success(order);
    }

    private async Task<Result<Order>> GetByExternalReferenceAsync(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var externalReference = ExternalReference.Create(command.ExternalReference);
        var order = await _repository.GetByExternalReferenceAsync(externalReference, cancellationToken);

        var isCustomerMatch = order is not null && string.Equals(order.CustomerId.Value, command.CustomerId, StringComparison.InvariantCultureIgnoreCase);

        if(!isCustomerMatch)
        {
            return Result<Order>.Fail(
                ErrorCodes.Order.INVALID_EXTERNAL_REFERENCE,
                $"Unable to find reference '{command.ExternalReference}' for customer ID '{command.CustomerId}'");
        }

        return Result<Order>.Success(order);
    }
}