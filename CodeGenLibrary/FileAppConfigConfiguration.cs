using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace PerfectStorm.CodeGenLibrary
{
    public class FileAppConfigConfiguration : IConfiguration
    {

        private XmlDocument _document;

        public string GetConfigData(string section, string xpath)
        {
            XmlNode sectionNode = GetConfigSection(section);
            if (sectionNode == null) return string.Empty;
            return sectionNode.SelectSingleNode(xpath).Value;
        }

        public bool HasConfigItem(string section, string xpath)
        {
            XmlNode sectionNode = GetConfigSection(section);
            if (sectionNode == null) return false;
            XmlNode singleNode = sectionNode.SelectSingleNode(xpath);
            return singleNode != null;
        }

        public XmlNode GetConfigSection(string section)
        {
            return _document.SelectSingleNode( string.Format("//configuration/{0}", section));
        }

        public FileAppConfigConfiguration(string filename)
        {
            _document = new XmlDocument();
            _document.Load(File.OpenRead(filename));
        }
    }
}
