namespace nanoFramework.SourceGenerators.Models
{
    internal sealed class ResourceMetadataProviderGenerationOptions
    {
        public ResourceMetadataProviderGenerationOptions(bool shouldGenerateFindByName, bool shouldGenerateFindByUriPath)
        {
            ShouldGenerateFindByName = shouldGenerateFindByName;
            ShouldGenerateFindByUriPath = shouldGenerateFindByUriPath;
        }

        public bool ShouldGenerateFindByName { get; }

        public bool ShouldGenerateFindByUriPath { get; }
    }
}
