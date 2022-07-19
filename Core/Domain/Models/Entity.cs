using System;

namespace Core.Domain.Models
{
    public class Entity
    {
        protected Guid Identity { get; }

        protected Entity()
        {
            Identity = Guid.NewGuid();
        }
    }
}