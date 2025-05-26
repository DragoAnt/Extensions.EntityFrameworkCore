using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.EntityConventions;

public sealed class EntityConventionsOptionsExtension : IDbContextOptionsExtension
{
    private ExtensionInfo? _info;
    private IEntityConventionsService? _entityConventionsService;
    private readonly IEntityConventionsProviderConfigurator _configurator;
    private readonly EntityConventionsOptions _options;
    private readonly Action<IEntityConventionsBuilder>? _init;

    public EntityConventionsOptionsExtension(IEntityConventionsProviderConfigurator configurator, EntityConventionsOptions options,
        Action<IEntityConventionsBuilder>? init)
    {
        _configurator = configurator;
        _options = options ?? throw new ArgumentException(nameof(options));
        _init = init;
    }

    private static EntityConventionsOptionsExtension GetExtension(IServiceProvider provider)
    {
#pragma warning disable EF1001
        return provider.GetRequiredService<IDbContextServices>().ContextOptions
                   .FindExtension<EntityConventionsOptionsExtension>() ??
               throw new Exception("Can't find EF extension: EntityConventionsOptionsExtension");
#pragma warning restore EF1001
    }

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
        _configurator.RegisterServices(services, _options);
        services.AddScoped(provider => GetExtension(provider).GetEntityConventionsService());
    }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options)
    {
    }

    /// <inheritdoc />
    public DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this);


    private IEntityConventionsService GetEntityConventionsService()
    {
        if (_entityConventionsService is { } service)
        {
            return service;
        }

        var builder = new EntityConventionsBuilder(_options.Defaults);
        if (_options.IncludeCommonConventions)
        {
            builder.AddCommonConventions();
        }
        _init?.Invoke(builder);

        return _entityConventionsService = builder;
    }

    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        private string? _logFragment;

        public ExtensionInfo(EntityConventionsOptionsExtension extension)
            : base(extension)
        {
        }

        private new EntityConventionsOptionsExtension Extension => (EntityConventionsOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider => false;

        public override string LogFragment
        {
            get
            {
                if (_logFragment != null)
                {
                    return _logFragment;
                }
                var builder = new StringBuilder();

                builder.Append($"IncludeCommonConventions={Extension._options.IncludeCommonConventions}");

                _logFragment = builder.ToString();

                return _logFragment;
            }
        }
#if NET6_0_OR_GREATER
        /// <inheritdoc />
        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is ExtensionInfo otherInfo &&
                   otherInfo.GetServiceProviderHashCode() == GetServiceProviderHashCode();
        }
#endif

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            if (debugInfo == null)
            {
                throw new ArgumentNullException(nameof(debugInfo));
            }

            debugInfo["Entity Conventions: IncludeCommonConventions"] = Extension._options.IncludeCommonConventions.ToString();
        }

        public override int GetServiceProviderHashCode()
        {
            return 0;
        }
    }
}