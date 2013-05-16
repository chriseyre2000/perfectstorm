using System;
using System.Collections.Generic;
using System.IO;

namespace PerfectStorm.CodeGenLibrary
{
    public interface ITransformOutput
    {
        void WriteOutput(string target, MemoryStream prevOutput, MemoryStream output);
    }
}
