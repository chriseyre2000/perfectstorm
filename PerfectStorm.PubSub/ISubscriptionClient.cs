namespace PerfectStorm.PubSub
{
    public interface ISubscriptionClient
    {
        /// <summary>
        /// Reads a message from internal queue.
        /// It returns a null message when the queue is empty.
        /// </summary>
        /// <returns></returns>
        IMessage Read();
    }
}
