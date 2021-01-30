namespace Nytte.PubSub
{
    public interface IAsyncPubSubEvent
    {
        
    }

    public interface IAsyncPubSubEvent<out T> : IAsyncPubSubEvent
    {
        T Args { get; }
    }
}