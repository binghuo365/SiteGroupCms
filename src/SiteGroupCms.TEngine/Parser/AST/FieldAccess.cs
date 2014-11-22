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
	public class FieldAccess : Expression
	{
		Expression exp;
		string field;

		public FieldAccess(int line, int col, Expression exp, string field)
			:base(line, col)
		{
			this.exp = exp;
			this.field = field;
		}

		public Expression Exp
		{
			get { return this.exp; }
		}

		public string Field
		{
			get { return this.field; }
		}

	}
}
