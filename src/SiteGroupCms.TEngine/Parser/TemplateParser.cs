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

using SiteGroupCms.TEngine.Parser.AST;

namespace SiteGroupCms.TEngine.Parser
{
    public class TemplateParser
    {
        TemplateLexer lexer;
        Token current;
        List<Element> elements;

        public TemplateParser(TemplateLexer lexer)
        {
            this.lexer = lexer;
            this.elements = new List<Element>();
        }

        Token Consume()
        {
            Token old = current;
            current = lexer.Next();
            return old;
        }

        Token Consume(TokenKind kind)
        {
            Token old = current;
            current = lexer.Next();

            if (old.TokenKind != kind)
                throw new ParseException("Unexpected token: " + current.TokenKind.ToString() + ". Was expecting: " + kind, current.Line, current.Col);

            return old;
        }

        Token Current
        {
            get { return current; }
        }

        public List<Element> Parse()
        {
            elements.Clear();

            Consume();

            while (true)
            {
                Element elem = ReadElement();
                if (elem == null)
                    break;
                else
                    elements.Add(elem);
            }
            return elements;
        }

        Element ReadElement()
        {
            switch (Current.TokenKind)
            {
                case TokenKind.EOF:
                    return null;
                case TokenKind.TagStart:
                    return ReadTag();
                case TokenKind.TagClose:
                    return ReadCloseTag();
                case TokenKind.ExpStart:
                    return ReadExpression();
                case TokenKind.TextData:
                    Text text = new Text(Current.Line, Current.Col, Current.Data);
                    Consume();
                    return text;
                default:
                    throw new ParseException("Invalid token: " + Current.TokenKind.ToString(), Current.Line, Current.Col);
            }
        }

        TagClose ReadCloseTag()
        {
            Consume(TokenKind.TagClose);
            Token idToken = Consume(TokenKind.ID);
            Consume(TokenKind.TagEnd);

            return new TagClose(idToken.Line, idToken.Col, idToken.Data);
        }

        Expression ReadExpression()
        {
            Consume(TokenKind.ExpStart);

            Expression exp = TopExpression();

            Consume(TokenKind.ExpEnd);

            return exp;
        }

        Tag ReadTag()
        {
            Consume(TokenKind.TagStart);
            Token name = Consume(TokenKind.ID);
            Tag tag = new Tag(name.Line, name.Col, name.Data);

            while (true)
            {
                if (Current.TokenKind == TokenKind.ID)
                    tag.Attributes.Add(ReadAttribute());
                else if (Current.TokenKind == TokenKind.TagEnd)
                {
                    Consume();
                    break;
                }
                else if (Current.TokenKind == TokenKind.TagEndClose)
                {
                    Consume();
                    tag.IsClosed = true;
                    break;
                }
                else
                    throw new ParseException("Invalid token in tag: " + Current.TokenKind, Current.Line, Current.Col);

            }

            return tag;

        }

        TagAttribute ReadAttribute()
        {
            Token name = Consume(TokenKind.ID);
            Consume(TokenKind.TagEquals);

            Expression exp = null;

            if (Current.TokenKind == TokenKind.StringStart)
                exp = ReadString();
            else
                throw new ParseException("Unexpected token: " + Current.TokenKind + ". Was expection '\"'", Current.Line, Current.Col);

            return new TagAttribute(name.Data, exp);
        }

        Expression ReadString()
        {
            Token start = Consume(TokenKind.StringStart);
            StringExpression exp = new StringExpression(start.Line, start.Col);

            while (true)
            {
                Token tok = Current;

                if (tok.TokenKind == TokenKind.StringEnd)
                {
                    Consume();
                    break;
                }
                else if (tok.TokenKind == TokenKind.EOF)
                    throw new ParseException("Unexpected end of file", tok.Line, tok.Col);
                else if (tok.TokenKind == TokenKind.StringText)
                {
                    Consume();
                    exp.Add(new StringLiteral(tok.Line, tok.Col, tok.Data));
                }
                else if (tok.TokenKind == TokenKind.ExpStart)
                    exp.Add(ReadExpression());
                else
                    throw new ParseException("Unexpected token in string: " + tok.TokenKind, tok.Line, tok.Col);
            }

            if (exp.ExpCount == 1)
                return exp[0];
            else
                return exp;
        }

        Expression TopExpression()
        {
            return OrExpression();
        }

