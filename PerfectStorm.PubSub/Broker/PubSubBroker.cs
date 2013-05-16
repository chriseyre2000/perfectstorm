using System.Collections.Generic;
using PerfectStorm.PubSub.Message;

namespace PerfectStorm.PubSub.Broker
{
    public class PubSubBroker : PubSubBrokerBase
    {
        private object clientListLock = new object();
        private Dictionary<string, Queue<IMessage>> clientList = new Dictionary<string, Queue<IMessage>>();

        public void Register(string username, Queue<IMessage> messageQueue)
        {
            lock (clientListLock)
            {
                if (!clientList.ContainsKey(username))
                {
                    clientList.Add(username, messageQueue);
                }
            }
        }

        public void UnRegister(string username)
        {
            lock (clientListLock)
            {
                if (clientList.ContainsKey(username))
                {
                    clientList.Remove(username);

                    if (interestSubjectByUser.ContainsKey(username))
                    {
                        List<string> subjects = interestSubjectByUser[username];
                        foreach (string subject in subjects)
                        {
                            RemoveInterest(username, subject);
                        }
                    }

                    if (interestMonitorsByUser.ContainsKey(username))
                    {
                        List<string> interestMonitors = interestMonitorsByUser[username];
                        foreach (string interestMonitor in interestMonitors)
                        {
                            RemoveInterestMonitor(username, interestMonitor);
                        }
                    }
                }
            }
        }

        protected override void PublishInterestMonitorToUsers(IEnumerable<string> users, string subject, int count)
        {
            lock (clientListLock)
            {
                foreach (string user in users)
                {
                    if (clientList.ContainsKey(user))
                    {
                        clientList[user].Enqueue(new InterestMonitorMessage { Subject = subject, Count = count });
                    }
                }
            }
        }
        
        protected override void PublishToUsers(IEnumerable<string> users, IMessage message) 
        {
            foreach (string username in users)
            {
                lock (clientListLock)
                {
                    clientList[username].Enqueue(message);
                }
            }
        }
    }
}
