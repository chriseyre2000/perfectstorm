using System;
using System.Collections.Generic;
using System.Reflection;
using System.Workflow.Activities.Rules;
using System.Xml;

namespace PerfectStorm.Rules
{
    /// <summary>
    /// This is a far more compact serializer than the one supplied with the WF rules engine.
    /// </summary>
    public class TextRuleSetSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="rs"></param>
        public void Serialize(XmlWriter writer, RuleSet rs)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("RuleSet.Text");
            writer.WriteAttributeString("Name", rs.Name);
            writer.WriteAttributeString("Description", rs.Description);
            writer.WriteAttributeString("ChainingBehavior", rs.ChainingBehavior.ToString());

            foreach (Rule rule in rs.Rules)
            {
                writer.WriteStartElement("Rule");
                writer.WriteAttributeString("Active", rule.Active.ToString());
                writer.WriteAttributeString("Description", rule.Description);
                writer.WriteAttributeString("Name", rule.Name);
                writer.WriteAttributeString("Priority", rule.Priority.ToString());
                writer.WriteAttributeString("ReevaluationBehavior", rule.ReevaluationBehavior.ToString());

                writer.WriteStartElement("Condition");
                writer.WriteValue(rule.Condition.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("ThenActions");

                foreach (RuleAction action in rule.ThenActions)
                {
                    writer.WriteStartElement("Action");
                    writer.WriteValue(action.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteStartElement("ElseActions");

                foreach (RuleAction action in rule.ElseActions)
                {
                    writer.WriteStartElement("Action");
                    writer.WriteValue(action.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private T GetEnumValue<T>(string value)
        {
            if (!typeof(T).IsSubclassOf(typeof(Enum)))
                throw new Exception("Must be an Enum");

            int[] allValues = (int[])Enum.GetValues(typeof(T));

            foreach (int i in allValues)
            {
                T newValue = (T)Enum.ToObject(typeof(T), i);
                if (newValue.ToString() == value)
                {
                    return newValue;
                }
            }
            throw new Exception(value + " is not of Type " + typeof(T).Name);
        }

        public RuleSet Deserialize(Type factType, string xmldoc)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xmldoc);
            return PopulateRuleSet(factType, xDoc);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factType"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public RuleSet Deserialize(Type factType, XmlReader reader)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(reader);
            return PopulateRuleSet(factType, xDoc);
        }

        private RuleSet PopulateRuleSet(Type factType, XmlDocument xDoc)
        {
            RuleSet result = new RuleSet();

            ParserProxy parser = new ParserProxy(factType);

            //string allXml = reader.ReadOuterXml();

            result.Name = xDoc.SelectSingleNode("//RuleSet.Text/@Name").InnerText;
            result.Description = xDoc.SelectSingleNode("//RuleSet.Text/@Description").InnerText;
            result.ChainingBehavior = GetEnumValue<RuleChainingBehavior>(xDoc.SelectSingleNode("//RuleSet.Text/@ChainingBehavior").InnerText);

            foreach (XmlNode ruleNode in xDoc.SelectNodes("//Rule"))
            {
                Rule rule = new Rule();
                rule.Active = ruleNode.SelectSingleNode("@Active").InnerText == "true";
                rule.Description = ruleNode.SelectSingleNode("@Description").InnerText;
                rule.Name = ruleNode.SelectSingleNode("@Name").InnerText;
                rule.Priority = int.Parse(ruleNode.SelectSingleNode("@Priority").InnerText);
                rule.ReevaluationBehavior = GetEnumValue<RuleReevaluationBehavior>(ruleNode.SelectSingleNode("@ReevaluationBehavior").InnerText);

              

                // I have problem identifying rule condition...
                rule.Condition = parser.ParseCondition(ruleNode.SelectSingleNode("Condition").InnerText);

                foreach (XmlNode thenNode in ruleNode.SelectNodes("ThenActions/Action"))
                {
                    List<RuleAction> actions = parser.ParseStatementList(thenNode.InnerText);
                    foreach (RuleAction action in actions)
                    {
                        rule.ThenActions.Add(action);
                    }
                }

                foreach (XmlNode elseNode in ruleNode.SelectNodes("ThenActions"))
                {
                    List<RuleAction> actions = parser.ParseStatementList(elseNode.InnerText);
                    foreach (RuleAction action in actions)
                    {
                        rule.ElseActions.Add(action);
                    }
                }

                result.Rules.Add(rule);
            }
            return result;
        }
    }
}
