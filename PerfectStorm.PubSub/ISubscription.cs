namespace PerfectStorm.PubSub
{   
    public interface ISubscription
    {
        /// <summary>
        /// This is used to register an interest in a subject.
        /// Response will be through a distinct interface (see ISubscriptionClient for an example).
        /// </summary>
        /// <param name="username">Unique identifier of sender.</param>
        /// <param name="subject">Identifier of subject.</param>
        void AddInterest(string username, string subject);
        /// <summary>
        /// This is used to measure the level of interest in a given subject.
        /// It will be updated through the same channels as the main interest.
        /// This allows clients to know that a server is available and a server to know what clients need.
        /// A client will receive an initial status value per specific subject with a non-zero subscription.
        /// </summary>
        /// <param name="username">Unique identifier of sender.</param>
        /// <param name="subject">Regex of subject to be monitored.</param>
        void AddInterestMonitor(string username, string subject);
        /// <summary>
        /// Expresses that the client no longer wants to receive messages on this subject.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="subject"></param>
        void RemoveInterest(string username, string subject);
        /// <summary>
        /// Expresses that the client no longer wants to receive messages on this subject.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="subject"></param>
        void RemoveInterestMonitor(string username, string subject);
    }
}
