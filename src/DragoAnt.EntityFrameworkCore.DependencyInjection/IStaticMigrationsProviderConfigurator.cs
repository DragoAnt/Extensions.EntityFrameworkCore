using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.DependencyInjection;

public interface IStaticMigrationsProviderConfigurator
{
    void RegisterServices(IServiceCollection services, StaticMigrationsOptions options);
}