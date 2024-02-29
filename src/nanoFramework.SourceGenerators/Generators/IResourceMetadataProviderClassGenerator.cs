using System.Collections.Generic;

using nanoFramework.SourceGenerators.Models;

namespace nanoFramework.SourceGenerators.Generators
{
    internal interface IResourceMetadataProviderClassGenerator
    {
        string GenerateSource(
            IEnumerable<ResourceMetadata> values,
            ResourceMetadataGenerationOptions metadataClassOptions,
            ResourceMetadataProviderGenerationOptions providerGenerationOptions);
    }
}
