namespace nanoFramework.SourceGenerators.Models
{
    internal sealed class ResourceMetadata
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public string UriPath { get; set; }

        public long Size { get; set; }

        public string ContentType { get; set; }

        public string ContentEncoding { get; set; }
    }
}
