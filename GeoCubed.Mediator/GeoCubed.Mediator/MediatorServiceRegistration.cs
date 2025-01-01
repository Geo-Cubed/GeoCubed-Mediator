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
    private static List<Assembly> _usedAssemblies = new ();

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
                        
            var assemblyTypes = assemblyToUse.GetTypes();
            for (int i = 0; i < assemblyTypes.Length; ++i)
            {
                var handler = assemblyTypes[i];

                // TODO: Need to create the request handler type for the specific request / response ??? not really sure.
                var genericHanlder = handler.GetInterface(MediatorHelper.CreateRequestHandlerType().Name);
                if (genericHanlder != null)
                {
                    services.AddScoped(genericHanlder, handler);
                }
            }
        }

        return services;
    }
}
