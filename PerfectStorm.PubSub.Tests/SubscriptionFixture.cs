using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PerfectStorm.PubSub.Broker;
using PerfectStorm.PubSub.Message;

namespace PerfectStorm.PubSub.Tests
{
    [TestFixture]
    public class SubscriptionFixture
    {
        PubSubBroker broker;
        Queue<IMessage> messageQueue;
        Queue<IMessage> interestQueue;

        [SetUp]
        public void SetUp()
        {
            broker = new PubSubBroker();
            messageQueue = new Queue<IMessage>();
            interestQueue = new Queue<IMessage>();
        }

        #region Helper

        private void PublishMessage(string Subject)
        {
            broker.Publish(new PublicationMessage { Subject = "Hello", Message = null });
        }

        #endregion Helper

        [Test]
        public void Publication_Without_Register_No_Messages()
        {
            //Act
            PublishMessage("Hello");
            
            //Assert
            Assert.That(messageQueue.Count, Is.EqualTo(0));
        }

        [Test]
        public void Publication_With_Register_And_Subscription_One_Messsage()
        {
            //Arrange
            broker.Register("first", messageQueue);
            broker.AddInterest("first", "Hello");

            //Act
            PublishMessage("Hello");
            
            //Assert
            Assert.That(messageQueue.Count, Is.EqualTo(1), "Q should have one message");
            IMessage message = messageQueue.Dequeue();
            Assert.That(message, Is.Not.Null, "Message should not be null");
            Assert.That(message.Subject, Is.EqualTo("Hello"));
        }

        [Test]
        public void InterestMonitor_Gets_Count_When_InterestAdded_After_Monitor()
        {
            //Arrange
            broker.Register("first", messageQueue);
            broker.Register("count", interestQueue);
            broker.AddInterestMonitor("count", ".*");

            //Act
            broker.AddInterest("first", "Hello");

            //Assert
            Assert.That(interestQueue.Count, Is.EqualTo(1));
            Assert.That(messageQueue.Count, Is.EqualTo(0));

            IInterestMonitorMessage imm = interestQueue.Dequeue() as IInterestMonitorMessage;
            Assert.That(imm, Is.Not.Null);
            Assert.That(imm.Subject, Is.EqualTo("Hello"));
            Assert.That(imm.Count, Is.EqualTo(1));
        }

        [Test]
        public void InterestMonitor_Gets_Count_When_Monitor_Added_After_Interest()
        {
            //Arrange
            broker.Register("first", messageQueue);
            broker.Register("count", interestQueue);
            broker.AddInterest("first", "Hello");

            //Act            
            broker.AddInterestMonitor("count", ".*");

            //Assert
            Assert.That(interestQueue.Count, Is.EqualTo(1));
            Assert.That(messageQueue.Count, Is.EqualTo(0));

            IInterestMonitorMessage imm = interestQueue.Dequeue() as IInterestMonitorMessage;
            Assert.That(imm, Is.Not.Null);
            Assert.That(imm.Subject, Is.EqualTo("Hello"));
            Assert.That(imm.Count, Is.EqualTo(1));
        }
    }
}
