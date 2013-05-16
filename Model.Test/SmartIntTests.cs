using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using PerfectStorm.Model;

namespace Model.Tests
{
    
    [TestFixture]
    public class SmartIntTests
    {
        private SmartInt _i;

        [SetUp]
        protected void SetUpSmartInt()
        {
            _i = new SmartInt();
        }

        [TearDown]
        protected void TearDownSmartInt()
        {
            _i = null;
        }
        
        [Test]
        public void TestSmartInt()
        {
            Assert.AreEqual(false, _i.IsDirty);
            _i.Value = 42;
            Assert.AreEqual(true, _i.IsDirty);
        }

        [Test]
        public void TestInvalid()
        {
            Assert.AreEqual(false, _i.IsValid);
            _i.Value = 42;
            Assert.AreEqual(true, _i.IsValid);
            _i.setString("xxx");
            Assert.AreEqual(false, _i.IsValid);
        }

        [Test]
        public void TestIs()
        {
            Assert.AreEqual(false, _i.IsDirty, "Initially not dirty");
            Assert.AreEqual(false, _i.IsSet, "Initially not set");
            Assert.AreEqual(false, _i.IsValid, "Initially not valid");
            _i.Value = 42;
            Assert.AreEqual(true, _i.IsDirty, "Dirty once set");
            Assert.AreEqual(true, _i.IsSet, "Set once set");
            Assert.AreEqual(true, _i.IsValid, "Valid once set");
            _i.Init();
            Assert.AreEqual(false, _i.IsDirty, "Not dirty after Init");
            Assert.AreEqual(true, _i.IsSet, "Set after Init");
            Assert.AreEqual(true, _i.IsValid, "Valid after Init");
            _i.setString("xxx");
            Assert.AreEqual(true, _i.IsDirty, "Dirty after xxx");
            Assert.AreEqual(true, _i.IsSet, "Set after xxx");
            Assert.AreEqual(false, _i.IsValid, "Not Valid after xxx");
            _i.Restore();
            Assert.AreEqual(42, _i.Value, "42 After restore");
            Assert.AreEqual(false, _i.IsDirty, "Not dirty after restore");
            Assert.AreEqual(true, _i.IsSet, "Set after Restore");
            Assert.AreEqual(true, _i.IsValid, "Valid after Restore");
            _i.Clear();
            Assert.AreEqual(false, _i.IsDirty, "Not dirty after clear");
            Assert.AreEqual(false, _i.IsSet, "Not set after clear");
            Assert.AreEqual(false, _i.IsValid, "Not valid after clear");
        }
    }
}
