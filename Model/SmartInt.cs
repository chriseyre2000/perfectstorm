using System;
using System.Collections.Generic;
using System.Text;

namespace PerfectStorm.Model
{
    public class SmartInt : SmartTypeBase
    {
        private int? _value = null;

        public override void ProcessValue(string value)
        {
            base.ProcessValue(value);
            int result;
            _isValid = int.TryParse(value, out result);
            if (_isValid)
            {
                _value = result;
            }
        }

        public int Value 
        { 
            get 
            {
                if (!IsValid) throw new Exception("Value must be set before you can read it.");
                return (int)_value;
            } 
            set 
            {
                setString(value.ToString());
            } 
        }
    }
}
