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
	public class TagClose : Element
	{
		string name;

		public TagClose(int line, int col, string name)
			:base(line, col)
		{
			this.name = name;
		}

		public string Name
		{
			get { return this.name; }
		}
	}
}
