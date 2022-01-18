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
            var order = Order.Initialize(command.OrderId);
            order.Submit(command);
            
            await _orderEventStoreRepository.Add(order);
            
            return Unit.Value;
        }
    }
}