using MediatR;

namespace Core
{
    public interface ICommandHandler<in T>: IRequestHandler<T> where T : ICommand
    {
        
    }
}