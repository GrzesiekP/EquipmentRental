using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.EventStore
{
    public static class JsonObjectContractProvider
    {
        private static readonly Type ConstructorAttributeType = typeof(JsonConstructorAttribute);
        private static readonly ConcurrentDictionary<string, JsonObjectContract> Constructors = new();

        public static JsonObjectContract UsingNonDefaultConstructor(
            JsonObjectContract contract,
            Type objectType,
            Func<ConstructorInfo, JsonPropertyCollection, IList<JsonProperty>> createConstructorParameters)
        {
            return Constructors.GetOrAdd(objectType.AssemblyQualifiedName!, _ =>
            {
                var nonDefaultConstructor = GetNonDefaultConstructor(objectType);

                if (nonDefaultConstructor == null)
                {
                    return contract;
                }

                contract.OverrideCreator = GetObjectConstructor(nonDefaultConstructor);
                contract.CreatorParameters.Clear();

                var parameters = createConstructorParameters(nonDefaultConstructor, contract.Properties);
                foreach (var constructorParameter in parameters)
                {
                    contract.CreatorParameters.Add(constructorParameter);
                }

                return contract;
            });
        }

        private static ObjectConstructor<object> GetObjectConstructor(MethodBase method)
        {
            var constructor = method as ConstructorInfo;

            if (constructor == null)
            {
                return arrayOfObjects => method.Invoke(null, arrayOfObjects)!;
            }

            if (!constructor.GetParameters().Any())
            {
                return _ => constructor.Invoke(Array.Empty<object?>());
            }

            return arrayOfObjects => constructor.Invoke(arrayOfObjects);
        }

        private static ConstructorInfo? GetNonDefaultConstructor(Type type)
        {
            // Use default contract for non-object types.
            if (!IsObjectType(type))
                return null;

            return GetAttributeConstructor(type)
                   ?? GetTheMostSpecificConstructor(type);
        }

        private static ConstructorInfo? GetAttributeConstructor(Type type)
        {
            // Use default contract for non-object types.
            if (!IsObjectType(type))
                return null;

            var constructors = type
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(c => 
                    c.GetCustomAttributes().Any(a => a.GetType() == ConstructorAttributeType))
                .ToList();

            return constructors.Count switch
            {
                1 => constructors[0],
                > 1 => throw new JsonException($"Multiple constructors with a {ConstructorAttributeType.Name}."),
                _ => null
            };
        }
        
        private static bool IsObjectType(Type type) => !(type.IsPrimitive || type.IsEnum);

        private static ConstructorInfo? GetTheMostSpecificConstructor(Type objectType) =>
            objectType
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .OrderByDescending(e => e.GetParameters().Length)
                .FirstOrDefault();
    }
}