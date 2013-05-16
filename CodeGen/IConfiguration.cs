using System;
using System.Xml;

namespace PerfectStorm.CodeGen
{
    /// <summary>
    /// 
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        string GetConfigData(string section, string xpath);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        bool HasConfigItem(string section, string xpath);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        XmlNode GetConfigSection(string section);
    }
}
