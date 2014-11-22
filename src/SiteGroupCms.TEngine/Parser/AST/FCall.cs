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
	public class FCall : Expression
	{
		string name;
		Expression[] args;

		public FCall(int line, int col, string name, Expression[] args)
			:base(line, col)
		{
			this.name = name;
			this.args = args;
		}

		public Expression[] Args
		{
			get { return this.args; }
		}

		public string Name
		{
			get { return this.name; }
		}
	}
}
