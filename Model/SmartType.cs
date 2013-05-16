using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace PerfectStorm.Model
{
    /// <summary>
    /// This uses two distinct techniques to get the job done.
    /// Generics won't let you specify a method, but do most of the job.
    /// Reflection (with caching) can solve the remainder.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartType<T> : SmartTypeBase where T : struct
    {
        private T? _value;

        private static MethodInfo _mi = null;
        private static bool _miInvoked = false;

        /// <summary>
        /// This will always fail if the underlying type does not implement TryParse.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string s, out T result)
        {
            bool success = false;
           
            result = default(T);
            if (!_miInvoked)
            {
                Type type = typeof(T);
                Type[] arrTypes = { typeof(string) };
                _mi = type.GetMethod("Parse", arrTypes);
            }

            if (_mi != null)
            {
                result = default(T);
                object[] paramArgs = { s };
                try
                {
                    result = (T)_mi.Invoke(null, paramArgs);
                    success = true;
                }
                catch (TargetInvocationException ex)
                {
                    Console.WriteLine(ex);
                }            
            }
            return success;
        }
        
        public override void ProcessValue(string value)
        {
            base.ProcessValue(value);
            T result;
            _isValid = TryParse(value, out result);

            if (_isValid)
            {
                _value = result;
            }
        }

        public T Value
        {
            get
            {
                if (!(IsValid && IsSet))  throw new Exception("Value must be set (and valid) before you can read it.");
                return (T)_value;
            }
            set
            {
                setString(value.ToString());
            }
        }
    }
     
}
