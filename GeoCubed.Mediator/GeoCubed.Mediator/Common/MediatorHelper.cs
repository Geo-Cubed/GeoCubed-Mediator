using System.Reflection;

namespace GeoCubed.Mediator.Common;

internal static class MediatorHelper
{
    internal static Type CreateRequestHandlerType()
    {
        return typeof(IRequestHandler<,>);
    }

    internal static Type CreateRequestHandlerType(Type request, Type response)
    {
        return CreateRequestHandlerType().MakeGenericType(request, response);
    }

    internal static async Task<object> InvokeAsync(this MethodInfo methodToInvoke, object obj, params object[] parameters)
    {
        dynamic awaitable = methodToInvoke.Invoke(obj, parameters) ?? throw new ArgumentException(ERR_INVOKE(methodToInvoke));
        await awaitable;
        return awaitable.GetAwaiter().GetResult();
    }

    internal static string ERR_NO_HANDLER(Type request) 
        => string.Format("There was no handler found for the request {0}.", request.Name);

    internal static string ERR_NO_METHOD(string methodName, Type handler) 
        => string.Format("Cannot find the method '{0}' on the handler {1}.", methodName, handler.Name);

    internal static string ERR_EXCEPTION_ON_HANDLER(string methodName, Type handler)
        => string.Format("An exception occured while trying to run the '{0}' method on the handler {1} see inner exception for more details", methodName, handler.Name);

    internal static string ERR_INVOKE(MethodInfo method)
        => string.Format("There was an error invoking the method '{0}'.", method.Name);
}
