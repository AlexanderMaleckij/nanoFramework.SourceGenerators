using System.Linq;

namespace nanoFramework.SourceGenerators.Extensions
{
    internal static class PathUtils
    {
        public static (string, string) GetLastTwoExtensions(string path)
        {
            string[] parts = path.Split('.');

            if (parts.Length == 1) // No extension
            {
                return (null, null);
            }
            if (parts.Length == 2) // One extension
            {
                return (null, $".{parts.Last()}");
            }
            else // Two or more extensions
            {
                return ($".{parts[parts.Length - 2]}", $".{parts[parts.Length - 1]}");
            }
        }
    }
}
