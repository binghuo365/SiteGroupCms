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
	public class StringLiteral : Expression
	{
		string content;

		public StringLiteral(int line, int col, string content)
			:base(line, col)
		{
			this.content = content;
		}

		public string Content
		{
			get { return this.content; }
		}
	}
}
