using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

public interface IEntityConventionsProviderConfigurator
{
    void RegisterServices(IServiceCollection services, EntityConventionsOptions options);
}