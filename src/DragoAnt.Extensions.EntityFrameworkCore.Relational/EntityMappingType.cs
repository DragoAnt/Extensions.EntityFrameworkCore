namespace DragoAnt.Extensions.EntityFrameworkCore.Relational;

public enum EntityMappingType
{
    Table = 0,
    View = 1,
    Function = 2,
    SqlQuery = 3,
    InMemoryQuery = 4
}