using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Nytte.PubSub
{
    public class EventManager : IEventManager
    {
        private readonly IEventHandlerRegister _eventHandlerRegister;
        private readonly IAsyncEventHandlerRegister _asyncEventHandlerRegister;

        public EventManager(IEventHandlerRegister eventHandlerRegister, IAsyncEventHandlerRegister asyncEventHandlerRegister)
        {
            _eventHandlerRegister = eventHandlerRegister;
            _asyncEventHandlerRegister = asyncEventHandlerRegister;
        }

        public void Publish<T>([NotNull] T @event) where T : IPubSubEvent
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync<T>([NotNull] T @event) where T : IAsyncPubSubEvent
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>([NotNull] Action<T> eventHandler) where T : IPubSubEvent
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>([NotNull] Func<T, Task> eventHandler) where T : IAsyncPubSubEvent
        {
            throw new NotImplementedException();
        }
    }
}