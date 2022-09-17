namespace Infrastructure.Constants
{
    internal static class InfrastructureConstants
    {
        internal static class ErrorMessages
        {
            internal const string QUERY_NOT_ORDERED = "Query must me sorted before pagination";

            internal const string UNKNOWN_ERROR = "Something went wrong!";

            internal const string VALIDATION_FAILED = "Validation failed";
        }

        internal static class Pagination
        {
            internal const string COUNT_FACET_NAME = "COUNT_FACET";

            internal const string DATA_FACET_NAME = "DATA_FACET";
        }

        internal static class ExceptionHandling
        {
            internal const string EXCEPTION_OCCURRED_TEMPLATE = "Error occurred, ";
        }

        internal static class Migration
        {
            internal const string CONFIGURE_METHOD_NAME = "Configure";
        }

        internal static class Convention
        {
            internal const string DEFAULT_CONVENTIONS_PACK_NAME = "GLOBAL_CONVENTION";
        }
    }
}
