using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace PerfectStorm.CodeGenLibrary
{
    /// <summary>
    /// It did not seem right that the XsltExtension class should have state
    /// so it has been abstracted to a class whose responsibility is for the
    /// state.
    /// </summary>
    public class TransformContext
    {
        private IConfiguration _config;
        private Dictionary<string, string> _variables = null;

        private string _generatedFile;

        private bool _targetExists;

        /// <summary>
        /// Used to indicate that the target exists.
        /// </summary>
        public bool TargetExists
        {
            set { _targetExists = value; }
            get { return _targetExists; }
        }

        public Dictionary<string, string> Variables
        {
            get { return _variables; }
        }

        /// <summary>
        /// Used to set the name of the generated file.
        /// </summary>
        public string GeneratedFile
        {
            set 
            {
                if (value != _generatedFile)
                {
                    _generatedFile = value;
                    _targetExists = File.Exists(_generatedFile);
                }
            }
            get { return _generatedFile; }
        }

        /*

        /// <summary>
        /// The theory behind regex variables is that a regex expression can be used to populate a variable.
        /// This variable can then be reinserted into the document.
        /// This was written to permit vcs headers to be maintained across regeneration of the file.
        /// However the implementation is generic.
        /// </summary>
        private void LoadRegexVariables()
        {
            // Initialise the variables.
            XmlNode node = (XmlNode)ConfigurationManager.GetSection("variables");
            if (node != null)
            {
                if (node.HasChildNodes)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.LocalName == "regexVariable")
                        {
                            string pattern = child.InnerText;
                            string keyName = child.Attributes["name"].Value;
                            Regex reg = new Regex(pattern, RegexOptions.Singleline);
                            Match m = reg.Match(File.ReadAllText(_generatedFile));
                            
                            if (m.Success)
                            {
                                if (_variables.ContainsKey(keyName))
                                {
                                    _variables[keyName] = m.Groups[0].Value;
                                }
                                else
                                {
                                    _variables.Add(keyName, m.Groups[0].Value);
                                }
                            }
                                  
                        }
                    }
                }
            }
        
        }
        */

        public TransformContext(IConfiguration config) : base()
        {
            _config = config;
            _variables = new Dictionary<string, string>();

            // Initialise the variables.
            XmlNode node = _config.GetConfigSection("variables");
            if (node != null)
            {
                if (node.HasChildNodes)
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.LocalName == "variable")
                        {
                            _variables.Add(child.Attributes["name"].Value, child.InnerXml);
                        }
                    }
                }
            }
        
        }
    }
}
