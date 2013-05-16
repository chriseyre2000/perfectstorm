using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// This class was written to allow the mono version to work.
    /// Mono does not yet implement XslCompiledTransform so I have
    /// provided an adapter to allow the two classes to conform to 
    /// the same interface.
    /// </summary>
    public class XslTransformAdapter : IXlsTransformer
    {
// The following may be obselete but I want to use it anyway
        #pragma warning disable 0618
        private XslTransform _transform;
        #pragma warning restore 0618

        /// <summary>
        /// Minimal constructor.
        /// </summary>
        public XslTransformAdapter() : base()
        {
            // The following may be obselete but I want to use it anyway
            #pragma warning disable 0618
            _transform = new XslTransform();
            #pragma warning restore 0618
        }

        #region IXlsTransformer Members

        /// <summary>
        /// Trivial load.
        /// </summary>
        /// <param name="transform"></param>
        public void Load(string transform)
        {
            _transform.Load(transform);
        }

        /// <summary>
        /// Minor change to the transform from the XslCompiledVersion.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <param name="stream"></param>
        public void Transform(string source, XsltArgumentList args, Stream stream)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(source);
            _transform.Transform(xDoc, args, stream);
        }

        #endregion
    }
}
