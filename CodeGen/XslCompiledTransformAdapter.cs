using System;
using System.IO;
using System.Xml.Xsl;

namespace PerfectStorm.CodeGen
{
    public class XslCompiledTransformAdapter : IXlsTransformer
    {
        XslCompiledTransform _transform;
        
        public XslCompiledTransformAdapter()
        { 
            _transform = new XslCompiledTransform();
        }

        #region IXlsTransform Members

        public void Load(string transform)
        {
            _transform.Load(transform);
        }

        public void Transform(string source, XsltArgumentList args, Stream stream)
        {
            _transform.Transform(source, args, stream);
        }

        #endregion
    }
}
