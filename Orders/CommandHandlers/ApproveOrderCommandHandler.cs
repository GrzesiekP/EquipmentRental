using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Commands;
using Core.EventStore;
using MediatR;
using Orders.Aggregate;
using Orders.Commands;

namespace Orders.CommandHandlers
{
    public class ApproveOrderCommandHandler : ICommandHandler<ApproveOrder>
    {
        private readonly IMartenEventStoreRepository<Order> _orderEventStoreRepository;
    
        public ApproveOrderCommandHandler(IMartenEventStoreRepository<Order> orderEventStoreRepository)
        {
            _orderEventStoreRepository = orderEventStoreRepository;
        }
    
        public async Task<Unit> Handle(ApproveOrder command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(ApproveOrderCommandHandler)} handling {nameof(ApproveOrder)}");
            var order = await _orderEventStoreRepository.Find(command.OrderId);
    
            order.Approve(command);
    
            await _orderEventStoreRepository.Update(order);
            
            return Unit.Value;
        }
    }
}