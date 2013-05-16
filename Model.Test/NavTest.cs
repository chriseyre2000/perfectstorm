using System.Xml.XPath;
using System.Xml.Xsl;
using NUnit.Framework;
using PerfectStorm.Model;

namespace Model.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class NavTest
    {

        private Royal _Anne = null;
        private Royal _Root = null;


        #region Nested Enums/Structs/Classes

        /// <summary>
        /// The following is a model definition that will assist with the testing of the Model
        /// Navigator.
        /// </summary>
        class Royal : ModelBase
        {

            private SmartString _Name = new SmartString();
            private SmartInt _Value = new SmartInt();


            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            public Royal(string name, int value)
            {
                AddAttribute("Name", _Name);
                AddAttribute("Value", _Value);
                _Name.Value = name;
                _Value.Value = value;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string AbstractTypeName()
            {
                return "Royal";
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="royal"></param>
            public void Add(Royal royal)
            {
                this.AddChild(royal);
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="dog"></param>
            public void AddCorgi(Corgi dog)
            {
                this.AddChild(dog);
            }

            public SmartString Name
            {
                get 
                {
                    return this.GetAttribute("Name") as SmartString;
                }
            }

        }


        /// <summary>
        /// This represents a royal corgi.
        /// </summary>
        class Corgi : ModelBase
        {

            private SmartString _Name = new SmartString();


            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            public Corgi(string name)
            {
                AddAttribute("Name", _Name);
                _Name.Value = name;
            }

            public override string AbstractTypeName()
            {
                return "Corgi";
            }

        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        [TearDown]
        public void teardown()
        {
            _Root = null;
            _Anne = null;
        }


        /// <summary>
        /// 
        /// </summary>
        [SetUp]
        public void setUp()
        {
            // This is a set of test data taken from Code Generation in .NET.
            // These are used as worked examples for the XSLT.
            _Root = new Royal("British Royals", 1);
            Royal george = new Royal("George VI", 2);
            Royal elizabeth = new Royal("Elizabeth", 3);
            Royal charles = new Royal("Charles", 4);
            _Anne = new Royal("Anne", 5);
            Royal andrew = new Royal("Andrew", 5);
            Royal margaret = new Royal("Margaret", 5);
            Royal david = new Royal("David", 5);
            Royal sarah = new Royal("Sarah", 5);
            _Root.Add(george);
            george.Add(elizabeth);
            george.Add(margaret);
            elizabeth.Add(charles);
            charles.Add(new Royal("William", 6));
            charles.Add(new Royal("Harry", 6));
            elizabeth.Add(_Anne);
            _Anne.Add(new Royal("Peter", 7));
            _Anne.Add(new Royal("Zara", 7));
            elizabeth.Add(andrew);
            andrew.Add(new Royal("Beatrice", 8));
            andrew.Add(new Royal("Eugenie", 8));
            elizabeth.Add(new Royal("Edward", 5));
            margaret.Add(david);
            david.Add(new Royal("Margaritta", 9));
            david.Add(new Royal("Charles", 9));
            margaret.Add(sarah);
            sarah.Add(new Royal("Samuel", 10));
            sarah.Add(new Royal("Arthur", 10));
        }


        /// <summary>
        /// The following is a test of the ancestor axis.
        /// </summary>
        [Test]
        public void Ancestor()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("ancestor::*");

            string[] Royals = { "British Royals", "George VI", "Elizabeth" };

            Assert.AreEqual(3, iter.Count, "The count is set before MoveNext is called");

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the ancestor-or-self axis.
        /// </summary>
        [Test]
        public void AncestorOrSelf()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("ancestor-or-self::*");

            string[] Royals = { "British Royals", "George VI", "Elizabeth", "Anne" };

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the child axis.
        /// </summary>
        [Test]
        public void Child()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("child::*");

            string[] Royals = { "Peter", "Zara" };

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the descendant axis.
        /// </summary>
        [Test]
        public void Descendant()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("descendant::*");

            string[] Royals = { "Peter", "Zara" };

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the descendant-or-self axis.
        /// </summary>
        [Test]
        public void DescendantOrSelf()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("descendant-or-self::*");

            string[] Royals = { "Anne", "Peter", "Zara" };

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the following axis.
        /// </summary>
        [Test]
        public void Following()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("following::*");

            string[] Royals = 
            { 
                "Andrew", "Beatrice", "Eugenie", "Edward",
                "Margaret", "David", "Margaritta", "Charles",
                "Sarah", "Samuel", "Arthur"
            };

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the following-sibling axis.
        /// </summary>
        [Test]
        public void FollowingSibling()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("following-sibling::*");

            string[] Royals = 
            { 
                "Andrew", "Edward"
            };

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the parent axis.
        /// </summary>
        [Test]
        public void Parent()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("parent::*");
            iter.MoveNext();
            Assert.AreEqual("Elizabeth", iter.Current.GetAttribute("Name", string.Empty));
            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the preceding axis.
        /// </summary>
        [Test]
        public void Preceding()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("preceding::*");

            string[] Royals = 
            { 
                "Charles", "William", "Harry"
            };

            foreach (string name in Royals)
            {
                iter.MoveNext();
                Assert.AreEqual(name, iter.Current.GetAttribute("Name", string.Empty));
            }

            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the preceding-sibling axis.
        /// </summary>
        public void precedingSibling()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("preceding-sibling::*");
            iter.MoveNext();
            Assert.AreEqual("Charles", iter.Current.GetAttribute("Name", string.Empty));
            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// The following is a test of the self axis.
        /// </summary>
        public void Self()
        {
            Navigator nav = new Navigator(_Anne);
            XPathNodeIterator iter;
            iter = nav.Select("self::*");
            iter.MoveNext();
            Assert.AreEqual("Anne", iter.Current.GetAttribute("Name", string.Empty));
            Assert.AreEqual(false, iter.MoveNext());
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void IsSamePosition()
        {
            Navigator nav = new Navigator(_Anne);
            Assert.IsTrue(nav.MoveToParent(), "Anne should have a parent");
            ModelBase comp = (ModelBase)nav.UnderlyingObject;
            Navigator nav2 = new Navigator(comp);
            Assert.IsTrue(nav.IsSamePosition(nav2), "These should be on the same node");
            nav2.MoveToFirstChild();
            Assert.IsFalse(nav.IsSamePosition(nav2), "These should be on different nodes");
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void Name()
        {
            Navigator nav = new Navigator(_Anne);
            Assert.AreEqual("Royal", nav.Name);

            Assert.IsTrue(nav.MoveToFirstAttribute(), "Should have an attribute");
            Assert.AreEqual("Name", nav.Name);
        }

        [Test]
        public void SelectSingleModelNode()
        {
            Navigator nav = new Navigator(_Anne);
            Assert.IsNotNull(nav.SelectSingleModelNode("//Royal"));
            Royal r = (Royal)nav.SelectSingleModelNode("//Royal");
            Assert.AreEqual("George VI", r.Name.Value);
        }

        [Test]
        public void SelectNodes()
        {
            Navigator nav = new Navigator(_Anne);
            Royal[] royals = nav.SelectNodes<Royal>("//Royal");
            
            Assert.AreEqual(19, royals.Length);           
        }


        /// <summary>
        /// This is an example of pushing the XSLT further.
        /// </summary>
        [Test]
        public void RoyalsWithCorgi()
        {
            Navigator nav = new Navigator(_Anne);
            nav.MoveToParent();
            Royal liz = (Royal)nav.UnderlyingObject;

            liz.AddCorgi(new Corgi("A Dog"));

            nav = new Navigator(_Root);

            // This reads find me a list of Royal's which directly own a corgi called 'A Dog'.

            XPathNodeIterator iter = nav.Select("//Royal[Corgi[@Name='A Dog']]");

            Assert.IsTrue(iter.MoveNext(), "We should be able to find Elizabeth again");
            Assert.AreEqual("Elizabeth", iter.Current.GetAttribute("Name", string.Empty));

            nav.MoveToRoot();

            iter = nav.Select("//Corgi");
            Assert.IsTrue(iter.MoveNext(), "We should be able to find the dog");

            Assert.AreEqual("", iter.Current.GetAttribute("Name 2", string.Empty));
            Assert.AreEqual("A Dog", iter.Current.GetAttribute("Name", string.Empty));

            // The following is proof that searches using // work from anywhere in the model.
            // This would allow us to redirect ListMT to read from the model.
            // The price would be that we would have to load 
            nav = new Navigator(_Anne);
            iter = nav.Select("//Corgi[@Name='A Dog']");
            Assert.IsTrue(iter.MoveNext(), "We should still be able to the dog from Anne");
            Assert.AreEqual("A Dog", iter.Current.GetAttribute("Name", string.Empty), "Find A Dog from Anne");

            

        }

    }
}



namespace Wtk.Bagheera.Test.Model
{
    

}

