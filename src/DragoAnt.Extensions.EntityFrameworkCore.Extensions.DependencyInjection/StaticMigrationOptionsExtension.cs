using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations;
using DragoAnt.Extensions.EntityFrameworkCore.StaticMigrations.StaticMigrations;
using DragoAnt.Extensions.StaticMigrations;

namespace DragoAnt.Extensions.EntityFrameworkCore.Extensions.DependencyInjection;

internal sealed class StaticMigrationOptionsExtension : IDbContextOptionsExtension
{
    private ExtensionInfo? _info;
    private readonly IStaticMigrationsProviderConfigurator _configurator;
    private readonly StaticMigrationsOptions _options;
    private readonly Action<StaticMigrationBuilder>? _initMigrations;

    private StaticMigrationBuilder? _staticMigrationBuilder;


    public StaticMigrationOptionsExtension(IStaticMigrationsProviderConfigurator configurator,
        StaticMigrationsOptions options, Action<StaticMigrationBuilder>? initMigrations)
    {
        _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _initMigrations = initMigrations;
    }

    private static StaticMigrationOptionsExtension GetExtension(IServiceProvider provider)
    {
#pragma warning disable EF1001
        var extension = provider.GetRequiredService<IDbContextServices>().ContextOptions.FindExtension<StaticMigrationOptionsExtension>();
#pragma warning restore EF1001

        if (extension == null)
        {
            throw new InvalidOperationException("Can't find StaticMigrationOptionsExtension. Register it first with HasStaticMigration... extensions");
        }
        return extension;
    }

    private StaticMigrationBuilder GetStaticMigrationsBuilder()
    {
        if (_staticMigrationBuilder is { } b)
        {
            return b;
        }

        var builder = new StaticMigrationBuilder();

        if (_options.EnableEnumTables)
        {
            builder.AddEnumTables();
        }
        _initMigrations?.Invoke(builder);

        return _staticMigrationBuilder = builder;
    }

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
        _configurator.RegisterServices(services, _options);

        services.TryAddScoped<IStaticMigrationsService, StaticMigrationsService>();

        services.AddScoped<IStaticMigrationCollection<IStaticSqlMigration, DbContext>>(
            provider => GetExtension(provider).GetStaticMigrationsBuilder().SQLMigrations);

        services.TryAddScoped<IStaticMigrationsService, StaticMigrationsService>();

        var relationalOverrided = OverrideService<IRelationalDatabaseCreator>(services, (provider, creator) =>
            new RelationalDatabaseCreatorWithStaticMigrations(creator,
                provider,
                provider.GetRequiredService<RelationalDatabaseCreatorDependencies>(),
                provider.GetRequiredService<IRelationalConnection>(),
                provider.GetRequiredService<IMigrationCommandExecutor>(),
                provider.GetRequiredService<IMigrationsSqlGenerator>()));

        if (relationalOverrided)
        {
            return;
        }

        services.TryAddScoped<IStaticMigrationHistoryRepository, EmptyStaticMigrationHistoryRepository>();
        var overrided = OverrideService<IDatabaseCreator>(services, (provider, creator) =>
            new DatabaseCreatorWithStaticMigrations(creator, provider.GetRequiredService<IStaticMigrationsService>()));

        if (!overrided)
        {
            throw new NotSupportedException("Can't use static migrations with this database provider. Can;t find service 'IDatabaseCreator'");
        }
    }

    private static bool OverrideService<T>(IServiceCollection services, Func<IServiceProvider, T, T> factory)
        where T : notnull
    {
        var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(T));
        if (descriptor == null)
        {
            return false;
        }
        if (descriptor.ImplementationType != null)
        {
            services.Add(new ServiceDescriptor(descriptor.ImplementationType, descriptor.ImplementationType, descriptor.Lifetime));
        }
        services.Replace(new ServiceDescriptor(typeof(T), provider =>
        {
            T baseService;
            if (descriptor.ImplementationInstance != null)
            {
                baseService = (T)descriptor.ImplementationInstance;
            }
            else if (descriptor.ImplementationType != null)
            {
                baseService = (T)provider.GetRequiredService(descriptor.ImplementationType);
            }
            else if (descriptor.ImplementationFactory != null)
            {
                baseService = (T)descriptor.ImplementationFactory.Invoke(provider);
            }
            else
            {
                throw new NotSupportedException();
            }

            return factory(provider, baseService);
        }, descriptor.Lifetime));

        return true;
    }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options)
    {
    }

    /// <inheritdoc />
    public DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this);


    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        private int? _serviceProviderHash;
        private string? _logFragment;

        public ExtensionInfo(StaticMigrationOptionsExtension extension)
            : base(extension)
        {
        }

        private new StaticMigrationOptionsExtension Extension => (StaticMigrationOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider => false;

        public override string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();

                    builder.Append($"Configurator={Extension._configurator.GetType().Name} ");

                    _logFragment = builder.ToString();
                }

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

            debugInfo["Static Migrations: Configurator"] = Extension._configurator.GetType().Name;
        }
        public override int GetServiceProviderHashCode()
        {
            if (_serviceProviderHash != null)
            {
                return _serviceProviderHash.Value;
            }
            var hashCode = Extension._configurator.GetType().GetHashCode();
            _serviceProviderHash = hashCode;

            return _serviceProviderHash.Value;
        }
    }
}