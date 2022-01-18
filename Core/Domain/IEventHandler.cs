using MediatR;

namespace Core.Domain
{
    public interface IEventHandler<in T>: INotificationHandler<T> where T : IEvent
    {
    }
}