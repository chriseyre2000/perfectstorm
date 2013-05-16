namespace PerfectStorm.PubSub
{
    public interface IInterestMonitorMessage : IMessage
    {
        int Count { get; }
    }
}
