using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities.Rules;
using System.IO;

using NUnit.Framework;
using PerfectStorm.Rules;

namespace Rules.Tests
{
    [TestFixture]
    public class RulesTest
    {
        #region Test Classes

        public class Person
        {
            private string _Name;
            private int _Age;
            
            public string Name { get {return _Name;}  set { _Name = value;} }
            public int Age { get {return _Age;} set {_Age = value;} }
            private List<string> _ValidationMessages = new List<string>();
            public List<string> ValidationMessages { get { return _ValidationMessages; } }
        }

        public class CheckNameCondition : BaseRuleCondition
        {
            public override bool Evaluate(RuleExecution execution)
            {
                bool result = true;

                Person p = execution.ThisObject as Person;

                if (p != null)
                {
                    result = p.Name == null;
                }

                return result;
            }
        }

        public class CheckNameAction : BaseRuleAction
        {
            public override void Execute(RuleExecution context)
            {
                Person p = context.ThisObject as Person;
                if (p != null)
                {
                    p.ValidationMessages.Add("Name should not be empty");
                }
            }
        }

        public class CheckPersonName : Rule
        {
            public CheckPersonName()
            {
                this.Condition = new CheckNameCondition();
                this.ThenActions.Add(new CheckNameAction());
            }
        }

        #endregion

        [Test]
        public void RuleCheck()
        {
            Rule r = new Rule("CheckName");
            // Pluging custom behaviours into a rule
            r.ReevaluationBehavior = RuleReevaluationBehavior.Never;
            r.Condition = new CheckNameCondition();
            r.ThenActions.Add(new CheckNameAction());

            RuleSet Rules = new RuleSet();
            Rules.Rules.Add(r);

            Rules.Name = "Check Person";

            Person p = new Person();
            RuleExecution execution = new RuleExecution(new RuleValidation(p.GetType(), null), p);

            Rules.Execute(execution);
            Assert.AreEqual(1, p.ValidationMessages.Count);
            
        }
        

        const string XmlRule = @"
<RuleSet.Text Name='Text Ruleset' Description='Test Ruleset' ChainingBehavior='None'>
  <Rule Active='True' Name='R1' Description='Checks Name' Priority='1' ReevaluationBehavior='Never' >
  <Condition>this.Name == null</Condition>
  <ThenActions>
    <Action>this.ValidationMessages.Add(""Name should not be empty"")</Action>
  </ThenActions>
  </Rule>
</RuleSet.Text>
";

        [Test]
        public void StringRule()
        { 
            TextRuleSetSerializer rs = new TextRuleSetSerializer();
            RuleSet Rules = rs.Deserialize(typeof(Person), XmlRule);
            Person p = new Person();
            RuleExecution execution = new RuleExecution(new RuleValidation(p.GetType(), null), p);

            Rules.Execute(execution);
            Assert.AreEqual(1, p.ValidationMessages.Count);
        }

        [Test]
        public void FileStringRule()
        {
            TextRuleSetSerializer rs = new TextRuleSetSerializer();
            RuleSet Rules = rs.Deserialize(typeof(Person), File.ReadAllText(@".\Ruleset.Rule") );
            Person p = new Person();
            RuleExecution execution = new RuleExecution(new RuleValidation(p.GetType(), null), p);

            Rules.Execute(execution);
            Assert.AreEqual(1, p.ValidationMessages.Count);
        }


        [Test]
        public void StringConditions()
        {
            ParserProxy pp = new ParserProxy(typeof(Person));
            RuleSet rules = new RuleSet("foo");
            Rule r = new Rule("R3");            
            r.Condition = pp.ParseCondition("this.Age <= 0");
            r.ThenActions.Add(pp.ParseStatementList("this.ValidationMessages.Add(\"Age must be positive\")")[0]);
            rules.Rules.Add(r);

            Person p = new Person();
            RuleExecution execution = new RuleExecution(new RuleValidation(p.GetType(), null), p);
            rules.Execute(execution);
            Assert.AreEqual(1, p.ValidationMessages.Count);

        }

        [Test]
        [Ignore("This is a C# 3.5 version")]
        public void LambdaConditions()
        {
            ParserProxy pp = new ParserProxy(typeof(Person));
            RuleSet rules = new RuleSet("foo");
            Rule r = new Rule("R3");
         /*
            r.Condition = new CheckRuleCondition<Person>(n => n.Age <= 0);
            r.ThenActions.Add(new PerformRuleAction<Person>(n => n.ValidationMessages.Add("Age must be > 0")));
         */
            rules.Rules.Add(r);

            Person p = new Person();
            RuleExecution execution = new RuleExecution(new RuleValidation(p.GetType(), null), p);
            rules.Execute(execution);
            Assert.AreEqual(1, p.ValidationMessages.Count);
        }

        [Test]
        public void AnonDelegateConditions()
        {
            ParserProxy pp = new ParserProxy(typeof(Person));
            RuleSet rules = new RuleSet("foo");
            Rule r = new Rule("R3");
            r.Condition = new CheckRuleCondition<Person>(delegate(Person person) { return person.Age <= 0; });
            r.ThenActions.Add(new PerformRuleAction<Person>(delegate(Person person) { person.ValidationMessages.Add("Age must be > 0"); }));
            
            rules.Rules.Add(r);

            Person p = new Person();
            RuleExecution execution = new RuleExecution(new RuleValidation(p.GetType(), null), p);
            rules.Execute(execution);
            Assert.AreEqual(1, p.ValidationMessages.Count);
        }
 
    
    }
}
