using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DragoAnt.Extensions.EntityDefinition.EntityFrameworkCore;
using DragoAnt.Shared.AspNetCore;

namespace DragoAnt.Extensions.EntityDefinition.AspNetCore;

public class CsvEFEntityDefinitionMiddleware<TDbContext> : CsvMiddlewareBase
    where TDbContext : DbContext
{
    private readonly CsvEntityFrameworkCoreDefinitionOptions _options;

    /// <inheritdoc />
    public CsvEFEntityDefinitionMiddleware(string routePattern, RequestDelegate next, CsvEntityFrameworkCoreDefinitionOptions options)
        : base(routePattern, next)
    {
        _options = options;
    }

    protected override string GenerateCsv(HttpContext context)
    {
        var dbContext = context.RequestServices.GetRequiredService<TDbContext>();
        return dbContext.Model.GenerateCsv(_options, _options.Delimiter);
    }
}