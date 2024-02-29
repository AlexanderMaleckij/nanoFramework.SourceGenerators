[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/amaletski.nanoFramework.SourceGenerators.svg?label=NuGet&style=flat&logo=nuget)](https://www.nuget.org/packages/amaletski.nanoFramework.SourceGenerators/)

# nanoFramework.SourceGenerators

## Package Installation (Visual Studio)

 - Install the NuGet package as usual.
 - Unload the project:
   - Right-click the project in the "Solution Explorer" window.
   - Select "Unload Project".
 - Open the project file:
   - Right-click the unloaded project in the "Solution Explorer" window.
   - Select "Edit Project File".
 - Edit the project file:
   - Add the following `Import` node (somewhere at the top of the project node).
     ```xml
     <Import Project="..\packages\amaletski.nanoFramework.SourceGenerators.[PackageVersion]\build\netnano1.0\amaletski.nanoFramework.SourceGenerators.props" />
     ```
     Note: Replace [PackageVersion] with the appropriate version.
   - Add the following `ItemGroup` with `Analyzer` node:
     ```xml
       <ItemGroup>
         <Analyzer Include="..\packages\amaletski.nanoFramework.SourceGenerators.[PackageVersion]\analyzers\dotnet\cs\*.dll" />
       </ItemGroup>
     ```
     Note: Replace [PackageVersion] with the appropriate version.
   - Press `Ctrl+S` to save changes.
- Reload the project:
   - Right click the unloaded project in the "Solution Explorer" window.
   - Select "Reload Project".

> [!IMPORTANT]
> Make sure to update the imports after updating the NuGet package version.

## Use Cases

### Generating the ResourcesMetadataProvider class

To utilize this feature, an `AdditionalFiles` item node must be added to the `nfproj` file. This node can include attributes to specify which metadata properties and metadata provider methods should be generated. Below is an example that includes all applicable attributes:
```xml
<ItemGroup>
  <AdditionalFiles Include="Resources.resx"
    ResourceMetadata_IncludeName="true"
    ResourceMetadata_IncludeUriPath="true"
    ResourceMetadata_IncludeSize="true"
    ResourceMetadata_IncludeContentType="true"
    ResourceMetadata_IncludeContentEncoding="true"
    ResourcesMetadataProvider_Generate="true"
    ResourcesMetadataProvider_GenerateFindByName="true"
    ResourcesMetadataProvider_GenerateFindByUriPath="true" />
</ItemGroup>
```
