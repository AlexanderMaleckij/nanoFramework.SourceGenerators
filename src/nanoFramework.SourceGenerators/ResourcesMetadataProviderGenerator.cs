using System;
using System.IO.Abstractions;
using System.Linq;

using Microsoft.CodeAnalysis;

using nanoFramework.SourceGenerators.Generators;
using nanoFramework.SourceGenerators.Models;
using nanoFramework.SourceGenerators.Providers;
using nanoFramework.SourceGenerators.Services;

using static nanoFramework.SourceGenerators.Constants.Configuration;

namespace nanoFramework.SourceGenerators
{
    [Generator]
    internal sealed class ResourcesMetadataProviderGenerator : ISourceGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IResourceIdEnumGenerator _resourceIdEnumGenerator;
        private readonly IResourceMetadataClassGenerator _metadataClassGenerator;
        private readonly IResourceMetadataProviderClassGenerator _providerClassGenerator;
        private readonly IResourcesMetadataGenerator _resourcesMetadataGenerator;

        public ResourcesMetadataProviderGenerator()
        {
            _fileSystem = new FileSystem();
            _resourceIdEnumGenerator = new ResourceIdEnumGenerator();
            _metadataClassGenerator = new ResourceMetadataClassGenerator();
            _providerClassGenerator = new ResourceMetadataProviderClassGenerator();
            _resourcesMetadataGenerator = new ResourcesMetadataGenerator(
                _fileSystem,
                new FileSystemService(_fileSystem),
                new ResourceIdProvider(),
                new ResourceUriPathProvider(),
                new ResourceContentTypeProvider(),
                new ResourceContentEncodingProvider());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var matchingResxFiles = GetMatchingResxFiles(context);

            if (matchingResxFiles.Length == 0)
            {
                return;
            }

            if (matchingResxFiles.Length > 1)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "RMPG0001",
                            "Resources metadata provider generation failed.",
                            "Resource metadata provider cannot be generated due to multiple .resx files being specified.",
                            "Configuration",
                            DiagnosticSeverity.Error,
                            true
                        ),
                        Location.None
                    )
                );
                return;
            }

            var resxFile = matchingResxFiles[0];

            if (!_fileSystem.File.Exists(resxFile.Path))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "RMPG0001",
                            "Configuration issue detected.",
                            "The .resx file for which the {0} should be generated does not exist. Path: {1}",
                            "Configuration",
                            DiagnosticSeverity.Warning,
                            true
                        ),
                        Location.None,
                        Constants.ResourcesMetadataProvider.ClassIdentifier,
                        resxFile.Path
                    )
                );
                return;
            }

            var metadataClassGenerationOptions = GetMetadataGenerationOptions(context, resxFile);
            var metadataProviderGenerationOptions = GetResourceMetadataProviderGenerationOptions(context, resxFile);

            if (metadataProviderGenerationOptions.ShouldGenerateFindByName && !metadataClassGenerationOptions.ShouldGenerateName)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "RMPG0001",
                            "Configuration issue detected.",
                            "The {0} method in {1} will always return null because the {2} metadata is not included.",
                            "Configuration",
                            DiagnosticSeverity.Warning,
                            true
                        ),
                        Location.None,
                        Constants.ResourcesMetadataProvider.MethodNames.FindByName,
                        Constants.ResourcesMetadataProvider.ClassIdentifier,
                        Constants.ResourceMetadata.PropertyNames.Name
                    )
                );
            }

            if (metadataProviderGenerationOptions.ShouldGenerateFindByUriPath && !metadataClassGenerationOptions.ShouldGenerateUriPath)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "RMPG0001",
                            "Configuration issue detected.",
                            "The {0} method in {1} will always return null because the {2} metadata is not included.",
                            "Configuration",
                            DiagnosticSeverity.Warning,
                            true
                        ),
                        Location.None,
                        Constants.ResourcesMetadataProvider.MethodNames.FindByUriPath,
                        Constants.ResourcesMetadataProvider.ClassIdentifier,
                        Constants.ResourceMetadata.PropertyNames.UriPath
                    )
                );
            }

            var metadataValues = _resourcesMetadataGenerator.GenerateMetadata(resxFile.Path, metadataClassGenerationOptions);

            var enumSource = _resourceIdEnumGenerator.GenerateSource();
            var metadataClassSource = _metadataClassGenerator.GenerateSource(metadataClassGenerationOptions);
            var providerClassSource = _providerClassGenerator.GenerateSource(metadataValues, metadataClassGenerationOptions, metadataProviderGenerationOptions);

            context.AddSource(string.Format(Constants.GeneratedFileNameTemplate, Constants.ResourceId.EnumIdentifier), enumSource);
            context.AddSource(string.Format(Constants.GeneratedFileNameTemplate, Constants.ResourceMetadata.ClassIdentifier), metadataClassSource);
            context.AddSource(string.Format(Constants.GeneratedFileNameTemplate, Constants.ResourcesMetadataProvider.ClassIdentifier), providerClassSource);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        private AdditionalText[] GetMatchingResxFiles(GeneratorExecutionContext context)
        {
            return context.AdditionalFiles
                .Where(file =>
                    file.Path.EndsWith(".resx", StringComparison.InvariantCultureIgnoreCase)
                    && IsOptionEnabled(context, file, ResourceMetadataProviderKeys.ShouldGenerateKey))
                .ToArray();
        }

        private static ResourceMetadataGenerationOptions GetMetadataGenerationOptions(
            GeneratorExecutionContext context,
            AdditionalText resxFile)
        {
            return new ResourceMetadataGenerationOptions(
                IsOptionEnabled(context, resxFile, ResourceMetadataKeys.ShouldGenerateName),
                IsOptionEnabled(context, resxFile, ResourceMetadataKeys.ShouldGenerateUriPath),
                IsOptionEnabled(context, resxFile, ResourceMetadataKeys.ShouldGenerateSize),
                IsOptionEnabled(context, resxFile, ResourceMetadataKeys.ShouldGenerateContentType),
                IsOptionEnabled(context, resxFile, ResourceMetadataKeys.ShouldGenerateContentEncoding)
            );
        }

        private static ResourceMetadataProviderGenerationOptions GetResourceMetadataProviderGenerationOptions(
            GeneratorExecutionContext context,
            AdditionalText resxFile)
        {
            return new ResourceMetadataProviderGenerationOptions(
                IsOptionEnabled(context, resxFile, ResourceMetadataProviderKeys.ShouldGenerateFindByNameKey),
                IsOptionEnabled(context, resxFile, ResourceMetadataProviderKeys.ShouldGenerateFindByUriPathKey)
            );
        }

        private static bool IsOptionEnabled(GeneratorExecutionContext context, AdditionalText resxText, string key)
        {
            if (context.AnalyzerConfigOptions.GetOptions(resxText).TryGetValue(key, out var value))
            {
                return value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
    }
}
