using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GeoCubed.Mediator.Test;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediator(Assembly.GetExecutingAssembly());
        services.AddScoped<InjectedService>();
    }
}
