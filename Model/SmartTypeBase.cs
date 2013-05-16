using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace PerfectStorm.Model
{
    public abstract class SmartTypeBase : INotifyPropertyChanged
    {
        protected string _internalString = null;
        protected string _initialValue = null;

        private string _oldValue = null;

        public string GetString()
        {
            return _internalString;
        }

        public virtual void ProcessValue(string value)
        {
            _isSet = true;
            if (_oldValue != value)
            {
                bool wasDirty = _isDirty;
                _isDirty = true;
                _oldValue = value;

                if (!wasDirty)
                {
                    NotifyPropertyChanged("IsDirty");
                }
            }
        }

        public virtual void setString(string value)
        {
            _internalString = value;
            ProcessValue(value);
            NotifyPropertyChanged("Value");
        }

        protected bool _isDirty = false;

        public bool IsDirty
        {
            get { return _isDirty; }
        }

        protected bool _isValid = false;

        public bool IsValid
        {
            get { return _isValid; }
        }

        protected bool _isSet;

        public bool IsSet
        {
            get { return _isSet; }
        }

        public void Init()
        {
            _isDirty = false;
            _initialValue = _internalString;
            NotifyPropertyChanged("IsDirty");
            NotifyPropertyChanged("Value");
        }

        public void Restore()
        {
            string tempInitial = _initialValue;
            Clear();
            setString(tempInitial);
            Init();
        }

        public virtual void Clear()
        {
            Init();
            _isSet = false;
            _isValid = false;
            _initialValue = null;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}
