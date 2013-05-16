using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using PerfectStorm.CodeGenLibrary;

using NUnit.Framework;

namespace PerfectStorm.CodeGenTest
{
    [TestFixture]
    public class TransformFilterTest
    {
        [Test]
        public void TextReplace()
        {
            ITransformFilter filter = new HeaderCopyFilter();
            MemoryStream inputMemoryStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(inputMemoryStream);
            writer.WriteLine("/***");
            writer.WriteLine("Test header block");
            writer.WriteLine("***/");
            writer.Write("Body Text");
            writer.Flush();

            MemoryStream priorMemoryStream = new MemoryStream();
            writer = new StreamWriter(priorMemoryStream);
            writer.WriteLine("/***");
            writer.WriteLine("VCS Header Block");
            writer.WriteLine("***/");
            writer.Flush();

            MemoryStream expectedMemoryStream = new MemoryStream();
            writer = new StreamWriter(expectedMemoryStream);
            writer.WriteLine("/***");
            writer.WriteLine("VCS Header Block");
            writer.WriteLine("***/");
            writer.Write("Body Text");
            writer.Flush();

            MemoryStream resultMemoryStream = filter.AfterTransform(null, null, priorMemoryStream, inputMemoryStream, null, null, null);
            TransformTestHelper.StringStreamCompare(expectedMemoryStream, resultMemoryStream, "Actual check");
        }
    }
}
