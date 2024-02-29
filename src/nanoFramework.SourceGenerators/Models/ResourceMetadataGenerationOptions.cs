namespace nanoFramework.SourceGenerators.Models
{
    internal sealed class ResourceMetadataGenerationOptions
    {
        public ResourceMetadataGenerationOptions(
            bool shouldGenerateName,
            bool shouldGenerateUriPath,
            bool shouldGenerateSize,
            bool shouldGenerateContentType,
            bool shouldGenerateContentEncoding)
        {
            ShouldGenerateName = shouldGenerateName;
            ShouldGenerateUriPath = shouldGenerateUriPath;
            ShouldGenerateSize = shouldGenerateSize;
            ShouldGenerateContentType = shouldGenerateContentType;
            ShouldGenerateContentEncoding = shouldGenerateContentEncoding;
        }

        public bool ShouldGenerateName { get; }

        public bool ShouldGenerateUriPath { get; }

        public bool ShouldGenerateSize { get; }

        public bool ShouldGenerateContentType { get; }

        public bool ShouldGenerateContentEncoding { get; }
    }
}
