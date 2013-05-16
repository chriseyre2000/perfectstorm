using System;
using System.Collections.Generic;
using System.Text;
using PerfectStorm.Model;

namespace WPFModel
{
    public class User : ModelBase
    {
        private SmartInt _Age = new SmartInt();
        private SmartString _Name = new SmartString();

        public User()
            : base()
        {
            AddAttribute("Age", _Age);
            AddAttribute("Name", _Name);
        }

        /// <summary>
        /// 
        /// </summary>
        public SmartString Name { get { return _Name; } }

        /// <summary>
        /// 
        /// </summary>
        public SmartInt Age { get { return _Age; } }
    }
}
