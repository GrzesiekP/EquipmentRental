using System;
using Core.Domain.Aggregates;
using Core.Domain.Commands;
using Core.Domain.Events;

namespace Core.Logging
{
    public class AggregateLogger<T, TId> where T : Aggregate<TId>
    {
        private TId _identifier;
        
        public AggregateLogger(TId identifier)
        {
            _identifier = identifier;
        }
        
        private static string AggregateTypeName => typeof(T).Name;

        public void LogCommand(ICommand command) => Log($"Handling {command.GetType().Name}");
        
        public void LogApplyMethod(IEvent e)
        {
            Log($"Applying {e.GetType().Name}. Id:{_identifier.ToString()}");
        }

        public void LogPublishEvent(IEvent e)
        {
            Log($"Publishing {e.GetType().Name}");
        }

        public void Log(string message)
        {
            Console.WriteLine($"{AggregateTypeName}: {message}");
        }
    }
}