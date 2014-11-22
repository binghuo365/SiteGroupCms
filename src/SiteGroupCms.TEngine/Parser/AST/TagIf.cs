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


	public class TagIf : Tag
	{
		Tag falseBranch;
		Expression test;

		public TagIf(int line, int col, Expression test)
			:base(line, col, "if")
		{
			this.test = test;
		}

		public Tag FalseBranch
		{
			get { return this.falseBranch; }
			set { this.falseBranch = value; }
		}

		public Expression Test
		{
			get { return test; }
		}
	}
}
