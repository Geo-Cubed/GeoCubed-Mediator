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
    /// <param name="assemblyToUse">The assembly to check</param>
    /// <returns>The service collection passed in <paramref name="services"/>.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly? assemblyToUse = null)
    {
        // Add mediator service if it doesn't already exist.
        services.TryAddTransient<IMediator, Mediator>();

        // Add the assembly to the list.
        assemblyToUse ??= Assembly.GetCallingAssembly();
        if (!_usedAssemblies.Contains(assemblyToUse))
        {
            _usedAssemblies.Add(assemblyToUse);

            var requestHandlerType = typeof(IRequestHandler<IRequest<string>, string>);
            var types = MediatorHelper.GetImplementingTypes(assemblyToUse);
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
