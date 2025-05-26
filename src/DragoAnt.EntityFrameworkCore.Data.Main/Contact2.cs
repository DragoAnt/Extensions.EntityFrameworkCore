using DragoAnt.EntityConventions.Contacts;

namespace DragoAnt.EntityFrameworkCore.Data.Main;

[SourceSystemIdOptions(MaxLength = 100, IsUnicode = false, HasIndex = false)]
public class Contact2 : Entity,
    ICreateAuditedEntityConvention,
    IWithSourceSystemIdEntityConventionGuid
{
    public string Name { get; set; }
    public string EMail { get; set; }
    public ContactType Type { get; set; }
    public ContactStrType TypeName { get; set; }

    public ContactStr2Type TypeName2 { get; set; }

    public ContactTypeNullable? TypeNameNullable { get; set; }
}