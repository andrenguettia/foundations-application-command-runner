using System;

namespace ACR.Application.Abstractions;

public interface IClock
{
    DateTime UtcNow { get; }
}