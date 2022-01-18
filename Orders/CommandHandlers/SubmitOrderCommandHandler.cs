using System.Threading;
using System.Threading.Tasks;
using Core.Domain;
using Core.EventStore;
using MediatR;
using Orders.Aggregate;
using Orders.Commands;

namespace Orders.CommandHandlers
{
    public class SubmitOrderCommandHandler : ICommandHandler<SubmitOrder>
    {
        private readonly IMartenEventStoreRepository<Order> _orderEventStoreRepository;

        public SubmitOrderCommandHandler(IMartenEventStoreRepository<Order> orderEventStoreRepository)
        {
            _orderEventStoreRepository = orderEventStoreRepository;
        }

        public async Task<Unit> Handle(SubmitOrder command, CancellationToken cancellationToken)
        {
            // On init order submitted is applied and enqueued
            var order = Order.Submit(command.OrderId);
            
            // on add enqueued events are publsihed
            await _orderEventStoreRepository.Add(order);
            
            return Unit.Value;
        }
    }
}