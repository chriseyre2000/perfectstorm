using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PerfectStorm.Model;

namespace Model.Tests
{
    [TestFixture]
    public class GenericSmartTypeTests
    {
        [Test]
        public void Double()
        {
            SmartType<double> st = new SmartType<double>();
            st.setString("50.5");
            Assert.IsTrue(st.IsValid, "The generic parse has not worked!");
        }

        [Test]
        public void InvalidDouble()
        {
            SmartType<double> st = new SmartType<double>();
            st.setString("xxx");
            Assert.IsFalse(st.IsValid, "The generic parse has not worked!");
        }

        [Test]
        public void Int64()
        {
            SmartType<Int64> st = new SmartType<long>();
            st.setString("1000000000");
            Assert.IsTrue(st.IsValid, "The generic parse has not worked!");
        }

        [Test]
        public void DateTimeTest()
        {
            SmartType<DateTime> st = new SmartType<DateTime>();
            st.setString("2007/12/25");
            Assert.IsTrue(st.IsValid, "The generic parse has not worked!");
            Console.WriteLine(st.Value.ToString());
        }
    }
}
