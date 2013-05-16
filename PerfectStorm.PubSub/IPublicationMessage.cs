namespace PerfectStorm.PubSub
{
    public interface IPublicationMessage : IMessage
    {
        string Message { get; }
    }
}
