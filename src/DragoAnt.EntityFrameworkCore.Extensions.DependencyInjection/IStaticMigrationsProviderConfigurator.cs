using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.Extensions.DependencyInjection;

public interface IStaticMigrationsProviderConfigurator
{
    void RegisterServices(IServiceCollection services, StaticMigrationsOptions options);
}