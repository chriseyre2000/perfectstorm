using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectStorm.CodeGen
{
    public interface IPluginInit
    {
        void Init(IConfiguration config);
    }
}
