using System;

namespace Core.EventStore
{
    public interface IIdGenerator
    {
        // ReSharper disable once UnusedMember.Global
        // Used by Marten and need to be injected
        Guid New();
    }
}