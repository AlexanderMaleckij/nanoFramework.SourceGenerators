using System.IO;
using System.IO.Abstractions;

namespace nanoFramework.SourceGenerators.Providers
{
    internal sealed class ResourceContentEncodingProvider : IResourceContentEncodingProvider
    {
        public string GetContentEncoding(IFileInfo fileInfo)
        {
            switch (fileInfo.Extension.ToLowerInvariant())
            {
                case ".br":
                    return "br";
                case ".gz":
                    return "gzip";
                case ".z":
                    return "compress";
                default:
                    return GetContentEncodingByFileSignature(fileInfo);
            }
        }

        private static string GetContentEncodingByFileSignature(IFileInfo fileInfo)
        {
            var headerBytes = new byte[4];

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                fileStream.Read(headerBytes, 0, headerBytes.Length);
            }

            if (IsDeflateEncoded(headerBytes))
            {
                return "deflate";
            }

            if (IsGzipEncoded(headerBytes))
            {
                return "gzip";
            }

            if (IsCompressEncoded(headerBytes))
            {
                return "compress";
            }

            return null;
        }

        private static bool IsDeflateEncoded(byte[] headerBytes)
        {
            return headerBytes.Length >= 2
                && headerBytes[0] == 0x78
                && (headerBytes[1] == 0x01 || headerBytes[1] == 0x9C || headerBytes[1] == 0xDA);
        }

        private static bool IsGzipEncoded(byte[] headerBytes)
        {
            return headerBytes.Length >= 2
                && headerBytes[0] == 0x1F
                && headerBytes[1] == 0x8B;
        }

        private static bool IsCompressEncoded(byte[] headerBytes)
        {
            return headerBytes.Length >= 2
                && headerBytes[0] == 0x1F
                && headerBytes[1] == 0x9D;
        }
    }
}
