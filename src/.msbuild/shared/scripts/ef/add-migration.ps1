param (
# migration name (required)
    [Parameter(Mandatory = $true)][string]$migrationName = '',
# Path to initialization file
    [string]$initFile = ''
)

# Validate migration name
if ( [string]::IsNullOrWhiteSpace($migrationName))
{
    Write-Error "Migration name cannot be empty or null"
    exit 1
}

# Source the initialization script
. "$PSScriptRoot/init.ps1" -initFile $initFile
if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

# Source the test-migration-file script
. "$PSScriptRoot/test-migration-file.ps1"

# Execute the migration command
dotnet ef migrations add $migrationName -c $dbContext -p $dbContextProj -v --no-build

Write-Host

# Call the function to handle the migration file
Test-MigrationFile -migrationName $migrationName -dbContextProj $dbContextProj