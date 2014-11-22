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
	public class Text : Element
	{
		string data;

		public Text(int line, int col, string data)
			:base(line, col)
		{
			this.data = data;
		}

		public string Data
		{
			get { return this.data; }
		}
	}
}
