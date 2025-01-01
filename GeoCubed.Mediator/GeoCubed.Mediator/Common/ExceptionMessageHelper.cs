namespace GeoCubed.Mediator.Common;

internal static class ExceptionMessageHelper
{
    private const string MULTIPLE_HANDLER_EXCEPTION = "There are multiple of the same handler";
    private const string HANDLER_NOT_FOUND = "There was no handler found";

    internal static string MultipleHandlerMsg => MULTIPLE_HANDLER_EXCEPTION;
    internal static string NoHandlerMsg => HANDLER_NOT_FOUND;
}
