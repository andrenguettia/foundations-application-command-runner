using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ACR.Application.Commands;
using ACR.Application.Validation;
using ACR.Domain;
using ACR.Infrastructure.InMemory;
using CommandLine;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ACR.Cli;

public class Program
{
    public static int Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .Build();

        var services = new ServiceCollection();

        services.AddLogging(builder => 
            builder.AddConsole()
            .SetMinimumLevel(LogLevel.Warning));

        services.AddSingleton<InMemoryOrderStore>();
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddSingleton<IUnitOfWork, InMemoryUnitOfWork>();

        services.AddSingleton<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
        services.AddSingleton<IValidator<UpdateOrderStatusCommand>, UpdateOrderStatusCommandValidator>();

        services.AddSingleton<CreateOrderHandler>();
        services.AddSingleton<UpdateOrderStatusHandler>();

        var serviceProvider = services.BuildServiceProvider();

        var createOrderHandler = serviceProvider.GetRequiredService<CreateOrderHandler>();
        var updateOrderHandler = serviceProvider.GetRequiredService<UpdateOrderStatusHandler>();
        var store = serviceProvider.GetRequiredService<InMemoryOrderStore>();

        try
        {
            var parser = Parser.Default.ParseArguments<CommandLineOptions>(args);
            parser.WithParsedAsync(async (option) => 
            {
               if(option.CreateOrder)
                {
                    Environment.ExitCode = await ExecuteCreateOrder(option, createOrderHandler);
                }
            });

            return ExitCodes.SUCCESS;
        }
        catch (CommandLineParseException ex)
        {
            Console.WriteLine($"Usage Error: {ex.Message}");
            return ExitCodes.USAGE_ERROR;
        }
    }

    private static async Task<int> ExecuteCreateOrder(CommandLineOptions commandLineOptions, CreateOrderHandler createOrderHandler)
    {
        var command = new CreateOrderCommand(
            commandLineOptions.CustomerId,
            commandLineOptions.TotalAmount,
            commandLineOptions.CurrencyCode,
            commandLineOptions.ExternalReference);

        var result = await createOrderHandler.ExecuteAsync(command, CancellationToken.None);

        if (result.HasError)
        {
            Console.WriteLine(result.Error.Message);
            return ExitCodes.USAGE_ERROR;
        }

        Console.WriteLine(result.Value.ToString());
        return ExitCodes.SUCCESS;
    }
}