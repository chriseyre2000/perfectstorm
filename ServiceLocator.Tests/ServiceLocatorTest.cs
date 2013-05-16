using System;
using System.Collections.Generic;
using System.Text;
using PerfectStorm;
using NUnit.Framework;

namespace ServiceLocatorTests
{
    [TestFixture]
    public class ServiceLocatorTest
    {
        private interface IFoo
        {
            int Hello();
        }

        private class Foo : IFoo
        {
            public int Hello() { return 42; }
        }
        
        [Test]
        public void TestMe()
        {
            ServiceLocator.Clear();
            ServiceLocator.Register<Foo>();
            Foo f = ServiceLocator.CreateInstance<Foo>("ServiceLocatorTests.ServiceLocatorTest+Foo");
            Assert.AreEqual(42, f.Hello());
        }

        [Test]
        public void TestMeViaInterface()
        {
            ServiceLocator.Clear();
            ServiceLocator.Register<Foo>();
            IFoo f = ServiceLocator.CreateInstance<IFoo>("ServiceLocatorTests.ServiceLocatorTest+Foo");
            Assert.AreEqual(42, f.Hello());
        }

    }
}
