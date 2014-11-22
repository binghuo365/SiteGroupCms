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
using System.IO;

namespace SiteGroupCms.TEngine.Parser
{
    public class TemplateLexer
    {
        static Dictionary<string, TokenKind> keywords;

        static TemplateLexer()
        {
            keywords = new Dictionary<string, TokenKind>(StringComparer.InvariantCultureIgnoreCase);
            keywords["or"] = TokenKind.OpOr;
            keywords["and"] = TokenKind.OpAnd;
            keywords["is"] = TokenKind.OpIs;
            keywords["isnot"] = TokenKind.OpIsNot;
            keywords["lt"] = TokenKind.OpLt;
            keywords["gt"] = TokenKind.OpGt;
            keywords["lte"] = TokenKind.OpLte;
            keywords["gte"] = TokenKind.OpGte;
        }

        enum LexMode
        {
            Text,
            Tag,
            Expression,
            String
        }

        const char EOF = (char)0;

        LexMode currentMode;
        Stack<LexMode> modes;

        int line;
        int column;
        int pos;	// position within data

        string data;

        int saveLine;
        int saveCol;
        int savePos;

        public TemplateLexer(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            data = reader.ReadToEnd();

            Reset();
        }

        public TemplateLexer(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.data = data;

            Reset();
        }

        private void EnterMode(LexMode mode)
        {
            modes.Push(currentMode);
            currentMode = mode;
        }

        private void LeaveMode()
        {
            currentMode = modes.Pop();
        }

        private void Reset()
        {
            modes = new Stack<LexMode>();
            currentMode = LexMode.Text;
            modes.Push(currentMode);

            line = 1;
            column = 1;
            pos = 0;
        }

        protected char LA(int count)
        {
            if (pos + count >= data.Length)
                return EOF;
            else
                return data[pos + count];
        }

        protected char Consume()
        {
            char ret = data[pos];
            pos++;
            column++;

            return ret;
        }

        protected char Consume(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count", "count has to be greater than 0");

            char ret = ' ';
            while (count > 0)
            {
                ret = Consume();
                count--;
            }
            return ret;
        }

        void NewLine()
        {
            line++;
            column = 1;
        }

        protected Token CreateToken(TokenKind kind, string value)
        {
            return new Token(kind, value, line, column);
        }

        protected Token CreateToken(TokenKind kind)
        {
            string tokenData = data.Substring(savePos, pos - savePos);
            if (kind == TokenKind.StringText)
                tokenData = tokenData.Replace("\"\"", "\""); // replace double "" with single "
            if (kind == TokenKind.StringText || kind == TokenKind.TextData)
                tokenData = tokenData.Replace("@@", "@");	// replace @@ with @

            return new Token(kind, tokenData, saveLine, saveCol);
        }

