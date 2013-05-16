using System;
using System.Collections.Generic;
using System.Text;

using PerfectStorm.CodeGenLibrary;

using NUnit.Framework;


namespace PerfectStorm.CodeGenTest
{
    [TestFixture]
    public class NullLoggerTest
    {
        /// <summary>
        /// This is all I can think of to test with a null logger!
        /// </summary>
        /// <remarks>How do you implement a negative test in NUnit?</remarks>
        [Test]
        public void TestCreate()
        {
            ILogger logger = new NullLogger();
            logger.Log("This is a test");
        }
    }
}
