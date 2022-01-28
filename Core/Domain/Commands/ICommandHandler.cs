using MediatR;

namespace Core.Domain.Commands
{
    public interface ICommandHandler<in T>: IRequestHandler<T> where T : ICommand
    {
        
    }
}