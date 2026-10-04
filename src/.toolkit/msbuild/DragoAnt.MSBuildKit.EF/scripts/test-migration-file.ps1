function Test-MigrationFile
{
    param (
        [string]$migrationName,
        [string]$dbContextProj
    )

    Write-Host "Check for empty migration..."

    # Try to find the migration file in the project
    $projectDir = Split-Path -Parent $dbContextProj
    $migrationFiles = Get-ChildItem -Path $projectDir -Recurse -Filter "*_$migrationName.cs" -File

    if ($migrationFiles.Count -eq 0)
    {
        Write-Error "Failed to find migration file. Raw output:"
        Write-Error $migrationOutput
        exit 1
    }

    $migrationFilePath = $migrationFiles[0].FullName

    # Check if the migration file exists
    if (-not (Test-Path $migrationFilePath))
    {
        Write-Error "Migration file not found at: $migrationFilePath"
        exit 1
    }

    # Read the migration file content
    $migrationContent = Get-Content $migrationFilePath -Raw

    # Check if the Up method is empty (only whitespace between braces)
    if ($migrationContent -match 'protected override void Up\(MigrationBuilder migrationBuilder\)\s*{\s*}')
    {
        Write-Host "Empty migration detected. Removing migration files..."

        # Get the designer file path (same name with .Designer.cs extension)
        $designerFilePath = $migrationFilePath -replace '\.cs$', '.Designer.cs'

        # Remove both files if they exist
        if (Test-Path $migrationFilePath)
        {
            Remove-Item $migrationFilePath -Force
            Write-Host "Removed migration file: $migrationFilePath" -ForegroundColor DarkGray
        }

        if (Test-Path $designerFilePath)
        {
            Remove-Item $designerFilePath -Force
            Write-Host "Removed designer file: $designerFilePath" -ForegroundColor DarkGray
        }

        Write-Host "Empty migration files have been removed" -ForegroundColor Yellow
        exit 0
    }
    else
    {
        Write-Host "Migration created successfully: " -ForegroundColor Green -NoNewline
        Write-Host $migrationName -ForegroundColor Yellow
    }

}