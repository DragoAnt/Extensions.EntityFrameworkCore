using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.Extensions.EntityFrameworkCore.EntityConventions;

public interface IEntityConventionsProviderConfigurator
{
    void RegisterServices(IServiceCollection services, EntityConventionsOptions options);
}