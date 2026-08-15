namespace ACR.Application.Common;

public static class ErrorCodes
{
    public static class Validation
    {
        private const string VALIDATION_PREFIX = "validation.";

        public const string CUSTOMER_ID_REQUIRED = $"{VALIDATION_PREFIX}customer_id_required";
        public const string ORDER_ID_REQUIRED = $"{VALIDATION_PREFIX}order_id_required";
        public const string INVALID_AMOUNT = $"{VALIDATION_PREFIX}invalid_amount";
        public const string INVALID_CURRENCY_CODE = $"{VALIDATION_PREFIX}invalid_currency_code";
        public const string INVALID_ORDER_STATUS = $"{VALIDATION_PREFIX}invalid_order_status";
        public const string INVALID_CUSTOMER_ID = $"{VALIDATION_PREFIX}invalid_customer_id";
    }

    public static class Order
    {
        private const string ORDER_PREFIX = "order.";

        public const string CURRENCY_CODE_NOT_SUPPORTED = $"{ORDER_PREFIX}currency_code_not_supported";
        public const string ORDER_NOT_FOUND = $"{ORDER_PREFIX}order_not_found";
        public const string INVALID_TARGET_STATUS = $"{ORDER_PREFIX}status_update_failed";
        public const string INVALID_EXTERNAL_REFERENCE = $"{ORDER_PREFIX}incorrect_external_reference";
    }

    public static class Database
    {
        private const string DATABASE_PREFIX = "database.";

        public const string ORDER_SAVE_FAILED = $"{DATABASE_PREFIX}order_save_failed";
    }
}