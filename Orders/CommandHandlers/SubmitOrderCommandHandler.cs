using System.Threading;
using System.Threading.Tasks;
using Core;
using MediatR;
using Orders.Commands;

namespace Orders.CommandHandlers
{
    public class SubmitOrderCommandHandler : ICommandHandler<SubmitOrder>
    {
        private readonly IEventStoreRepository<Order> _orderEventStoreRepository;

        public SubmitOrderCommandHandler(IEventStoreRepository<Order> orderEventStoreRepository)
        {
            _orderEventStoreRepository = orderEventStoreRepository;
        }

        public async Task<Unit> Handle(SubmitOrder command, CancellationToken cancellationToken)
        {
            var order = Order.Submit(command.OrderId);

            await _orderEventStoreRepository.Add(order);
            
            return Unit.Value;
        }
    }
}