using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace PerfectStorm.CodeGen
{
    /// <summary>
    /// This allows the config section to be read as an XML node.
    /// </summary>
    public class XmlNodeConfigSection : IConfigurationSectionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public object Create(
             object parent,
             object configContext,
             System.Xml.XmlNode section)
        {
            // This was based upon an idea that I got from:
            // (http://alt.pluralsight.com/wiki/default.aspx/Craig/XmlSerializerSectionHandler.html)
            return section;
        }
    }

}
