/*****************************************************
 * 本类库的核心系 AderTemplates
 * (C) Andrew Deren 2004
 * http://www.adersoftware.com
 *****************************************************/

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace SiteGroupCms.TEngine.Parser.AST
{


	public class Tag : Element
	{
		string name;
		List<TagAttribute> attribs;
		List<Element> innerElements;
		TagClose closeTag;
		bool isClosed;	// set to true if tag ends with />

		public Tag(int line, int col, string name)
			:base(line, col)
		{
			this.name = name;
			this.attribs = new List<TagAttribute>();
			this.innerElements = new List<Element>();
		}

		public List<TagAttribute> Attributes
		{
			get { return this.attribs; }
		}

		public Expression AttributeValue(string name)
		{
			foreach (TagAttribute attrib in attribs)
				if (string.Compare(attrib.Name, name, true) == 0)
					return attrib.Expression;

			return null;
		}

		public List<Element> InnerElements
		{
			get { return this.innerElements; }
		}

		public string Name
		{
			get { return this.name; }
		}

		public TagClose CloseTag
		{
			get { return this.closeTag; }
			set { this.closeTag = value; }
		}

		public bool IsClosed
		{
			get { return this.isClosed; }
			set { this.isClosed = value; }
		}


	}
}
