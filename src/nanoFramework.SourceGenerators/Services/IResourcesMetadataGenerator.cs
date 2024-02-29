using nanoFramework.SourceGenerators.Models;

namespace nanoFramework.SourceGenerators.Services
{
    internal interface IResourcesMetadataGenerator
    {
        ResourceMetadata[] GenerateMetadata(string resxFilePath, ResourceMetadataGenerationOptions options);
    }
}
