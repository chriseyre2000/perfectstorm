namespace PerfectStorm.PubSub
{
    public interface IPublicationServer
    {
        void Publish(IMessage message);
    }
}
