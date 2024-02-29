using System;
using System.Linq;

using nanoFramework.SourceGenerators.Extensions;
using nanoFramework.SourceGenerators.Utils;

namespace nanoFramework.SourceGenerators.Providers
{
    internal sealed class ResourceUriPathProvider : IResourceUriPathProvider
    {
        private static readonly string[] ResourceNameEndingsToTrim = new string[]
        {
            ".gz",
            ".br",
            ".z"
        };

        private readonly string[] _resourceNameEndingsToTrim;

        public ResourceUriPathProvider()
        {
            _resourceNameEndingsToTrim = ResourceNameEndingsToTrim;
        }

        public string GetUriPath(string resourceName)
        {
            Guard.ThrowIfNullOrWhitespace(resourceName, nameof(resourceName));

            var (penultimateExtension, lastExtension) = PathUtils.GetLastTwoExtensions(resourceName);

            var path = resourceName;

            // Make sure name has the "name.x.y" format and ".y" should be trimmed.
            if (penultimateExtension != null && lastExtension != null && _resourceNameEndingsToTrim.Contains(lastExtension, StringComparer.InvariantCultureIgnoreCase))
            {
                path = resourceName.Substring(0, resourceName.Length - lastExtension.Length);
            }

            if (!path.StartsWith("/"))
            {
                path = string.Concat("/", path);
            }

            path = path.Replace("\\", "/");

            return path;
        }
    }
}
