using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Nytte.PubSub
{
    public interface IAsyncEventHandlerRegister
    {
        IReadOnlyList<Func<T, Task>> GetAsyncEventHandlers<T>() where T : IAsyncPubSubEvent;
        void RegisterAsyncEventHandler<T>(Func<T, Task> eventHandler) where T : IAsyncPubSubEvent;
        void DeregisterAsyncEventHandler<T>(Func<T, Task> eventHandler) where T : IAsyncPubSubEvent;
    }
}