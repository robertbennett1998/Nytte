using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Nytte.PubSub
{
    public class AsyncEventHandlerRegister : IAsyncEventHandlerRegister
    {
        private readonly Dictionary<Type, List<object>> _eventHandlers;

        public AsyncEventHandlerRegister()
        {
            _eventHandlers = new Dictionary<Type, List<object>>();
        }
        
        public IReadOnlyList<Func<T, Task>> GetAsyncEventHandlers<T>() where T : IAsyncPubSubEvent
        {
            return _eventHandlers.ContainsKey(typeof(T)) 
                ? _eventHandlers[typeof(T)].Cast<Func<T, Task>>().ToImmutableList() 
                : new List<Func<T, Task>>();
        }

        public void RegisterAsyncEventHandler<T>(Func<T, Task> eventHandler) where T : IAsyncPubSubEvent
        {
            if (_eventHandlers.ContainsKey(typeof(T)))
            {
                _eventHandlers[typeof(T)].Add(eventHandler as object);
            }
            else
            {
                var init = new[] {eventHandler as object};
                _eventHandlers[typeof(T)] = new List<object>(init);
            }
        }

        public void DeregisterAsyncEventHandler<T>(Func<T, Task> eventHandler) where T : IAsyncPubSubEvent
        {
            if (_eventHandlers.ContainsKey(typeof(T)))
            {
                _eventHandlers[typeof(T)].Remove(eventHandler);
            }
        }
    }
}