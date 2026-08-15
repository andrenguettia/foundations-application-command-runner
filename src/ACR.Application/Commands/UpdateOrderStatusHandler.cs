using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ACR.Application.Common;
using ACR.Domain;
using ACR.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ACR.Application.Commands;

public sealed class UpdateOrderStatusHandler
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly IValidator<UpdateOrderStatusCommand> _validator;
    private readonly ILogger<UpdateOrderStatusHandler> _logger;

    public UpdateOrderStatusHandler(
        IOrderRepository repository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IValidator<UpdateOrderStatusCommand> validator,
        ILogger<UpdateOrderStatusHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;    
        _timeProvider = timeProvider;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Order>> ExecuteAsync(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(command, cancellationToken);
        if(!validation.IsValid)
        {
            var result = validation.Errors.First();
            return Result<Order>.Fail(result.ErrorCode, result.ErrorMessage);
        }

        var order = await _repository.GetByIdAsync(command.OrderId, cancellationToken);
        if(order is null)
        {
            return Result<Order>.Fail(ErrorCodes.Order.ORDER_NOT_FOUND, $"Order with ID '{command.OrderId}' not found.");
        }

        var targetStatus = Enum.Parse<OrderStatus>(command.OrderStatus, ignoreCase: true);

        Order updatedOrder;
        try
        {
            updatedOrder = order.UpdateStatus(targetStatus, _timeProvider.GetUtcNow().UtcDateTime);
        }
        catch (InvalidOrderTransitionException ex)
        {
            return Result<Order>.Fail(ErrorCodes.Order.INVALID_TARGET_STATUS, ex.Message);
        }

        try
        {
            await _repository.UpdateAsync(updatedOrder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Failed to persist status update for order {OrderId}", command.OrderId);
            
            return Result<Order>.Fail(
                ErrorCodes.Database.ORDER_SAVE_FAILED,
                "Unable to update the order status. Try again later");
        }

        return Result<Order>.Success(updatedOrder);
    }
}