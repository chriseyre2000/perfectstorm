using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectStorm.CodeGenLibrary
{
    public interface IPluginInit
    {
        void Init(IConfiguration config);
    }
}
