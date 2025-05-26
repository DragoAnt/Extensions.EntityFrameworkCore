using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DragoAnt.EntityFrameworkCore.EntityConventions.SqlServer.Extensions.DependencyInjection;

public sealed class SqlServerEntityConventionsProvider : RelationalDbContextOptionsConfigurator, IEntityConventionsProviderConfigurator
{
    /// <inheritdoc />
    public void RegisterServices(IServiceCollection services, EntityConventionsOptions options)
    {
        services.TryAddSingleton<IEntityConventionsProviderService, EntityConventionsProviderServiceSqlServer>();
    }
}