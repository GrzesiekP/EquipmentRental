﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using MediatR;
using Orders.Commands;
using Orders.Events;

namespace Orders.EventHandlers
{
    public class OrderSubmittedEventHandler : IEventHandler<OrderSubmitted>
    {
        private readonly IMediator _mediator;
        
        public OrderSubmittedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(OrderSubmitted eOrderSubmitted, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(OrderSubmitted)}:{eOrderSubmitted.OrderId}");
            
            var requestApproval = new RequestApproval(eOrderSubmitted.OrderId);

            return _mediator.Send(requestApproval, cancellationToken);
        }
    }
}