using GeoCubed.Mediator.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace GeoCubed.Mediator;

/// <summary>
/// Static class used to register the mediator services.
/// </summary>
public static class MediatorServiceRegistration
{
    private static List<Assembly> _usedAssemblies;

    static MediatorServiceRegistration()
    {
        _usedAssemblies = new();
    }

    /// <summary>
    /// Registers the mediator services and adds the current assembly to the mediator.
    /// </summary>
    /// <param name="services">The service collection to use.</param>
    /// <returns>The service collection passed in <paramref name="services"/>.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        // Add mediator service if it doesn't already exist.
        services.TryAddTransient<IMediator, Mediator>();

        // Add the assembly to the list.
        var assembly = Assembly.GetCallingAssembly();
        if (!_usedAssemblies.Contains(assembly))
        {
            _usedAssemblies.Add(assembly);

            var requestHandlerType = typeof(IRequestHandler<IRequest<string>, string>);
            var types = MediatorHelper.GetImplementingTypes(assembly);
            foreach (var type in types)
            {
                // Add the request handlers to the service container.
                var interfaceType = type.GetInterface(requestHandlerType.Name);
                if (interfaceType != null)
                {
                    services.TryAddScoped(interfaceType, type);
                }
            }
        }

        return services;
    }
}
