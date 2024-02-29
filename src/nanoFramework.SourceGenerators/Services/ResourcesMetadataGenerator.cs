using System.Collections;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Resources.NetStandard;

using nanoFramework.SourceGenerators.Models;
using nanoFramework.SourceGenerators.Providers;
using nanoFramework.SourceGenerators.Utils;

namespace nanoFramework.SourceGenerators.Services
{
    internal sealed class ResourcesMetadataGenerator : IResourcesMetadataGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IFileSystemService _fileSystemService;
        private readonly IResourceIdProvider _resourceIdProvider;
        private readonly IResourceUriPathProvider _uriPathProvider;
        private readonly IResourceContentTypeProvider _contentTypeProvider;
        private readonly IResourceContentEncodingProvider _contentEncodingProvider;

        public ResourcesMetadataGenerator(
            IFileSystem fileSystem,
            IFileSystemService fileSystemService,
            IResourceIdProvider resourceIdProvider,
            IResourceUriPathProvider uriPathProvider,
            IResourceContentTypeProvider contentTypeProvider,
            IResourceContentEncodingProvider contentEncodingProvider)
        {
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
            _fileSystemService = ParamChecker.Check(fileSystemService, nameof(fileSystemService));
            _resourceIdProvider = ParamChecker.Check(resourceIdProvider, nameof(resourceIdProvider));
            _uriPathProvider = ParamChecker.Check(uriPathProvider, nameof(uriPathProvider));
            _contentTypeProvider = ParamChecker.Check(contentTypeProvider, nameof(contentTypeProvider));
            _contentEncodingProvider = ParamChecker.Check(contentEncodingProvider, nameof(contentEncodingProvider));
        }

        public ResourceMetadata[] GenerateMetadata(string resxFilePath, ResourceMetadataGenerationOptions options)
        {
            Guard.ThrowIfNullOrEmpty(resxFilePath, nameof(resxFilePath));
            Guard.ThrowIfNull(options, nameof(options));

            var metadataValues = new List<ResourceMetadata>();

            using (var reader = new ResXResourceReader(resxFilePath) { UseResXDataNodes = true })
            {
                foreach (DictionaryEntry entry in reader)
                {
                    if (entry.Value is ResXDataNode dataNode)
                    {
                        var resourceName = dataNode.Name;
                        var relativeOrAbsoluteResourceFilePath = dataNode.FileRef.FileName;
                        var resxFileDirectoryAbsolutePath = _fileSystem.Path.GetDirectoryName(resxFilePath);
                        var resourceFileAbsolutePath = _fileSystemService.GetAbsolutePath(relativeOrAbsoluteResourceFilePath, resxFileDirectoryAbsolutePath);
                        var resourceFileInfo = _fileSystem.FileInfo.New(resourceFileAbsolutePath);

                        var resourceMetadata = new ResourceMetadata
                        {
                            Id = _resourceIdProvider.GetResourceId(resourceName)
                        };

                        if (options.ShouldGenerateName)
                        {
                            resourceMetadata.Name = resourceName;
                        }

                        if (options.ShouldGenerateUriPath)
                        {
                            resourceMetadata.UriPath = _uriPathProvider.GetUriPath(resourceName);
                        }

                        if (options.ShouldGenerateSize)
                        {
                            resourceMetadata.Size = resourceFileInfo.Length;
                        }

                        if (options.ShouldGenerateContentType)
                        {
                            resourceMetadata.ContentType = _contentTypeProvider.GetContentType(resourceName);
                        }

                        if (options.ShouldGenerateContentEncoding)
                        {
                            resourceMetadata.ContentEncoding = _contentEncodingProvider.GetContentEncoding(resourceFileInfo);
                        }

                        metadataValues.Add(resourceMetadata);
                    }
                }
            }

            return metadataValues.ToArray();
        }
    }
}
