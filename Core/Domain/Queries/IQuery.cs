using MediatR;

namespace Core.Domain.Queries
{
    public interface IQuery<out TResponse>: IRequest<TResponse>
    {
        
    }
}