using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PerfectStorm.PubSub.Broker
{
    /// <summary>
    /// This is the base class for a simple pub-sub system.
    /// </summary>
    public abstract class PubSubBrokerBase : ISubscription, IPublicationServer
    {
        private Queue<IMessage> messages = new Queue<IMessage>();

        private object interestsLock = new object();

        private Dictionary<string, List<string>> interestUsersBySubject = new Dictionary<string, List<string>>();
        protected Dictionary<string, List<string>> interestSubjectByUser = new Dictionary<string, List<string>>();

        public void AddInterest(string username, string subject)
        {
            if (username == null) throw new ArgumentNullException(username);
            if (subject == null) throw new ArgumentNullException(subject);

            int count = 0;
            lock (interestsLock)
            {
                if (interestUsersBySubject.ContainsKey(subject))
                {
                    List<string> interestedUsers = interestUsersBySubject[subject];
                    if (!interestedUsers.Contains(username))
                    {
                        interestedUsers.Add(username);
                        count = interestedUsers.Count;
                    }
                }
                else
                {
                    interestUsersBySubject.Add(subject, new List<string> { username });
                    count = 1;
                }

                if (interestSubjectByUser.ContainsKey(username))
                {
                    List<string> subjects = interestSubjectByUser[username];
                    if (!subjects.Contains(subject))
                    {
                        subjects.Add(subject);
                    }
                }
                else
                {
                    interestSubjectByUser.Add(username, new List<string> { subject });
                }
            }
            if (count > 0)
            {
                PublishInterestMonitor(subject, count);
            }
        }

        public void RemoveInterest(string username, string subject)
        {
            if (username == null) throw new ArgumentNullException(username);
            if (subject == null) throw new ArgumentNullException(subject);

            bool changed = false;
            int count = 0;

            lock (interestsLock)
            {
                if (interestUsersBySubject.ContainsKey(subject))
                {
                    List<string> interestedUsers = interestUsersBySubject[subject];
                    if (interestedUsers.Contains(username))
                    {
                        interestedUsers.Remove(username);
                        changed = true;
                        count = interestedUsers.Count;
                        if (count == 0)
                        {
                            interestUsersBySubject.Remove(subject);
                        }
                    }

                    List<string> subjects = interestSubjectByUser[username];
                    if (subjects.Contains(subject))
                    {
                        subjects.Remove(subject);
                        if (subjects.Count == 0)
                        {
                            interestSubjectByUser.Remove(username);
                        }
                    }

                }
            }

            if (changed)
            {
                PublishInterestMonitor(subject, count);
            }

        }

        private void PublishInterestMonitor(string subject, int count)
        {
            List<string> usersToNotify = new List<string>();
            lock (interestMonitorLock)
            {
                foreach (string interestMonitor in regexByInterestMonitor.Keys)
                {
                    if (regexByInterestMonitor[interestMonitor].Match(subject) != null)
                    {
                        foreach (string user in usersbyInterestMonitor[interestMonitor])
                        {
                            if (!usersToNotify.Contains(user))
                            {
                                usersToNotify.Add(user);
                            }
                        }
                    }
                }
            }

            PublishInterestMonitorToUsers(usersToNotify, subject, count);

        }

        protected abstract void PublishInterestMonitorToUsers(IEnumerable<string> users, string subject, int count);
        protected abstract void PublishToUsers(IEnumerable<string> users, IMessage message);

        object interestMonitorLock = new object();
        Dictionary<string, List<string>> usersbyInterestMonitor = new Dictionary<string, List<string>>();
        protected Dictionary<string, List<string>> interestMonitorsByUser = new Dictionary<string, List<string>>();
        Dictionary<string, Regex> regexByInterestMonitor = new Dictionary<string, Regex>();

        public void AddInterestMonitor(string username, string subject)
        {
            if (username == null) throw new ArgumentNullException(username);
            if (subject == null) throw new ArgumentNullException(subject);

            lock (interestMonitorLock)
            {
                if (usersbyInterestMonitor.ContainsKey(subject))
                {
                    List<string> users = usersbyInterestMonitor[subject];
                    if (!users.Contains(username))
                    {
                        users.Add(username);
                    }
                }
                else
                {
                    usersbyInterestMonitor.Add(subject, new List<string> { username });
                    regexByInterestMonitor.Add(subject, new Regex(subject));
                }

                if (interestMonitorsByUser.ContainsKey(username))
                {
                    List<string> subjects = interestMonitorsByUser[username];
                    if (!subjects.Contains(subject))
                    {
                        subjects.Add(subject);
                    }
                }
                else
                {
                    interestMonitorsByUser.Add(username, new List<string> { subject });
                }
            }

            ReportInitialInterest(username, subject);
        }

        private void ReportInitialInterest(string username, string interest)
        {
            lock (interestsLock)
            {
                Regex re = regexByInterestMonitor[interest];

                foreach (string subject in interestUsersBySubject.Keys)
                {
                    if (re.Match(subject) != null)
                    {
                        PublishInterestMonitorToUsers(new[] { username }, subject, interestUsersBySubject[subject].Count);
                    }
                }
            }
        }

        public void RemoveInterestMonitor(string username, string subject)
        {
            lock (interestMonitorLock)
            {
                if (usersbyInterestMonitor.ContainsKey(subject))
                {
                    List<string> users = usersbyInterestMonitor[subject];
                    if (users.Contains(username))
                    {
                        users.Remove(username);
                        if (users.Count == 0)
                        {
                            usersbyInterestMonitor.Remove(subject);
                            regexByInterestMonitor.Remove(subject);
                        }
                    }
                }

                if (interestMonitorsByUser.ContainsKey(username))
                {
                    List<string> interestMonitors = interestMonitorsByUser[username];
                    if (interestMonitors.Contains(subject))
                    {
                        interestMonitors.Remove(username);
                        if (interestMonitors.Count == 0)
                        {
                            interestMonitorsByUser.Remove(username);
                        }
                    }
                }
            }
        }

        public void Publish(IMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (message.Subject == null) throw new ArgumentNullException("message.subject");

            List<string> usersToPublish = new List<string>();

            if (interestUsersBySubject.ContainsKey(message.Subject))
            {
                usersToPublish = interestUsersBySubject[message.Subject];
            }
            PublishToUsers(usersToPublish, message);
        }
    }
}
