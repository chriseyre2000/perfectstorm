using System;
using System.IO;
using System.Xml.Xsl;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// This is a thinning interface. It takes the interface of a complex class 
    /// and reduces it to the used interface.
    /// </summary>
    public interface IXlsTransformer
    {
        void Load(string transform);

        void Transform(string source, XsltArgumentList args, Stream stream); 
    }
}
