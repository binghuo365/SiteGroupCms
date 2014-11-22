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
	public class TagAttribute
	{
		string name;
		Expression expression;

		public TagAttribute(string name, Expression expression)
		{
			this.name = name;
			this.expression = expression;
		}

		public Expression Expression
		{
			get { return this.expression; }
		}

		public string Name
		{
			get { return this.name; }
		}
	}
}