        Expression OrExpression()
        {
            Expression ret = AndExpression();

            while (Current.TokenKind == TokenKind.OpOr)
            {
                Consume(); // Or
                Expression rhs = AndExpression();
                ret = new BinaryExpression(ret.Line, ret.Col, ret, TokenKind.OpOr, rhs);
            }

            return ret;
        }

        Expression AndExpression()
        {
            Expression ret = EqualityExpression();

            while (Current.TokenKind == TokenKind.OpAnd)
            {
                Consume(); // Or
                Expression rhs = EqualityExpression();
                ret = new BinaryExpression(ret.Line, ret.Col, ret, TokenKind.OpAnd, rhs);
            }

            return ret;
        }

        private Expression EqualityExpression()
        {
            Expression ret = RelationalExpression();
            while (Current.TokenKind == TokenKind.OpIs
                || Current.TokenKind == TokenKind.OpIsNot)
            {
                Token tok = Consume(); // consume operator
                Expression rhs = RelationalExpression();

                ret = new BinaryExpression(ret.Line, ret.Col, ret, tok.TokenKind, rhs);
            }

            return ret;
        }

        private Expression RelationalExpression()
        {
            Expression ret = PrimaryExpression();

            while (Current.TokenKind == TokenKind.OpLt
                || Current.TokenKind == TokenKind.OpLte
                || Current.TokenKind == TokenKind.OpGt
                || Current.TokenKind == TokenKind.OpGte)
            {
                Token tok = Consume(); // consume operator
                Expression rhs = PrimaryExpression();
                ret = new BinaryExpression(ret.Line, ret.Col, ret, tok.TokenKind, rhs);
            }

            return ret;
        }

        Expression PrimaryExpression()
        {
            if (Current.TokenKind == TokenKind.StringStart)
                return ReadString();
            else if (Current.TokenKind == TokenKind.ID)
            {
                Token id = Consume();

                Expression exp = null;

                // if ( follows ID, we have a function call
                if (Current.TokenKind == TokenKind.LParen)
                {
                    Consume();	// consume LParen
                    Expression[] args = ReadArguments();
                    Consume(TokenKind.RParen);

                    exp = new FCall(id.Line, id.Col, id.Data, args);
                }
                else  // else, we just have id
                    exp = new Name(id.Line, id.Col, id.Data);

                // while we have ".", keep chaining up field access or method call
                while (Current.TokenKind == TokenKind.Dot || Current.TokenKind == TokenKind.LBracket)
                {
                    if (Current.TokenKind == TokenKind.Dot)
                    {
                        Consume();	// consume DOT
                        Token field = Consume(TokenKind.ID);	// consume ID after dot

                        // if "(" after ID, then it's a method call
                        if (Current.TokenKind == TokenKind.LParen)
                        {
                            Consume(); // consume "("
                            Expression[] args = ReadArguments();
                            Consume(TokenKind.RParen); // read ")"

                            exp = new MethodCall(field.Line, field.Col, exp, field.Data, args);
                        }
                        else
                            exp = new FieldAccess(field.Line, field.Col, exp, field.Data);
                    }
                    else // must be LBracket
                    {
                        // array access
                        Token bracket = Current;
                        Consume(); // consume [
                        Expression indexExp = TopExpression();
                        Consume(TokenKind.RBracket);

                        exp = new ArrayAccess(bracket.Line, bracket.Col, exp, indexExp);
                    }

                }

                return exp;

            }
            else if (Current.TokenKind == TokenKind.Integer)
            {
                int value = Int32.Parse(Current.Data);
                IntLiteral intLiteral = new IntLiteral(Current.Line, Current.Col, value);
                Consume(); // consume int
                return intLiteral;
            }
            else if (Current.TokenKind == TokenKind.Double)
            {
                double value = Double.Parse(Current.Data);
                DoubleLiteral dLiteral = new DoubleLiteral(Current.Line, Current.Col, value);
                Consume(); // consume int
                return dLiteral;
            }
            else if (Current.TokenKind == TokenKind.LParen)
            {
                Consume(); // eat (
                Expression exp = TopExpression();
                Consume(TokenKind.RParen); // eat )

                return exp;
            }
            else
                throw new ParseException("Invalid token in expression: " + Current.TokenKind + ". Was expecting ID or string.", Current.Line, Current.Col);

        }

        private Expression[] ReadArguments()
        {
            List<Expression> exps = new List<Expression>();

            int index = 0;
            while (true)
            {
                if (Current.TokenKind == TokenKind.RParen)
                    break;

                if (index > 0)
                    Consume(TokenKind.Comma);

                exps.Add(TopExpression());

                index++;
            }

            return exps.ToArray();
        }
    }
}
