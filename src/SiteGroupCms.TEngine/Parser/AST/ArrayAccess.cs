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
	public class ArrayAccess : Expression
	{
		Expression exp;
		Expression index;

		public ArrayAccess(int line, int col, Expression exp, Expression index)
			:base(line, col)
		{
			this.exp = exp;
			this.index = index;
		}

		public Expression Exp
		{
			get { return this.exp; }
		}

		public Expression Index
		{
			get { return this.index; }
		}

	}
}
