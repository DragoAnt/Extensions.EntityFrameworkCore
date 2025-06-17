# MSBuild common routine for DragoAnt repositories

## Prerequisites

- .NET SDK (the latest version recommended)
- MSBuild
- Git

## Overview

This repository contains common MSBuild routines and configurations used across company repositories. It provides standardized build settings, version
management, and project configurations.

_You can copy this repository and adapt for your needs._

## Key Features

### Company-Specific Settings

- Standardized company information (ManufacturerName, FullManufacturerName)
- Default warning configurations
- GitLab CI integration
- Version management strategies:
    - DateBased (default): `yyyy.M.d.$([MSBuild]::Modulo($(CIPipelineId), $(MaxBuildNumber)))`
    - SemVer: Semantic versioning with build number
    - SemVer4: Modified semantic versioning(uses CI pipeline id as build number)
    - VersionTag: Git tag-based versioning

### Build Configuration

- Nullable reference types enabled by default
- Implicit usings enabled
- Latest C# language version
- Code style enforcement in build
- Warning treatment as errors
- Documentation file generation for NuGet packages

### Project Structure

The repository is organized into several key directories:

- `shared/`: Contains common build configurations
    - `common/`: Common properties and settings
    - `tests/`: Test project configurations
    - `utils/`: Utility build scripts
    - `checks/`: Build validation checks

### Key Files

- `init.company.specific.props`: Company-wide property definitions
- `init.company.specific.targets`: Company-wide target definitions
- `shared/init.props`: Base property initialization
- `shared/init.targets`: Base target initialization
- `shared/version.props`: Version management configuration
- Various local configuration files for secrets and project-specific settings

## Usage

1. Add submodule to your git repository. File `.gitmodules`
   ```
   [submodule ".msbuild"]
	path = .msbuild
	url = ../MSBuild.Routine.git
	branch = master
   ```

2. Add `Directory.Build.props` in your solution's directory:

```xml

<Project>
  <PropertyGroup>
    <RepositoryUrl><!--your repo path --></RepositoryUrl>

    <TargetFramework>net9.0</TargetFramework>
    <IsPackable>false</IsPackable>

    <SlnSecretsId><!--your sln name --></SlnSecretsId>
  </PropertyGroup>

  <Import Project="$(MSBuildThisFileDirectory).msbuild\shared\init.props" />

</Project>
```

3. Add `Directory.Build.targets` in your solution's directory:

```xml

<Project>

  <Import Project="$(MSBuildThisFileDirectory).msbuild\shared\init.targets" />

</Project>
```

You can change properties to enable/disable MSBuild routine functionality in `Directory.Build.props` for all sln projects or in `.csproj` file for an individual
project.

## Version Management

The system supports multiple versioning strategies:

- **DateBased** (default): Uses date-based versioning with build number
- **SemVer**: Follows semantic versioning (https://semver.org/)
- **SemVer4**: Modified semantic versioning with build number(uses CI pipeline id as build number)
- **VersionTag**: Uses Git tags for versioning

## Security

The repository includes support for managing secrets and sensitive information:

- Local secrets management
- Directory-level secrets configuration
- Secret cleanup routines

## Best Practices

1. Always use the company-specific settings for consistency
2. Enable code style enforcement in builds
3. Use the provided version management system
4. Follow the established project structure
5. Utilize the built-in security features for managing secrets

## Questions

If you have questions or suggestions, feel free to write to [Issues](https://github.com/DragoAnt/MSBuild.Routine/issues).