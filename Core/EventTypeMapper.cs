using System;
using System.Collections.Concurrent;

namespace Core
{
    public class EventTypeMapper
    {
        private static readonly EventTypeMapper Instance = new();
        private readonly ConcurrentDictionary<Type, string> _typeNameMap = new();
        private readonly ConcurrentDictionary<string, Type> _typeMap = new();
        
        public static string ToName(Type eventType) => Instance._typeNameMap.GetOrAdd(eventType, (_) =>
        {
            var eventTypeName = eventType.FullName!.Replace(".", "_");

            Instance._typeMap.AddOrUpdate(eventTypeName, eventType, (_, _) => eventType);

            return eventTypeName;
        });
    }
}