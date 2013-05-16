using System;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// This allows custom functions to be added to the transform.
    /// The following attribute needs to be added to the stylesheet.
    /// xmlns:PerfectStorm="urn:PerfectStorm"
    /// </summary>
    public class XsltExtension
    {
        /// <summary>
        /// This defines the urn that is registered.
        /// </summary>
        public const string EXTENSION_URN = "urn:PerfectStorm";

        private TransformContext _context;

        /// <summary>
        /// 
        /// </summary>
        public XsltExtension(TransformContext context) : base()
        {
            _context = context;
        }

        /// <summary>
        /// Changes the first character of the supplied string to upper case.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>Converted string.</returns>
        /// <example><xsl:value-of select="PerfectStorm:CamelToPascal(@name)" /></example>
        public string CamelToPascal(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return value.Substring(0, 1).ToUpper() + value.Substring(1);
            }
        }

        /// <summary>
        /// Allows the code to report the generated filename.
        /// </summary>
        /// <returns></returns>
        /// <example><xsl:value-of select="PerfectStorm:Filename()" /></example>
        public string Filename()
        {
            return _context.GeneratedFile;
        }

        /// <summary>
        /// Indicates that the file was regenerated.
        /// </summary>
        /// <returns></returns>
        /// <example><xsl:value-of select="PerfectStorm:FileRegenerated()" /></example>
        public string FileRegenerated()
        {
            return _context.TargetExists ? "TRUE" : "FALSE";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <example><xsl:value-of select="PerfectStorm:GetDateTime()" /></example>
        public string GetDateTime()
        {
            return DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        /// <summary>
        /// Returns a named variable.
        /// Variables can be initialised in the App.Config file under the variables node.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <example><xsl:value-of select="PerfectStorm:GetVariable(@name)" /></example>
        public string GetVariable(string name)
        {
            if (_context.Variables.ContainsKey(name))
            {
                return _context.Variables[name];
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Converts the supplied string to lower case.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>Converted string</returns>
        /// <example><xsl:value-of select="PerfectStorm:LowerCase(@name)" /></example>
        public string LowerCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return value.ToLower();
            }
        }

        /// <summary>
        /// Returns the same guid within a session for the given key.
        /// </summary>
        /// <param name="name">name of key</param>
        /// <returns></returns>
        /// <example><xsl:value-of select="PerfectStorm:NamedGuid(@Name)" /></example>
        public string NamedGuid(string name)
        {
            if (!_context.Variables.ContainsKey(name))
            {
                _context.Variables.Add(name, NewGuid());
            }

            return _context.Variables[name];
        }

        /// <summary>
        /// Returns a new guid as a string.
        /// </summary>
        /// <returns></returns>
        /// <example><xsl:value-of select="PerfectStorm:NewGuid()" /></example>
        public string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Changes the first character of the supplied string to lower case.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>Converted string.</returns>
        /// <example><xsl:value-of select="PerfectStorm:PascalToCamel(@name)" /></example>
        public string PascalToCamel(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return value.Substring(0,1).ToLower() + value.Substring(1);
            }
        }

        /// <summary>
        /// Converts the string to upper case.
        /// </summary>
        /// <param name="value">String to convert.</param>
        /// <returns>Converted string.</returns>
        /// <example><xsl:value-of select="PerfectStorm:UpperCase(@name)" /></example>
        public string UpperCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return value.ToUpper();
            }
        }
    }
}
