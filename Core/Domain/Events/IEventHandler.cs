﻿using MediatR;

namespace Core.Domain.Events
{
    public interface IEventHandler<in T>: INotificationHandler<T> where T : IEvent
    {
    }
}