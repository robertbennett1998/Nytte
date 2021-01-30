using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nytte.PubSub
{
    public class EventHandlerRegister : IEventHandlerRegister
    {
        private readonly Dictionary<Type, List<object>> _eventHandlers;

        public EventHandlerRegister()
        {
            _eventHandlers = new Dictionary<Type, List<object>>();
        }

        public IReadOnlyList<Action<T>> GetEventHandlers<T>() where T : IPubSubEvent
        {
            return _eventHandlers.ContainsKey(typeof(T)) ? _eventHandlers[typeof(T)].Cast<Action<T>>().ToImmutableList() : new List<Action<T>>();
        }
        
        public void RegisterEventHandler<T>(Action<T> eventHandler) where T : IPubSubEvent
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

        public void DeregisterEventHandler<T>(Action<T> eventHandler) where T : IPubSubEvent
        {
            if (_eventHandlers.ContainsKey(typeof(T)))
            {
                _eventHandlers[typeof(T)].Remove(eventHandler);
            }
        }
    }
}