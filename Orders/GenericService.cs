using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Aggregates;
using Core.Domain.Commands;
using Core.EventStore;
using MediatR;
using Orders.Aggregate;
using Orders.Commands;

namespace Orders
{
    // public class GenericService<T> : 
    // ICommandHandler<RequestOrderApproval>, 
    // ICommandHandler<ApproveOrder>,
    // ICommandHandler<SubmitOrder>,
    // ICommandHandler<ICommand>
        // where T : Aggregate<Guid>
    // {
    //     private readonly IEventStoreRepository<T> _eventStoreRepository;
    //     private readonly IAsyncDomainEventPublisher _eventPublisher;
    //
    //     public GenericService(
    //         IAsyncDomainEventPublisher eventPublisher, 
    //         IEventStoreRepository<T> eventStoreRepository)
    //     {
    //         _eventPublisher = eventPublisher;
    //         _eventStoreRepository = eventStoreRepository;
    //     }
    //
    //     public Task<Unit> Handle(AggregateCommand request, CancellationToken cancellationToken)
    //     {
    //         var aggregate = _eventStoreRepository.GetAggregate(request.Identifier);
    //
    //         var events = aggregate.Consume(request);
    //
    //         _eventStoreRepository.SaveEvents(request.OrderId, events);
    //
    //
    //         foreach (var eventum in events)
    //         {
    //             aggregate.Apply(eventum);
    //         }
    //         _eventStoreRepository.SaveAggregate(aggregate);
    //
    //         foreach (var eventum in events)
    //         {
    //             await _eventPublisher.Publish(eventum, cancellationToken);
    //         }
    //         
    //         return Unit.Value;
    //     }
    // }
}