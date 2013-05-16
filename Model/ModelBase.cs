using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace PerfectStorm.Model
{
    public class ModelBase : INotifyPropertyChanged
    {
        private List<ModelBase> _children = new List<ModelBase>();
        private ModelBase _parent = null;

        internal Dictionary<string, SmartTypeBase> _attributes = new Dictionary<string, SmartTypeBase>();

        internal List<string> _attributeNames = new List<string>();

        protected void AddAttribute(string name, SmartTypeBase value)
        {
            _attributeNames.Add(name);
            _attributes.Add(name, value);
            value.PropertyChanged += new PropertyChangedEventHandler(value_PropertyChanged);
        }

        void value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, e);

                if (e.PropertyName == "IsDirty")
                {
                    NotifyPropertyChanged("IsDirty");
                }
            }
        }

        public bool IsDirty
        {
            get
            {
                bool result = false;

                foreach (SmartTypeBase attr in _attributes.Values)
                {
                    if (attr.IsDirty) result = true;
                }

                return result;
            }
        }


        public virtual string AbstractTypeName()
        {
            return this.GetType().Name;
        }

        protected internal SmartTypeBase GetAttribute(string localName)
        {
            if (_attributes.ContainsKey(localName))
            {
                return _attributes[localName];
            }
            else
            {
                return null;
            }
        }

        public void InitAttributes()
        {
            foreach (SmartTypeBase attr in _attributes.Values)
            {
                attr.Init();
            }
        }

        public List<ModelBase> Children(Type childType)
        {
            List<ModelBase> result = new List<ModelBase>();
            foreach (ModelBase child in _children)
            {
                if (childType.IsInstanceOfType(child))
                {
                    result.Add(child);
                }
            }

            return result;
        }

        public ModelBase Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        protected void AddChild(ModelBase child)
        {
            _children.Add(child);
            child._parent = this;
        }

        protected void RemoveChild(ModelBase child)
        {
            _children.Remove(child);
            child.Parent = null;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
