param (
    [string]$initFile = ''
)

# Set default init file if not provided
if ([string]::IsNullOrWhiteSpace($initFile)) {
    $initFile = '../../../../ef-scripts-init.ps1'
}

# Resolve the init file path relative to the script location if it's not an absolute path
if (-not [System.IO.Path]::IsPathRooted($initFile)) {
    $initFilePath = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot $initFile))
}
else {
    $initFilePath = $initFile
}

if (-not (Test-Path $initFilePath)) {
    Write-Error "Initialization file not found at: $initFilePath"
    exit 1
}

# Source the initialization file and handle errors
try {
    . $initFilePath
}
catch {
    Write-Error "Failed to source initialization file: $_"
    exit 1
}

# Build the project
dotnet build $dbContextProj --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to build project: $dbContextProj"
    exit 1
}

# Update EF tools
dotnet tool update --global dotnet-ef 

Write-Host