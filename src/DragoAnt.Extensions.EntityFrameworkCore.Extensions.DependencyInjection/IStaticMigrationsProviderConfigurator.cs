using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;

public interface IStaticMigrationsProviderConfigurator
{
    void RegisterServices(IServiceCollection services, StaticMigrationsOptions options);
}