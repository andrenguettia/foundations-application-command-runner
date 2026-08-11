using System;
using CommandLine;

namespace ACR.Cli;

public class CommandLineOptions
{
    [Option("customer-id", Required = true, SetName = "CreateOrder", HelpText = "Assigns a customer ID to the order.")]
    public string CustomerId { get; set; }

    [Option("total-amount", Required = true, SetName = "CreateOrder", HelpText = "Specifies the total amount for the order.")]
    public decimal TotalAmount { get; set; }

    [Option("currency-code", Required = true, SetName = "CreateOrder", HelpText = "Specifies the currency code for the order.")]
    public string CurrencyCode { get; set; }

    [Option("external-reference", Required = false, HelpText = "Specifies the external reference associated with the order.")]
    public string ExternalReference { get; set; }

    [Option("create", Required = true, SetName = "CreateOrder", HelpText = "Creates a new order.")]
    public bool CreateOrder { get; set; }

    [Option("order-id", Required = true, SetName = "UpdateOrderStatus", HelpText = "The order to lookup.")]
    public Guid OrderId { get; set; }

    [Option("status", Required = true, SetName = "UpdateOrderStatus", HelpText = "The new order status.")]
    public string OrderStatus { get; set; }

    [Option("update", Required = true, SetName = "UpdateOrderStatus", HelpText = "Updates the order status.")]
    public bool UpdateOrder { get; set; }
}