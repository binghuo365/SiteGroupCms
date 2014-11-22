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
using SiteGroupCms.TEngine.Parser;

namespace TemplateEngine
{
    class Program
    {
        static void Main(string[] args)
        {
        Start:
            Console.Write("> ");
            string data = Console.ReadLine();

            TemplateLexer lexer = new TemplateLexer(data);
            do
            {
                Token token = lexer.Next();
                Console.WriteLine("{0} ({1}, {2}): {3}", token.TokenKind.ToString(), token.Line, token.Col
                    , token.Data);
                if (token.TokenKind == TokenKind.EOF)
                    break;
            } while (true);

            goto Start;


        }
    }
}
