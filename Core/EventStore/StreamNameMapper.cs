using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Core.EventStore
{
    public class StreamNameMapper
    {
        private static readonly StreamNameMapper Instance = new();

        private readonly ConcurrentDictionary<Type, string> _typeNameMap = new();
        
        public static string ToStreamId<TStream>(object aggregateId, object? tenantId = null) =>
            ToStreamId(typeof(TStream), aggregateId);

        private static string ToStreamId(Type streamType, object aggregateId, object? tenantId = null)
        {
            var tenantPrefix = tenantId != null ? $"{tenantId}_"  : "";

            return $"{tenantPrefix}{ToStreamPrefix(streamType)}-{aggregateId}";
        }

        private static string ToStreamPrefix(Type streamType) => Instance._typeNameMap.GetOrAdd(streamType, (_) =>
        {
            var modulePrefix = streamType.Namespace!.Split(".").First();
            return $"{modulePrefix}_{streamType.Name}";
        });
    }
}