using nanoFramework.SourceGenerators.Models;

namespace nanoFramework.SourceGenerators.Generators
{
    internal interface IResourceMetadataClassGenerator
    {
        string GenerateSource(ResourceMetadataGenerationOptions options);
    }
}
