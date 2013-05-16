using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NUnit.Framework;

namespace PerfectStorm.CodeGenTest
{
    public static class TransformTestHelper
    {
        public static void StringStreamCompare(Stream msExpected, Stream msActual, string message)
        {
            msExpected.Position = 0;
            msActual.Position = 0;
            StreamReader expectedReader = new StreamReader(msExpected);
            StreamReader actualReader = new StreamReader(msActual);
            string expected = expectedReader.ReadToEnd();
            string actual = actualReader.ReadToEnd();
            Assert.AreEqual(expected, actual, message);
        }
        
        public static void StreamCompare(Stream msExpected, Stream msActual, string message)
        {
            if ((msExpected == null) && (msActual != null))
            {
                Assert.Fail("Expected Stream is null while Actual Stream is not. " + message);
            }

            if ((msExpected != null) && (msActual == null))
            {
                Assert.Fail("Actual Stream is null while Expected Stream is not. " + message);
            }

            if ((msActual == null) && (msExpected == null))
            {
                return;
            }

            StringBuilder errorMessage = new StringBuilder();


            if (msActual.Length != msExpected.Length)
            {
                errorMessage.AppendFormat("Stream lengths differ Expected: {0} Actual: {1}",
                    msExpected.Length, msActual.Length);
            }

            int expectedItem;
            int actualItem;

            StreamReader expectedReader = new StreamReader(msExpected);
            StreamReader actualReader = new StreamReader(msActual);

            long minLength = Math.Min(msExpected.Length, msActual.Length);
            for (long i = 0; i < minLength; i++)
            {
                expectedItem = expectedReader.Read();
                actualItem = actualReader.Read();
                if (expectedItem != actualItem)
                {
                    errorMessage.AppendFormat("Streams differ at position {0} ", i);
                    errorMessage.AppendLine();

                    msActual.Position = i - 1;
                    msExpected.Position = i - 1;
                    char[] actual = new char[100];
                    char[] expected = new char[100];
                    expectedReader.Read(expected, 0, 100);
                    actualReader.Read(actual, 0, 100);

                    errorMessage.AppendLine("Expected block:");
                    errorMessage.AppendFormat("{0}", new string(expected));
                    errorMessage.AppendLine();
                    errorMessage.AppendLine("Actual block:");
                    errorMessage.AppendFormat("{0}", new string(actual));

                    break;
                }
            }


            if (errorMessage.Length > 0)
            {
                errorMessage.AppendLine(message);
                Assert.Fail(errorMessage.ToString());
            }
        }

    }
}
