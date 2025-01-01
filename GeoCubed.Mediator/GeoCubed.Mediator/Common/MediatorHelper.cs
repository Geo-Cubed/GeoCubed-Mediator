using System.Reflection;

namespace GeoCubed.Mediator.Common;

internal static class MediatorHelper
{
    private static List<Type> _implementingTypes;
    private static string _requestHandlerName;

    static MediatorHelper()
    {
        _implementingTypes = new();
        _requestHandlerName = typeof(IRequestHandler<IRequest<string>, string>).Name;
    } 

    /// <summary>
    /// Gets the implementing types in the assembly.
    /// </summary>
    /// <param name="assembly">The assembly to check.</param>
    /// <returns>A list of types that implement the mediator interface.</returns>
    internal static List<Type> GetImplementingTypes(Assembly assembly)
    {
        var mediatorType = typeof(IRequestHandler<,>);
        var assemblyImplementingTypes =
            from assemblyType in assembly.GetTypes()
            from typeInterfaces in assemblyType.GetInterfaces()
            let baseType = assemblyType.BaseType
            where
                (baseType != null && baseType.IsGenericType &&
                mediatorType.IsAssignableFrom(baseType.GetGenericTypeDefinition())) ||
                (typeInterfaces.IsGenericType &&
                mediatorType.IsAssignableFrom(typeInterfaces.GetGenericTypeDefinition()))
            select assemblyType;

        _implementingTypes.AddRange(assemblyImplementingTypes);
        return assemblyImplementingTypes.ToList();
    }

    /// <summary>
    /// Gets the type of the handler.
    /// </summary>
    /// <param name="handlerType">Type of the request.</param>
    /// <returns>The handler type.</returns>
    /// <exception cref="HandlerNotFoundException">Cannot find the handler type.</exception>
    /// <exception cref="MultipleHandlerException">More than 1 handler for the requst.</exception>
    internal static Type GetHandlerType(Type handlerType, Type requestType)
    {
        var handler = _implementingTypes
            .Where(x => x.GetInterfaces()
                .Any(y =>
                    y.IsAssignableFrom(handlerType)
                    && y.GenericTypeArguments.Length == 2
                    && y.GenericTypeArguments.Contains(handlerType.GenericTypeArguments[1])
                    && y.GenericTypeArguments.Contains(requestType)))
            .ToList();

        if (handler == null || handler.Count == 0)
        {
            throw new HandlerNotFoundException(handlerType.FullName
                ?? ExceptionMessageHelper.NoHandlerMsg);
        }

        if (handler.Count > 1)
        {
            throw new MultipleHandlerException(ExceptionMessageHelper.MultipleHandlerMsg);
        }

        return handler.First();
    }

    /// <summary>
    /// Invokes a method on an object with the perameters.
    /// </summary>
    /// <param name="methodToInvoke">Method that is being called.</param>
    /// <param name="obj">The class the method belongs to.</param>
    /// <param name="parameters">The parameters for the method.</param>
    /// <returns>The response from the method.</returns>
    /// <exception cref="ArgumentException">If the method does not exist in the object.</exception>
    internal static async Task<object> InvokeAsync(this MethodInfo methodToInvoke, object obj, params object[] parameters)
    {
        dynamic awaitable = methodToInvoke.Invoke(obj, parameters) 
            ?? throw new ArgumentException(nameof(obj));
        await awaitable;
        return awaitable.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Instantiates a class using the service provider.
    /// </summary>
    /// <param name="typeToCreate">The type to instantiate.</param>
    /// <param name="provider">The service provider to use.</param>
    /// <returns>An instantatied type based on the service provider.</returns>
    internal static object? InstantiateType(this Type typeToCreate, IServiceProvider provider)
    {
        var interfaceOfType = typeToCreate.GetInterface(_requestHandlerName);
        if (interfaceOfType == null)
        {
            return null;
        }

        return provider.GetService(interfaceOfType);
    }
}
