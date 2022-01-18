using System;
using Marten.Schema.Identity;

namespace Core.EventStore
{
    public class MartenIdGenerator : IIdGenerator
    {
        public Guid New() => CombGuidIdGeneration.NewGuid();
    }
}