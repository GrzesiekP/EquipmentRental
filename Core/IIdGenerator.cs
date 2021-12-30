using System;

namespace Core
{
    public interface IIdGenerator
    {
        Guid New();
    }
}