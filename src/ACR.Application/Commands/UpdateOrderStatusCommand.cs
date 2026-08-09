using System;

namespace ACR.Application.Commands;

public sealed record UpdateOrderStatusCommand(
    Guid OrderId,
    string OrderStatus);