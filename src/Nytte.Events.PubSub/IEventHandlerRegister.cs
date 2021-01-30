using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nytte.PubSub
{
    public interface IEventHandlerRegister
    {
        IReadOnlyList<Action<T>> GetEventHandlers<T>() where T : IPubSubEvent;
        void RegisterEventHandler<T>(Action<T> eventHandler) where T : IPubSubEvent;
        void DeregisterEventHandler<T>(Action<T> eventHandler) where T : IPubSubEvent;
    }
}