        /// <summary>
        /// reads all whitespace characters (does not include newline)
        /// </summary>
        /// <returns></returns>
        protected void ReadWhitespace()
        {
            while (true)
            {
                char ch = LA(0);
                switch (ch)
                {
                    case ' ':
                    case '\t':
                        Consume();
                        break;
                    case '\n':
                        Consume();
                        NewLine();
                        break;

                    case '\r':
                        Consume();
                        if (LA(0) == '\n')
                            Consume();
                        NewLine();
                        break;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// save read point positions so that CreateToken can use those
        /// </summary>
        private void StartRead()
        {
            saveLine = line;
            saveCol = column;
            savePos = pos;
        }

        public Token Next()
        {
            switch (currentMode)
            {
                case LexMode.Text: return NextText();
                case LexMode.Expression: return NextExpression();
                case LexMode.Tag: return NextTag();
                case LexMode.String: return NextString();
                default: throw new ParseException("Encountered invalid lexer mode: " + currentMode.ToString(), line, column);
            }
        }

        private Token NextExpression()
        {
            StartRead();
        StartExpressionRead:
            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return CreateToken(TokenKind.EOF);
                case ',':
                    Consume();
                    return CreateToken(TokenKind.Comma);
                case '.':
                    Consume();
                    return CreateToken(TokenKind.Dot);
                case '(':
                    Consume();
                    return CreateToken(TokenKind.LParen);
                case ')':
                    Consume();
                    return CreateToken(TokenKind.RParen);
                case '}':
                    Consume();
                    LeaveMode();
                    return CreateToken(TokenKind.ExpEnd);
                case '[':
                    Consume();
                    return CreateToken(TokenKind.LBracket);
                case ']':
                    Consume();
                    return CreateToken(TokenKind.RBracket);
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    ReadWhitespace();
                    return NextExpression();

                case '"':
                    Consume();
                    EnterMode(LexMode.String);
                    return CreateToken(TokenKind.StringStart);

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadNumber();

                case '-':
                    {
                        if (Char.IsDigit(LA(1)))
                            return ReadNumber();

                        goto default;
                    }

                default:
                    if (Char.IsLetter(ch) || ch == '_')
                        return ReadId();
                    else
                        throw new ParseException("Invalid character in expression: " + ch, line, column);

            }
        }

        private Token NextTag()
        {
            StartRead();
        StartTagRead:
            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return CreateToken(TokenKind.EOF);
                case '=':
                    Consume();
                    return CreateToken(TokenKind.TagEquals);
                case '"':
                    Consume();
                    EnterMode(LexMode.String);
                    return CreateToken(TokenKind.StringStart);
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    ReadWhitespace();	// ignore whitespace
                    StartRead();		// remark current position
                    goto StartTagRead;	// start again
                case '>':
                    Consume();
                    LeaveMode();
                    return CreateToken(TokenKind.TagEnd);
                case '/':
                    if (LA(1) == '>')
                    {
                        Consume(2); // consume />
                        LeaveMode();
                        return CreateToken(TokenKind.TagEndClose);
                    }
                    break;
                default:
                    if (Char.IsLetter(ch) || ch == '_')
                        return ReadId();
                    break;

            }
            throw new ParseException("Invalid character in tag: " + ch, line, column);
        }

        private Token NextString()
        {
            StartRead();
        StartStringRead:
            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return CreateToken(TokenKind.EOF);

                case '$':
                    if (LA(1) == '{')
                    {
                        if (savePos == pos)
                        {
                            Consume(2);
                            EnterMode(LexMode.Expression);
                            return CreateToken(TokenKind.ExpStart);
                        }
                        else
                            break;
                    }
                    Consume();
                    goto StartStringRead;

                case '\r':
                case '\n':
                    ReadWhitespace();
                    goto StartStringRead;
                case '"':
                    if (LA(1) == '"')
                    {
                        // just escape
                        Consume(2);
                        goto StartStringRead;
                    }
                    else if (pos == savePos)
                    {
                        Consume();
                        LeaveMode();
                        return CreateToken(TokenKind.StringEnd);
                    }
                    else
                        break; // just break so that text is returned
                default:
                    Consume();
                    goto StartStringRead;

            }

            return CreateToken(TokenKind.StringText);
        }

        private Token NextText()
        {
            StartRead();

        StartTextRead:
            switch (LA(0))
            {
                case EOF:
                    if (savePos == pos)
                        return CreateToken(TokenKind.EOF);
                    else
                        break;

                case '$':
                    if (LA(1) == '{')
                    {
                        if (savePos == pos)
                        {
                            Consume(2);
                            EnterMode(LexMode.Expression);
                            return CreateToken(TokenKind.ExpStart);
                        }
                        else
                            break;
                    }
                    Consume();
                    goto StartTextRead;

                case '<':
                    if (LA(1) == '#' && LA(2) != '/')
                    {
                        if (savePos == pos)
                        {
                            Consume(2);
                            EnterMode(LexMode.Tag);
                            return CreateToken(TokenKind.TagStart);
                        }
                        else
                            break;
                    }
                    else if (LA(1) == '#' && LA(2) == '/')
                    {
                        if (savePos == pos)
                        {
                            Consume(3);
                            EnterMode(LexMode.Tag);
                            return CreateToken(TokenKind.TagClose);
                        }
                        else
                            break;
                    }
                    Consume();
                    goto StartTextRead;
                case '\n':
                case '\r':
                    ReadWhitespace();	// handle newlines specially so that line number count is kept
                    goto StartTextRead;

                default:
                    Consume();
                    goto StartTextRead;
            }

            return CreateToken(TokenKind.TextData);
        }

        /// <summary>
        /// reads word. Word contains any alpha character or _
        /// </summary>
        protected Token ReadId()
        {
            StartRead();

            Consume(); // consume first character of the word

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetterOrDigit(ch) || ch == '_')
                    Consume();
                else
                    break;
            }

            string tokenData = data.Substring(savePos, pos - savePos);

            if (keywords.ContainsKey(tokenData))
                return CreateToken(keywords[tokenData]);
            else
                return CreateToken(TokenKind.ID, tokenData);
        }

        /// <summary>
        /// returns either Integer or Double Token
        /// </summary>
        /// <returns></returns>
        protected Token ReadNumber()
        {
            StartRead();
            Consume(); // consume first digit or -

            bool hasDot = false;

            while (true)
            {
                char ch = LA(0);
                if (Char.IsNumber(ch))
                    Consume();

                // if "." and didn't see "." yet, and next char
                // is number, than starting to read decimal number
                else if (ch == '.' && !hasDot && Char.IsNumber(LA(1)))
                {
                    Consume();
                    hasDot = true;
                }
                else
                    break;
            }

            return CreateToken(hasDot ? TokenKind.Double : TokenKind.Integer);
        }

    }
}
