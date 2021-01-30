using System;
using System.Threading.Tasks;

namespace Nytte.PubSub
{
    public interface IAsyncEventHandlerRegister
    {
        void RegisterAsyncEventHandler<T>(Action<T, Task> eventHandler);
    }
}