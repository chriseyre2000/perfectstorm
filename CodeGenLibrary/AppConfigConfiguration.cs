using System;
using System.Configuration;
using System.Xml;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfigConfiguration : IConfiguration
    {

        #region IConfiguration Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string GetConfigData(string section, string xpath)
        {
            string result = null;
            XmlNode node = GetConfigSection(section);
            if (node != null)
            {
                XmlNode configNode = node.SelectSingleNode(xpath);

                if (configNode != null)
                {
                    result = configNode.Value;
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

            XmlNode node = GetConfigSection(section);
            if (node != null)
            {
                if (node.SelectSingleNode(xpath) != null)
                {
                    result = true;
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
            return (XmlNode)ConfigurationManager.GetSection(section);
        }

        #endregion
    }
}
