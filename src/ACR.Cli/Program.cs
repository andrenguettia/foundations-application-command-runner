using System;
using System.Threading;
using System.Threading.Tasks;
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
    public static async Task<int> Main(string[] args)
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
        services.AddSingleton(TimeProvider.System);

        var serviceProvider = services.BuildServiceProvider();

        var createOrderHandler = serviceProvider.GetRequiredService<CreateOrderHandler>();
        var updateOrderStatusHandler = serviceProvider.GetRequiredService<UpdateOrderStatusHandler>();
        var store = serviceProvider.GetRequiredService<InMemoryOrderStore>();

        var parser = Parser.Default.ParseArguments<CommandLineOptions>(args);
        return await parser.MapResult(async option => 
        {
            try
            {
                if(option.CreateOrder)
                {
                    return await ExecuteCreateOrder(option, createOrderHandler);
                }
                else if(option.UpdateOrder)
                {
                    return await ExecuteUpdateOrderStatus(option, updateOrderStatusHandler);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ExitCodes.ERROR;
            }

            return ExitCodes.SUCCESS;
        },
        errors => Task.FromResult(ExitCodes.ERROR));
    }

    private static async Task<int> ExecuteCreateOrder(CommandLineOptions commandLineOptions, CreateOrderHandler createOrderHandler)
    {
        Console.WriteLine("Creating order...");

        var command = new CreateOrderCommand(
            commandLineOptions.CustomerId,
            commandLineOptions.TotalAmount,
            commandLineOptions.CurrencyCode,
            commandLineOptions.ExternalReference);

        var result = await createOrderHandler.ExecuteAsync(command, CancellationToken.None);

        if (result.HasError)
        {
            Console.WriteLine(result.Error.Message);
            return ExitCodes.ERROR;
        }

        Console.WriteLine(result.Value.ToString());
        return ExitCodes.SUCCESS;
    }

    private static async Task<int> ExecuteUpdateOrderStatus(CommandLineOptions commandLineOptions, UpdateOrderStatusHandler updateOrderStatusHandler)
    {
        Console.WriteLine("Updating order...");

        var command = new UpdateOrderStatusCommand(
            commandLineOptions.OrderId,
            commandLineOptions.OrderStatus);

        var result = await updateOrderStatusHandler.ExecuteAsync(command, CancellationToken.None);

        if (result.HasError)
        {
            Console.WriteLine(result.Error.Message);
            return ExitCodes.ERROR;
        }

        Console.WriteLine(result.Value.ToString());
        return ExitCodes.SUCCESS;
    }
}