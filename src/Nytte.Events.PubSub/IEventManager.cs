using System;
using System.Threading.Tasks;

namespace Nytte.PubSub
{
    public interface IEventManager
    {
        void Publish<T>(T @event) where T : IPubSubEvent;
        Task PublishAsync<T>(T @event) where T : IPubSubEvent;
        void Subscribe<T>(Action<T> eventHandler) where T : IPubSubEvent;
        Task SubscribeAsync<T>(Func<T, Task> eventHandler) where T : IPubSubEvent;
    }
}