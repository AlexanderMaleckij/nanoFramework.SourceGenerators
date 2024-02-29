namespace nanoFramework.SourceGenerators
{
    internal static class Constants
    {
        public const string GeneratedFileNameTemplate = "{0}.g.cs";
        public const string ResourceMetadataNamespace = "nanoFramework.ResourcesMetadata";

        public static class Configuration
        {
            private const string BuildMetadataBaseKey = "build_metadata.AdditionalFiles.";

            public static class ResourceMetadataKeys
            {
                private const string KeyPrefix = "ResourceMetadata_";
                public const string ShouldGenerateName = BuildMetadataBaseKey + KeyPrefix + "IncludeName";
                public const string ShouldGenerateUriPath = BuildMetadataBaseKey + KeyPrefix + "IncludeUriPath";
                public const string ShouldGenerateSize = BuildMetadataBaseKey + KeyPrefix + "IncludeSize";
                public const string ShouldGenerateContentType = BuildMetadataBaseKey + KeyPrefix + "IncludeContentType";
                public const string ShouldGenerateContentEncoding = BuildMetadataBaseKey + KeyPrefix + "IncludeContentEncoding";
            }

            public static class ResourceMetadataProviderKeys
            {
                private const string KeyPrefix = "ResourcesMetadataProvider_";
                public const string ShouldGenerateKey = BuildMetadataBaseKey + KeyPrefix + "Generate";
                public const string ShouldGenerateFindByNameKey = BuildMetadataBaseKey + KeyPrefix + "GenerateFindByName";
                public const string ShouldGenerateFindByUriPathKey = BuildMetadataBaseKey + KeyPrefix + "GenerateFindByUriPath";
            }
        }

        public static class ResourceMetadata
        {
            public const string ClassIdentifier = "ResourceMetadata";

            public static class PropertyNames
            {
                public const string Id = "Id";
                public const string Name = "Name";
                public const string UriPath = "UriPath";
                public const string Size = "Size";
                public const string ContentType = "ContentType";
                public const string ContentEncoding = "ContentEncoding";
            }
        }

        public static class ResourcesMetadataProvider
        {
            public const string ClassIdentifier = "ResourcesMetadataProvider";

            public static class MethodNames
            {
                public const string FindByUriPath = "FindByUriPath";
                public const string FindByName = "FindByName";
            }
        }

        public static class ResourceId
        {
            public const string EnumIdentifier = "ResourceId";
        }
    }
}
