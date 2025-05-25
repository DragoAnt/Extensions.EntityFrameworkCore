namespace DragoAnt.Extensions.EntityConventions.Contacts;

public sealed class EntityConventionsCommonDefaultsOptions
{
    /// <summary>
    /// Default options for <see cref="IWithSourceSystemIdEntityConvention"/> entity convention
    /// </summary>
    public SourceSystemIdOptions SourceSystemId { get; } = new();
    /// <summary>
    /// Default options for <see cref="IWithDiscriminatorEntityConvention{T}"/> entity convention
    /// </summary>
    public DiscriminatorOptions Discriminator { get; } = new();
        
    /// <summary>
    /// Default options for <see cref="ICreateAuditedEntityConvention"/> entity convention
    /// </summary>
    public CreateAuditedOptions CreateAudited { get; } = new();
}