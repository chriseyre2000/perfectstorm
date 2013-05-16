using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectStorm.Model
{
    public class SmartString : SmartTypeBase
    {
        private string _value = null;

        public override void ProcessValue(string value)
        {
            base.ProcessValue(value);
            _isValid = true; // Strings are always valid...
            _value = value;
        }

        public string Value
        {
            get
            {
                if (!IsValid) throw new Exception("Value must be set before you can read it.");
                return _value;
            }
            set
            {
                setString(value);
            }
        }
    }
}
