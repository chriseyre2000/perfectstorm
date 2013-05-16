using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PerfectStorm.Model;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace Model.Tests
{
    public class Person : ModelBase
    {
        public Person()
        {
            this.AddAttribute("Age", new SmartInt());
        }

        public SmartInt Age 
        { 
            get 
            { 
                return (SmartInt)GetAttribute("Age"); 
            } 
        }

        public void AddChild(Person child)
        {
            base.AddChild(child);
        }
    }
    
    [TestFixture]
    public class ModelTests
    {
        
        [Test]
        public void PersonTest()
        {
            Person p = new Person();
            Assert.IsFalse(p.Age.IsSet);
        }

        [Test]
        public void PersonNavTest()
        { 
            Person p = new Person();
            Assert.AreEqual("Person", p.AbstractTypeName());
            Navigator nav = new Navigator(p);
            Assert.IsNull( nav.GetAttribute("Age", null) );
        }

        [Test]
        public void SelectSingleInTree()
        {
            ModelBase root = new ModelBase();
            
            Person p = new Person();
            p.Age.Value = 42;

            Person parent = new Person();
            parent.AddChild(p);
            
            Navigator nav = new Navigator(p);

            Assert.IsNotNull(nav.SelectSingleModelNode("//Person[@Age=42]"));

        }

        [Test]
        public void SelectRootInTree()
        {
            ModelBase root = new ModelBase();

            Person p = new Person();
            p.Age.Value = 42;

            Person parent = new Person();
            parent.AddChild(p);

            Navigator nav = new Navigator(parent);

            Assert.IsNotNull(nav.SelectSingleModelNode("//Person[@Age=42]"));
        }


        [Test]
        public void SelectSingleSolo()
        {
            // It appears that the Navigator works on trees but not on individual nodes.
            // This needs to be fixed...
            ModelBase root = new ModelBase();

            Person p = new Person();
            p.Age.Value = 42;

            //Person parent = new Person();
            //parent.AddChild(p);

            Navigator nav = new Navigator(p);

            Assert.IsNotNull(nav.SelectSingleModelNode("//Person[@Age=42]"),"This should not be null");
            Assert.IsNull(nav.SelectSingleModelNode("//Person[@Age=43]"),"This should be null");

        }
    }
}
