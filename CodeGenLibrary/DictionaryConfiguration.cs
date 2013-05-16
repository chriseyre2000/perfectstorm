using System;
using System.Collections.Generic;
using System.Xml;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class DictionaryConfiguration : IConfiguration
    {
        private Dictionary<string, string> _configData = null;

        /// <summary>
        /// 
        /// </summary>
        public DictionaryConfiguration()
            : base()
        {
            _configData = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _configData.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            _configData[key] = value;
        }

        #region IConfiguration Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string GetConfigData(string section, string xpath)
        {
            string result = "";

            if (_configData.ContainsKey(section))
            {
                string xml = _configData[section];

                if (xml.Length > 0)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xml);
                    XmlNode xNode = xDoc.FirstChild.SelectSingleNode(xpath);
                    if (xNode != null)
                    {
                        result = xNode.Value;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public bool HasConfigItem(string section, string xpath)
        {
            bool result = false;

            if (_configData.ContainsKey(section))
            {
                string xml = _configData[section];

                if (xml.Length > 0)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(xml);
                    XmlNode xNode = xDoc.FirstChild.SelectSingleNode(xpath);
                    if (xNode != null)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public XmlNode GetConfigSection(string section)
        {
            XmlNode result = new XmlDocument();
            if (_configData.ContainsKey(section))
            {
                string xml = _configData[section];

                if (xml.Length > 0)
                {
                    XmlDocument xDoc = (XmlDocument)result;
                    xDoc.LoadXml(xml);
                    result = xDoc.FirstChild;
                }
            }

            return result;
        }

        #endregion
    }
}
