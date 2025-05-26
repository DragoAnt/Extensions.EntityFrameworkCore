namespace DragoAnt.EntityFrameworkCore.Extensions.DependencyInjection;

[Flags]
public enum ResSqlFile
{
    None = 0,
    Apply = 1,
    Revert = 2,
    All = Apply | Revert
}