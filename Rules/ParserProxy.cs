using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Workflow.Activities.Rules;

namespace PerfectStorm.Rules
{
    /// <summary>
    /// This is a proxy to the internal Parser class in the System.Workflow.Activities assembly.
    /// </summary>
    public class ParserProxy
    {
        private object _parser = null;
        private Type _parserType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factType"></param>
        public ParserProxy(Type factType) : this ( new RuleValidation(factType, null) )
        {
        }

        public ParserProxy(RuleValidation validation)
        {
            //This is a bit dangerous - it may require a rewrite with each new version of the framework.
            Assembly a = Assembly.Load("System.Workflow.Activities, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            _parserType = a.GetType("System.Workflow.Activities.Rules.Parser");
            Type[] constructorParam = { typeof(RuleValidation) };
            object[] callParams = { validation };
            ConstructorInfo ci = _parserType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                null, constructorParam, null);
            _parser = ci.Invoke(callParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public RuleExpressionCondition ParseCondition(string fragment)
        {
            object[] parserParameters = { fragment };
            return (RuleExpressionCondition)_parserType.InvokeMember("ParseCondition",
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                null, _parser, parserParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statements"></param>
        /// <returns></returns>
        public List<RuleAction> ParseStatementList(string statements)
        {
            object[] parserParameters = { statements };
            return (List<RuleAction>)_parserType.InvokeMember("ParseStatementList",
                BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic,
                null, _parser, parserParameters);
        }
    }
}
