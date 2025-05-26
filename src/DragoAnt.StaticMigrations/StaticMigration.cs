using System.Security.Cryptography;
using System.Text.Json;

namespace DragoAnt.StaticMigrations;

public abstract class StaticMigration : IStaticMigration
{
    protected static HashAlgorithm GetHashAlgorithm() => SHA256.Create();
    private byte[]? _hash;

    /// <inheritdoc />
    public virtual byte[] GetHash() => _hash ??= GetHashInternal();

    protected abstract byte[] GetHashInternal();

    /// <summary>
    /// Gets default hash by using <see cref="System.Text.Json.JsonSerializer"/> and HashAlgorithm
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    protected static byte[] GetHash<T>(T items)
    {
        var itemsArray = JsonSerializer.SerializeToUtf8Bytes(items);
        return GetHashAlgorithm().ComputeHash(itemsArray);
    }
}