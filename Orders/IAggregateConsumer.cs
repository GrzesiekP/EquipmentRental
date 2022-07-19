using System.Collections.Generic;
using Core.Domain.Commands;
using Core.Domain.Events;

namespace Orders
{
    public interface IAggregateConsumer<TC, TE> where TC: ICommand where TE : IEvent
    {
        IEnumerable<TE> Consume(TC command);
        void Apply(TE e);
    }
}