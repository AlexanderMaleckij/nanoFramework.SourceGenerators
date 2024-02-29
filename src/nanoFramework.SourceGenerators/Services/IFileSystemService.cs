namespace nanoFramework.SourceGenerators.Services
{
    internal interface IFileSystemService
    {
        string GetAbsolutePath(string path, string basePath);
    }
}
