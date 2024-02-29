using System.IO.Abstractions;

namespace nanoFramework.SourceGenerators.Providers
{
    internal interface IResourceContentEncodingProvider
    {
        string GetContentEncoding(IFileInfo fileInfo);
    }
}
