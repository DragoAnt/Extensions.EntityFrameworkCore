using System.Security.Cryptography;
using System.Text.Json;

namespace DragoAnt.Extensions.StaticMigrations;

public abstract class StaticMigration : IStaticMigration
{
    public static readonly HashAlgorithm HashAlgorithm = SHA256.Create();
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
        return HashAlgorithm.ComputeHash(itemsArray);
    }
}