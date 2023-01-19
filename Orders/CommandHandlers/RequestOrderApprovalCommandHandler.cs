using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Commands;
using Core.EventStore;
using MediatR;
using Orders.Aggregate;
using Orders.Commands;

namespace Orders.CommandHandlers
{
    public class RequestOrderApprovalCommandHandler : ICommandHandler<RequestOrderApproval>
    {
        private readonly IMartenEventStoreRepository<Order> _orderEventStoreRepository;

        public RequestOrderApprovalCommandHandler(IMartenEventStoreRepository<Order> orderEventStoreRepository)
        {
            _orderEventStoreRepository = orderEventStoreRepository;
        }
        
        public async Task<Unit> Handle(RequestOrderApproval command, CancellationToken cancellationToken)
        {
            var order = await _orderEventStoreRepository.Find(command.OrderId);

            order.RequestApproval(command);

            await _orderEventStoreRepository.Update(order);
            
            return Unit.Value;
        }
    }
}