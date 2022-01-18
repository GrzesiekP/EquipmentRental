using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using MediatR;
using Orders.Commands;

namespace Orders.CommandHandlers
{
    public class NotifyClientCommandHandler : ICommandHandler<NotifyClient>
    {
        public Task<Unit> Handle(NotifyClient command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Hey, Client! You're order {command.OrderId} is approved.");
            
            return Task.FromResult(Unit.Value);
        }
    }
}