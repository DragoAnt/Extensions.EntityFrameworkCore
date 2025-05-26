using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DragoAnt.EntityFrameworkCore.HistoricalMigrations;

public sealed class HistoricalMigrationsOptionsExtension : IDbContextOptionsExtension
{
    private ExtensionInfo? _info;
    public HistoricalMigrationsOptions Options { get; }

    public HistoricalMigrationsOptionsExtension(HistoricalMigrationsOptions options)
    {
        Options = options ?? throw new ArgumentException(nameof(options));
    }

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton(Options);
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

        public ExtensionInfo(HistoricalMigrationsOptionsExtension extension)
            : base(extension)
        {
        }

        private new HistoricalMigrationsOptionsExtension Extension => (HistoricalMigrationsOptionsExtension)base.Extension;

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

                builder.Append($"MigrateFromFullHistory={Extension.Options.MigrateFromFullHistory}");

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

            debugInfo["Historical Migrations: MigrateFromFullHistory"] = Extension.Options.MigrateFromFullHistory.ToString();
        }
#if NET6_0_OR_GREATER
        public override int GetServiceProviderHashCode()
        {
            if (_serviceProviderHash != null)
            {
                return _serviceProviderHash.Value;
            }
            var hashCode = Extension.Options.GetHashCode();
            _serviceProviderHash = hashCode;

            return _serviceProviderHash.Value;
        }
#else
            public override long GetServiceProviderHashCode()
            {
                if (_serviceProviderHash != null)
                {
                    return _serviceProviderHash.Value;
                }
                var hashCode = Extension.Options.GetHashCode();
                _serviceProviderHash = hashCode;

                return _serviceProviderHash.Value;
            }
#endif
    }
}