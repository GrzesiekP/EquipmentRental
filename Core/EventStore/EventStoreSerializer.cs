using System.Text;
using Core.Domain;
using EventStore.Client;
using Newtonsoft.Json;

namespace Core.EventStore
{
    public static class EventStoreSerializer
    {
        public static EventData ToJsonEventData(this IEvent @event)
        {
            return new EventData(
                Uuid.NewUuid(),
                EventTypeMapper.ToName(@event.GetType()),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { }))
            );
        }
    }
}