param (
# Path to initialization file
    [string]$initFile = ''
)

# Source the initialization script
. "$PSScriptRoot/init.ps1" -initFile $initFile
if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

dotnet ef migrations remove -c $dbContext -p $dbContextProj -v --no-build