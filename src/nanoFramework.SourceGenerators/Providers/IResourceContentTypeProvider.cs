namespace nanoFramework.SourceGenerators.Providers
{
    internal interface IResourceContentTypeProvider
    {
        string GetContentType(string subpath);
    }
}
