using nanoFramework.SourceGenerators.Utils;

namespace nanoFramework.SourceGenerators.Providers
{
    internal sealed class ResourceIdProvider : IResourceIdProvider
    {
        public short GetResourceId(string resourceName)
        {
            Guard.ThrowIfNull(resourceName, nameof(resourceName));

            var hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;

            for (var i = 0; i < resourceName.Length; ++i)
            {
                var c = resourceName[i];
                if (i % 2 == 0)
                {
                    hash1 = (hash1 << 5) + hash1 ^ c;
                }
                else
                {
                    hash2 = (hash2 << 5) + hash2 ^ c;
                }
            }

            var hash = hash1 + hash2 * 1566083941;

            return (short)((short)(hash >> 16) ^ (short)hash);
        }
    }
}
