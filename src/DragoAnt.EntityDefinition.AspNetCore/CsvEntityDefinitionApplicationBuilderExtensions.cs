using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using DragoAnt.EntityDefinition.EntityFrameworkCore;

namespace DragoAnt.EntityDefinition.AspNetCore;

/// <summary>
/// Provides extension methods for <see cref="IApplicationBuilder"/> to add csv routing documentation.
/// </summary>
public static class CsvEntityDefinitionApplicationBuilderExtensions
{
    /// <summary>
    /// Use <see cref="CsvEFEntityDefinitionMiddleware{TDbContext}"/> entity definition middleware using the given route pattern.
    /// For example, if the given route pattern is "myrouteinfo", then you can send request "~/myrouteinfo" after enabling this middleware.
    /// Please use basic (literal) route pattern with '.csv' at the end. 
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder "/> to use.</param>
    /// <param name="routePattern">The given route pattern.</param>
    /// <param name="init"></param>
    /// <returns>The <see cref="IApplicationBuilder "/>.</returns>
    public static IApplicationBuilder UseCsvEntityDefinition<TDbContext>(this IApplicationBuilder app, string routePattern = "entity-definition.csv",
        Action<ICsvEntityFrameworkCoreDefinitionOptions>? init = null)
        where TDbContext : DbContext
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (routePattern == null)
        {
            throw new ArgumentNullException(nameof(routePattern));
        }

        init ??= opt =>
        {
            opt.AddEntityColumn(EFCommonDefinitions.Entities.Name);
            opt.AddPropertyColumn(EFCommonDefinitions.Properties.Name);

        };

        var options = new CsvEntityFrameworkCoreDefinitionOptions();
        init(options);

        return app.UseMiddleware<CsvEFEntityDefinitionMiddleware<TDbContext>>(routePattern, options);
    }
}