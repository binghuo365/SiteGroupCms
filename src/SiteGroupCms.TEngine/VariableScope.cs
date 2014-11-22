/*****************************************************
 * 本类库的核心系 AderTemplates
 * (C) Andrew Deren 2004
 * http://www.adersoftware.com
 *****************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.TEngine
{
    public class VariableScope
    {
        VariableScope parent;
        Dictionary<string, object> values;

        public VariableScope()
            : this(null)
        {
        }

        public VariableScope(VariableScope parent)
        {
            this.parent = parent;
            this.values = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// clear all variables from this scope
        /// </summary>
        public void Clear()
        {
            values.Clear();
        }

        /// <summary>
        /// gets the parent scope for this scope
        /// </summary>
        public VariableScope Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// returns true if variable name is defined
        /// otherwise returns parents isDefined if parent != null
        /// </summary>
        public bool IsDefined(string name)
        {
            if (values.ContainsKey(name))
                return true;
            else if (parent != null)
                return parent.IsDefined(name);
            else
                return false;
        }

        /// <summary>
        /// returns value of variable name
        /// If name is not in this scope and parent != null
        /// parents this[name] is called
        /// </summary>
        public object this[string name]
        {
            get
            {
                if (!values.ContainsKey(name))
                {
                    if (parent != null)
                        return parent[name];
                    else
                        return null;
                }
                else
                    return values[name];
            }
            set { values[name] = value; }
        }
    }
}
