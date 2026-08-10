using System;

namespace ACR.Cli;

public sealed class CommandLineParseException : Exception
{
    public CommandLineParseException(string message)
        : base(message)
    {
    }
}