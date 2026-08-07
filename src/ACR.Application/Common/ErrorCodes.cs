namespace ACR.Domain.Common;

public static class ErrorCodes
{
    public static class Validation
    {
        private const string VALIDATION_PREFIX = "Error: ";

        public const string CustomerIdRequired = $"{VALIDATION_PREFIX}Customer ID required";
        public const string OrderIdRequired = $"{VALIDATION_PREFIX}Order ID required";
        public const string InvalidAmount = $"{VALIDATION_PREFIX}Invalid amount specified";
        public const string InvalidCurrency = $"{VALIDATION_PREFIX}Invalid currency code specified";
        public const string InvalidStatus = $"{VALIDATION_PREFIX}Invalid order status specified";
    }

    public static class Order
    {
        private const string ORDER_PREFIX = "Order Error: ";
        public const string INVALID_CURRENCY_CODE = $"{ORDER_PREFIX}Currency code not supported";
        public const string ORDER_NOT_FOUND = $"{ORDER_PREFIX}Order not found";
        public const string INVALID_TARGET_STATUS = $"{ORDER_PREFIX}Unable to update the order status";
        public const string INVALID_EXTERNAL_REFERENCE = $"{ORDER_PREFIX}Incorrect external reference specified";
    }

    public static class Database
    {
        private const string DATABASE_PREFIX = "Database Error: ";

        public const string OrderSaveFailed = $"{DATABASE_PREFIX}Unable to save the order";
    }
}