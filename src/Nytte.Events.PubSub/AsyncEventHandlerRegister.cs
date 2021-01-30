using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nytte.PubSub
{
    public class AsyncEventHandlerRegister : IAsyncEventHandlerRegister
    {
        public IReadOnlyList<Func<T, Task>> GetAsyncEventHandlers<T>() where T : IAsyncPubSubEvent
        {
            throw new NotImplementedException();
        }

        public void RegisterAsyncEventHandler<T>(Func<T, Task> eventHandler) where T : IAsyncPubSubEvent
        {
            throw new NotImplementedException();
        }

        public void DeregisterAsyncEventHandler<T>(Func<T, Task> eventHandler) where T : IAsyncPubSubEvent
        {
            throw new NotImplementedException();
        }
    }
}