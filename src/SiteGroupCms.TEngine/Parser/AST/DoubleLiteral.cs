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
	public class DoubleLiteral : Expression
	{
		double value;

		public DoubleLiteral(int line, int col, double value)
			:base(line, col)
		{
			this.value = value;
		}

		public double Value
		{
			get { return this.value; }
		}
	}
}
