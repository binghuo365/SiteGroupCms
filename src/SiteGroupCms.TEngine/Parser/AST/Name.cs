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
	public class Name : Expression
	{
		string id;

		public Name(int line, int col, string id)
			:base(line, col)
		{
			this.id = id;
		}

		public string Id
		{
			get { return this.id; }
		}
	}
}
