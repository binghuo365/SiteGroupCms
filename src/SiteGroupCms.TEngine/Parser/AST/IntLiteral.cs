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
	public class IntLiteral : Expression
	{
		int value;

		public IntLiteral(int line, int col, int value)
			:base(line, col)
		{
			this.value = value;
		}

		public int Value
		{
			get { return this.value; }
		}
	}
}
