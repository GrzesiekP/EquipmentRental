using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Events;

namespace Orders
{
    public interface IAsyncDomainEventPublisher
    {
        Task Publish(IEvent eventum, CancellationToken cancellationToken);

        async Task Publish(IEnumerable<IEvent> events, CancellationToken cancellationToken)
        {
            foreach (var eventum in events)
            {
                await Publish(eventum, cancellationToken);
            }
        }
    }
}