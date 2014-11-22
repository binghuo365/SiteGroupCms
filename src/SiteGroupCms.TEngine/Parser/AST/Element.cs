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
	public class Element
	{
		int line;
		int col;

		public Element(int line, int col)
		{
			this.line = line;
			this.col = col;
		}

		public int Col
		{
			get { return this.col; }
		}

		public int Line
		{
			get { return this.line; }
		}
	}
}
