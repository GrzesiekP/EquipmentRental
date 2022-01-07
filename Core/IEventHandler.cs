using MediatR;

namespace Core
{
    public interface IEventHandler<in T>: INotificationHandler<T> where T : IEvent
    {
    }
}