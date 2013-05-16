using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;


namespace PerfectStorm.Model
{
    // We now have a fully functional navigator, but are missing the model details.
    public class Navigator : XPathNavigator
    {
        #region Private Fields

        private ModelBase _node = null;
        private SmartTypeBase _attribute = null;
        // We need to know the _AttributeNames as well as the _Attribute to ease the navigation.
        private string _attributeName = "";

        #endregion

        #region Constructors

        /// <summary>
        /// This is the public accessible constructor.
        /// </summary>
        /// <param name="Node">ModelComponent that is the starting point for navigation</param>
        public Navigator(ModelBase Node)
        {
            _node = (ModelBase)Node;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This relates to namespaces that does not apply here.
        /// </summary>
        public override string BaseURI
        {
            // we don't expose a namespace right now
            get { return string.Empty; }
        }

        /// <summary>
        /// This will copy the model navigator.
        /// </summary>
        /// <returns></returns>
        public override XPathNavigator Clone()
        {
            Navigator newNav = new Navigator(null);
            newNav._attribute = _attribute;
            newNav._node = _node;
            newNav._attributeName = _attributeName;
            return newNav;
        }

        /// <summary>
        /// This returns the attribute from the specified Node.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="namespaceURI"></param>
        /// <returns></returns>
        public override string GetAttribute(string localName, string namespaceURI)
        {
            if (_attribute == null)
            {
                SmartTypeBase attr = _node.GetAttribute(localName);
                if (attr != null)
                {
                    return attr.GetString();
                }
                else
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Attributes are always empty as are nodes with no children and no attributes.
        /// </summary>
        public override bool IsEmptyElement
        {
            get 
            {                
                if (_attribute == null)
                {
                    if (NodeChildCount() + _node._attributes.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Indicate that the two navigators are looking at the same point.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool IsSamePosition(XPathNavigator other)
        {
            Navigator otherNav = other as Navigator;
            // Both must be Navigators
            if (otherNav == null)
            {
                return false;
            }

            if (_node != otherNav._node)
            {
                return false;
            }

            if (_attribute != otherNav._attribute)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the name (we don't use namespaces).
        /// </summary>
        public override string LocalName
        {
            get { return Name; }
        }

        /// <summary>
        /// Syncronises the two Navigators on the same point.
        /// </summary>
        /// <param name="other">The other navigator to syncronise with</param>
        /// <returns>bool : true iff successfully navigated.</returns>
        public override bool MoveTo(XPathNavigator other)
        {
            Navigator otherNav = other as Navigator;

            if (otherNav == null)
            {
                return false;
            }

            _node = otherNav._node;
            _attribute = otherNav._attribute;
            _attributeName = otherNav._attributeName;
            
            return true;
        }      

        /// <summary>
        /// Moves to the first attribute of the ModelComponent.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToFirstAttribute()
        {
            if (_attribute == null)
            {
                if (_node._attributes.Count == 0)
                {
                    //We have no attributes
                    return false;
                }
                else
                {
                    _attributeName = _node._attributeNames[0];
                    _attribute = (SmartTypeBase)_node._attributes[_attributeName];
                    return true;
                }
            }
            else
            {
                // We are already looking at an attribute so this must fail.
                return false;
            }
        }

        /// <summary>
        /// Moves to the first child of the ModelComponent.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToFirstChild()
        {
            if (_attribute == null)
            {
                if (NodeChildCount() == 0)
                {
                    return false;
                }
                else
                {
                    _node = (ModelBase)_node.Children(typeof(ModelBase))[0];
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// We do not implement namespaces so this always returns false.
        /// </summary>
        /// <param name="namespaceScope"></param>
        /// <returns></returns>
        public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
        {
            return false;
        }

        /// <summary>
        /// We do not currently implement ids so this always returns false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool MoveToId(string id)
        {
            return false;
        }

        /// <summary>
        /// Moves to the next sibling node of the current node.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToNext()
        {            
            if (_attribute != null)
            {
                return false;
            }

            ModelBase parent = _node.Parent;
            // If we have no parent then can't move to next sibling
            if (parent == null)
            {
                return false;
            }

            // Anywhere else in the code this would be prohibited and the navigator used instaid
            int index = parent.Children(typeof(ModelBase)).IndexOf(_node);

            if (index + 1 < parent.Children(typeof(ModelBase)).Count)
            {
                _node = (ModelBase)parent.Children(typeof(ModelBase))[index + 1];
                return true;
            }
            else
            {
                // We are already the last child
                return false;
            }
        }

        /// <summary>
        /// Move to the next attribute.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToNextAttribute()
        {
            if (_attribute == null)
            {
                return false;
            }

            int index = _node._attributeNames.IndexOf(_attributeName);

            if (index + 1 < _node._attributeNames.Count)
            {
                _attributeName = _node._attributeNames[index + 1];
                _attribute = (SmartTypeBase)_node._attributes[_attributeName];                                
                return true;
            }
            else
            {
                // We are already the last attribute
                return false;
            }
        }

        /// <summary>
        /// We do not implement namespaces.
        /// </summary>
        /// <param name="namespaceScope"></param>
        /// <returns></returns>
        public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
        {
            return false;
        }

        /// <summary>
        /// Moves the XPathNavigator to the parent node of the current node.
        /// </summary>
        /// <returns>Returns true if the XPathNavigator is successful moving to the parent node of
        /// the current node; otherwise, false. If false, the position of the XPathNavigator is 
        /// unchanged. </returns>
        public override bool MoveToParent()
        {            
            if (_attribute == null)
            {
                ModelBase parent = _node.Parent;

                if (parent == null)
                {
                    return false;
                }
                else
                {
                    _node = parent;
                    return true;
                }
            }
            else
            {
                _attribute = null;
                _attributeName = "";
                return true;
            }
        }

        /// <summary>
        /// Moves to the previous sibling of the current node.
        /// </summary>
        /// <returns></returns>
        public override bool MoveToPrevious()
        {
            if (_attribute != null)
            {
                return false;
            }

            ModelBase parent = _node.Parent;
            int index = parent.Children(typeof(ModelBase)).IndexOf(_node);

            if (index == 0)
            {
                //Already on first sibling.
                return false;
            }

            _node = (ModelBase)parent.Children(typeof(ModelBase))[index - 1];

            return true;
        }

        /// <summary>
        /// Gets the qualified name of the current node.  For a business entity this is the name of
        /// the class.  For an attribute it is the attributes name.
        /// </summary>
        public override string Name
        {                   
            get 
            {
                if (_attribute == null)
                {                    
                    return _node.AbstractTypeName();                    
                }
                else
                {
                    return _attributeName;   
                }                
            }
        }

        /// <summary>
        /// This would return a name table if we implemented one.
        /// </summary>
        public override XmlNameTable NameTable
        {
            get { return null; }
        }

        /// <summary>
        /// We do not implement namespaces so this is always the empty string.
        /// </summary>
        public override string NamespaceURI
        {
            get { return string.Empty;  }
        }

        /// <summary>
        /// Returns the type of the Node.
        /// </summary>
        public override XPathNodeType NodeType
        {
            get 
            {
                if (_attribute == null)
                {
                    return XPathNodeType.Element;
                }
                else
                {
                    return XPathNodeType.Attribute;
                }            
            }
        }

        /// <summary>
        /// We do not implement namespaces.
        /// </summary>
        public override string Prefix
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// This allows the XmlNavigator descendant to expose the underlying object.
        /// </summary>
        public override object UnderlyingObject 
        {
            get 
            {
                if (_attribute == null)
                {
                    return _node;
                }
                else
                {
                    return _attribute;
                }
            }
        }

        /// <summary>
        ///  Gets the string value of the item.
        /// </summary>
        public override string Value
        {
            get
            {
                if (_attribute == null)
                {
                    return Name;
                }
                else
                {
                    return _attribute.GetString();
                }
            }
        }

        /// <summary>
        /// We do not define a XmlLang so we must return string.Empty.
        /// </summary>
        public override string XmlLang
        {
            get { return string.Empty; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int NodeChildCount()
        {
            return _node.Children(typeof(ModelBase)).Count;
        }

        public SmartTypeBase SelectSingleAttribute(string xpath)
        {
            XPathNodeIterator iter = Select(xpath);
            if (iter.MoveNext())
            {
                return (SmartTypeBase)iter.Current.UnderlyingObject;
            }
            else
            {
                return null;
            }        
        }

        private class FosterParent : ModelBase
        {
            public override string AbstractTypeName()
            {
                return Guid.NewGuid().ToString();
            }

            public new void AddChild(ModelBase child)
            {
                base.AddChild(child);
                child.Parent = this;
            }

            public new void RemoveChild(ModelBase child)
            {
                base.RemoveChild(child);
                child.Parent = null;
            }
        }


        public ModelBase SelectSingleModelNode(string xpath)
        {
            // The select routine is bugged if there is only one node.
            // So add a fake node to be the parent!
            // Given that this code is designed to be used in a hierachy this is unlikely to be triggered.
            FosterParent parent = null;
            ModelBase orphan = null;
            if ((_node != null) && (_node.Parent == null) && (_node.Children(typeof(ModelBase)).Count == 0))
            {
                orphan = _node;
                parent = new FosterParent();
                parent.AddChild(orphan);
            }

            try
            {
                XPathNodeIterator iter = Select(xpath);
                if (iter.MoveNext())
                {
                    return (ModelBase)iter.Current.UnderlyingObject;
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                if (parent != null)
                {
                    parent.RemoveChild(orphan);
                }
            }
        }

        /// <summary>
        /// This is a generic form of the method that only returns subclasses of a given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public T[] SelectNodes<T>(string xpath) where T : ModelBase
        {
            // The select routine is bugged if there is only one node.
            // So add a fake node to be the parent!
            FosterParent parent = null;
            ModelBase orphan = null;
            if ((_node != null) && (_node.Parent == null) && (_node.Children(typeof(ModelBase)).Count == 0))
            {
                orphan = _node;
                parent = new FosterParent();
                parent.AddChild(orphan);
            }

            try
            {
                List<T> resultList = new List<T>();

                XPathNodeIterator iter = Select(xpath);
                while (iter.MoveNext())
                {
                    if (iter.Current.UnderlyingObject is T)
                    {
                        resultList.Add((T)iter.Current.UnderlyingObject);
                    }
                }
                return resultList.ToArray();
            }
            finally
            {
                if (parent != null)
                {
                    parent.RemoveChild(orphan);
                }
            }
        }

        public ModelBase[] SelectNodes(string xpath)
        {
            // The select routine is bugged if there is only one node.
            // So add a fake node to be the parent!
            FosterParent parent = null;
            ModelBase orphan = null;
            if ((_node != null) && (_node.Parent == null) && (_node.Children(typeof(ModelBase)).Count == 0))
            {
                orphan = _node;
                parent = new FosterParent();
                parent.AddChild(orphan);
            }

            try
            {
                List<ModelBase> resultList = new List<ModelBase>();

                XPathNodeIterator iter = Select(xpath);
                while (iter.MoveNext())
                {
                    resultList.Add((ModelBase)iter.Current.UnderlyingObject);
                }
                return resultList.ToArray();
            }
            finally
            {
                if (parent != null)
                {
                    parent.RemoveChild(orphan);
                }
            }
        }

        #endregion
    }
}

