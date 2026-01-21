using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Shared cache for SettingsDefinitionCatalogProfile instances.
    /// This enables using constructors directly while still returning the same cached instance
    /// per type + sanitized catalog name.
    /// </summary>
    internal static class SettingsDefinitionRegistry
    {
        private static readonly Dictionary<SerializerCacheUtility.CacheKey, object> Cache = new();
        private static readonly object Lock = new();

        public static TCatalogProfile GetOrCreate<TCatalogProfile>(string name, Func<string, TCatalogProfile> create)
            where TCatalogProfile : class
        {
            var sanitizedName = SerializerCacheUtility.SanitizeName(name);
            var cacheKey = new SerializerCacheUtility.CacheKey(typeof(TCatalogProfile), sanitizedName);

            lock (Lock)
            {
                if (Cache.TryGetValue(cacheKey, out var existing))
                    return (TCatalogProfile)existing;

                var created = create(sanitizedName);
                Cache[cacheKey] = created;
                return created;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearCacheOnLoad()
        {
            lock (Lock) Cache.Clear();
        }
    }
}
