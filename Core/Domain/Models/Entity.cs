using System;

namespace Core.Domain.Models
{
    public class Entity
    {
        public Guid Identity { get; }

        protected Entity()
        {
            Identity = Guid.NewGuid();
        }
    }
}