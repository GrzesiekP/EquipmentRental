using MediatR;

namespace Core.Domain
{
    public interface ICommandHandler<in T>: IRequestHandler<T> where T : ICommand
    {
        
    }
}