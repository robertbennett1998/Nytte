namespace Nytte.PubSub
{
    public interface IPubSubEvent
    {
        
    }

    public interface IPubSubEvent<out T> : IPubSubEvent
    {
        T Args { get; }
    }
}