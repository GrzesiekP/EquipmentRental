using System.Threading;
using System.Threading.Tasks;
using Core;
using MediatR;
using Orders.Commands;

namespace Orders.CommandHandlers
{
    public class RequestApprovalCommandHandler : ICommandHandler<RequestApproval>
    {
        private readonly IMartenEventStoreRepository<Order> _orderEventStoreRepository;

        public RequestApprovalCommandHandler(IMartenEventStoreRepository<Order> orderEventStoreRepository)
        {
            _orderEventStoreRepository = orderEventStoreRepository;
        }
        
        public async Task<Unit> Handle(RequestApproval requestApproval, CancellationToken cancellationToken)
        {
            var order = await _orderEventStoreRepository.Find(requestApproval.OrderId);

            order.RequestApproval(requestApproval);

            await _orderEventStoreRepository.Update(order);
            
            return Unit.Value;
        }
    }
}