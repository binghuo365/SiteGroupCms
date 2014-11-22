/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序版本: 5.2.X
 * 
 * 程序作者: 高伟 ( 合作请联系：254860396#qq.com)
 * 
 * 
 * 
 * 
 * 
 */

using System;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using MSScriptControl;
namespace SiteGroupCms.Utils
{
    /// <summary>
    /// 一些常用的字符串函数
    /// </summary>
    public static class Strings
    {
        #region 普通加解密
        /// <summary>
        /// 倒序加1加密
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static string EncryptStr(string rs) //倒序加1加密 
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] + 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }
        /// <summary>
        /// 顺序减1解码 
        /// </summary>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static string DecryptStr(string rs) //顺序减1解码 
        {
            byte[] by = new byte[rs.Length];
            for (int i = 0; i <= rs.Length - 1; i++)
            {
                by[i] = (byte)((byte)rs[i] - 1);
            }
            rs = "";
            for (int i = by.Length - 1; i >= 0; i--)
            {
                rs += ((char)by[i]).ToString();
            }
            return rs;
        }
        /// <summary>
        /// Escape加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(string str)
        {
            if (str == null)
                return String.Empty;
            StringBuilder sb = new StringBuilder();
            int len = str.Length;

            for (int i = 0; i < len; i++)
            {
                char c = str[i];

                //everything other than the optionally escaped chars _must_ be escaped 
                if (Char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '/' || c == '\\' || c == '.')
                    sb.Append(c);
                else
                    sb.Append(Uri.HexEscape(c));
            }

            return sb.ToString();
        }
        /// <summary>
        /// UnEscape解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(string str)
        {
            if (str == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }
            return sb.ToString();
        }
        #endregion
        /// <summary>
        /// 左截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Left(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(0, len);
        }
        /// <summary>
        /// 右截取
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Right(string inputString, int len)
        {
            if (inputString.Length < len)
                return inputString;
            else
                return inputString.Substring(inputString.Length - len, len);
        }
        /// <summary>
        /// 截取指定长度字符串,汉字为2个字符
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }
                if (tempLen > len)
                    break;
            }
            return tempString;
        }
        /// <summary>
        /// 去掉多余空格
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string RemoveSpaceStr(string original)
        {
            return System.Text.RegularExpressions.Regex.Replace(original, "\\s{2,}", " ");
        }
        public static string ToSummary(string Htmlstring)
        {
            string _content = NoHTML(Htmlstring);
            return RemoveSpaceStr(_content).Replace("[Jumbot_PageBreak]", " ");
        }
        #region 去除HTML标记
        ///<summary>   
        ///去除HTML标记   
        ///</summary>   
        ///<param name="NoHTML">包括HTML的源码</param>   
        ///<returns>已经去除后的文字</returns>   
        public static string NoHTML(string Htmlstring)
        {
            //Regex myReg = new Regex(@"(\<.[^\<]*\>)", RegexOptions.IgnoreCase);
            //Htmlstring = myReg.Replace(Htmlstring, "");
            //myReg = new Regex(@"(\<\/[^\<]*\>)", RegexOptions.IgnoreCase);
            //Htmlstring = myReg.Replace(Htmlstring, "");
            //return Htmlstring;

            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "“", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "”", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.Replace("<", "&lt;");
            Htmlstring = Htmlstring.Replace(">", "&gt;");
            return Htmlstring;
        }
        #endregion
        /// <summary>
        /// 不区分大小写的替换
        /// </summary>
        /// <param name="original">原字符串</param>
        /// <param name="pattern">需替换字符</param>
        /// <param name="replacement">被替换内容</param>
        /// <returns></returns>
        public static string ReplaceEx(string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (int i = position0; i < position1; ++i) chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i) chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i) chars[count++] = original[i];
            return new string(chars, 0, count);
        }
        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace("  ", " &nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("'", "&#39;");
            theString = theString.Replace("\r\n", "<br/> ");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDecode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "'");
            theString = theString.Replace("<br/> ", "\r\n");
            theString = theString.Replace("&mdash;", "—");//2012-05-07新加的
            return theString;
        }
        /// <summary>
        /// 转为货币格式
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static string ToMoney(double _value)
        {
            return string.Format("{0:C2}", _value).Replace("￥", "").Replace("$", "").Replace(",", "");

        }
        public static string ToMoney(string _value)
        {
            return string.Format("{0:C2}", Convert.ToDouble(_value)).Replace("￥", "").Replace("$", "").Replace(",", "");

        }
        public static string ToMoney(int _value)
        {
            return string.Format("{0:C2}", Convert.ToDouble(_value)).Replace("￥", "").Replace("$", "").Replace(",", "");

        }
        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 转半角的函数(DBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 输出单行简介
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string SimpleLineSummary(string theString)
        {
            theString = theString.Replace("&gt;", "");
            theString = theString.Replace("&lt;", "");
            theString = theString.Replace(" &nbsp;", "  ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "'");
            theString = theString.Replace("<br/> ", "\r\n");
            theString = theString.Replace("\"", "");
            theString = theString.Replace("\t", " ");
            theString = theString.Replace("\r", " ");
            theString = theString.Replace("\n", " ");
            theString = Regex.Replace(theString, "\\s{2,}", " ");
            return theString;
        }
        /// <summary> 
        /// UBB代码处理函数 
        /// </summary> 
        /// <param name="content">输入字符串</param> 
        /// <returns>输出字符串</returns> 
        public static string UBB2HTML(string content)  //ubb转html
        {
            content = Regex.Replace(content, @"\[b\](.+?)\[/b\]", "<b>$1</b>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[i\](.+?)\[/i\]", "<i>$1</i>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[u\](.+?)\[/u\]", "<u>$1</u>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[p\](.+?)\[/p\]", "<p>$1</p>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[align=left\](.+?)\[/align\]", "<align='left'>$1</align>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[align=center\](.+?)\[/align\]", "<align='center'>$1</align>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[align=right\](.+?)\[/align\]", "<align='right'>$1</align>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[url=(?<url>.+?)]\[/url]", "<a href='${url}' target=_blank>${url}</a>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[url=(?<url>.+?)](?<name>.+?)\[/url]", "<a href='${url}' target=_blank>${name}</a>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[quote](?<text>.+?)\[/quote]", "<div class=\"quote\">${text}</div>", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"\[img](?<img>.+?)\[/img]", "<a href='${img}' target=_blank><img src='${img}' alt=''/></a>", RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 将html转成js代码,不完全和原始数据一致
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Html2Js(string source)
        {
            return String.Format("document.write(\"{0}\");",
                String.Join("\");\r\ndocument.write(\"", source.Replace("\\", "\\\\")
                                        .Replace("/", "\\/")
                                        .Replace("'", "\\'")
                                        .Replace("\"", "\\\"")
                                        .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            ));
        }
        /// <summary>
        /// 将html转成可输出的js字符串,不完全和原始数据一致
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Html2JsStr(string source)
        {
            return String.Format("{0}",
                String.Join(" ", source.Replace("\\", "\\\\")
                                        .Replace("/", "\\/")
                                        .Replace("'", "\\'")
                                        .Replace("\"", "\\\"")
                                        .Replace("\t", "")
                                        .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                            ));
        }
        /// <summary>
        /// 过滤所有特殊特号
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string FilterSymbol(string theString)
        {
            string[] aryReg = { "'", "\"", "\r", "\n", "<", ">", "%", "?", ",", ".", "=", "-", "_", ";", "|", "[", "]", "&", "/" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                theString = theString.Replace(aryReg[i], string.Empty);
            }
            return theString;
        }
        /// <summary>
        /// 过滤所有特殊特号，只允许逗号、分号和小数点
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string DelSymbol(string theString)
        {
            string[] aryReg = { "'", "\"", "\r", "\n", "<", ">", "%", "?", "=", "-", "_", "|", "[", "]", "&", "/" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                theString = theString.Replace(aryReg[i], string.Empty);
            }
            return theString;
        }
        /// <summary>
        /// 过滤一般特殊特号,主要用于过滤标题
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string SafetyTitle(string theString)
        {
            string[] aryReg = { "'", ";", "\"", "\r", "\n" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                theString = theString.Replace(aryReg[i], string.Empty);
            }
            return theString;
        }
        /// <summary>
        /// 得到安全的sql关键词
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string SafetyLikeValue(string theString)
        {
            string[] aryReg = { "'", ";", "\"", "\r", "\n", "%", "-", "[", "]", "(", ")" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                theString = theString.Replace(aryReg[i], string.Empty);
            }
            return theString;
        }
        /// <summary>
        /// 正则表达式取值
        /// </summary>
        /// <param name="HtmlCode">HTML代码</param>
        /// <param name="RegexString">正则表达式</param>
        /// <param name="GroupKey">正则表达式分组关键字</param>
        /// <param name="RightToLeft">是否从右到左</param>
        /// <returns></returns>
        public static string[] GetRegValue(string HtmlCode, string RegexString, string GroupKey, bool RightToLeft)
        {
            MatchCollection m;
            Regex r;
            if (RightToLeft == true)
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.RightToLeft);
            }
            else
            {
                r = new Regex(RegexString, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
            m = r.Matches(HtmlCode);
            string[] MatchValue = new string[m.Count];
            for (int i = 0; i < m.Count; i++)
            {
                MatchValue[i] = m[i].Groups[GroupKey].Value;
            }
            return MatchValue;
        }
        /// <summary>
        /// 获得标签的属性值
        /// </summary>
        /// <param name="HtmlTag"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        public static string AttributeValue(string HtmlTag, string AttributeName)
        {
            //前缀符号，要么为空，要么是空格/双引号/单引号/竖线/冒号……你还可以自己加入其他的符号
            string prefixCHAR = (HtmlTag.StartsWith(AttributeName + "=")) ? "(.{0})" : "([\"'\\s\\|:]{1})";
            string RegexString = prefixCHAR + AttributeName + "=(\"|')(?<" + AttributeName + ">.*?[^\\\\]{1})(\\2)";
            string[] _att = GetRegValue(HtmlTag, RegexString, AttributeName, false);
            if (_att.Length > 0)
                return _att[0].ToString();
            else
                return "";
        }
        /// <summary>        
        /// 格式化显示时间为几个月,几天前,几小时前,几分钟前,或几秒前        
        /// </summary>        
        /// <param name="dt">要格式化显示的时间</param>        
        /// <returns>几个月,几天前,几小时前,几分钟前,或几秒前</returns>        
        public static string DateStringFromNow(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60) { return dt.ToShortDateString(); }
            else if (span.TotalDays > 30) { return "1个月前"; }
            else if (span.TotalDays > 14) { return "2周前"; }
            else if (span.TotalDays > 7) { return "1周前"; }
            else if (span.TotalDays > 1) { return string.Format("{0}天前", (int)Math.Floor(span.TotalDays)); }
            else if (span.TotalHours > 1) { return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours)); }
            else if (span.TotalMinutes > 1) { return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes)); }
            else if (span.TotalSeconds >= 1) { return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds)); }
            else { return "1秒前"; }
        }
        #region 根据头、尾来截断字符串内容
        /// <summary>
        /// <para>获取截取内容数组:不包含头尾</para> 
        /// <para>    sHtml(原文内容)</para> 
        /// <para>    strStart(开头内容)</para> 
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <returns></returns>
        public static ArrayList GetHtmls(string sHtml, string strStart, string strEnd)
        {
            return returnJSArrayResult(sHtml, strStart, strEnd);
        }
        /// <summary>
        /// <para>获取截取内容数组:自定义头尾</para> 
        /// <para>    sHtml(原文内容)</para> 
        /// <para>    strStart(开头内容)</para> 
        /// <para>    strEnd(结束内容)</para> 
        /// <para>    getStart(是否包含头内容)</para> 
        /// <para>    getEnd(是否包含尾内容)</para> 
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <param name="getStart">是否包含头内容</param>
        /// <param name="getEnd">是否包含尾内容</param>
        /// <returns></returns>
        public static ArrayList GetHtmls(string sHtml, string strStart, string strEnd, bool getStart, bool getEnd)
        {
            return returnJSArrayResult(sHtml, strStart, strEnd, getStart, getEnd);
        }
        /// <summary>
        /// <para>获取截取内容字符串:不包含头尾</para> 
        /// <para>    sHtml(原文内容)</para> 
        /// <para>    strStart(开头内容)</para> 
        /// <para>    strEnd(结束内容)</para> 
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <returns></returns>
        public static string GetHtml(string sHtml, string strStart, string strEnd)
        {
            return returnJSResult(sHtml, strStart, strEnd);
        }
        /// <summary>
        /// <para>获取截取内容字符串:自定义头尾</para> 
        /// <para>    sHtml(原文内容)</para> 
        /// <para>    strStart(开头内容)</para> 
        /// <para>    strEnd(结束内容)</para> 
        /// <para>    getStart(是否包含头内容)</para> 
        /// <para>    getEnd(是否包含尾内容)</para> 
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <param name="getStart">是否包含头内容</param>
        /// <param name="getEnd">是否包含尾内容</param>
        /// <returns></returns>
        public static string GetHtml(string sHtml, string strStart, string strEnd, bool getStart, bool getEnd)
        {
            return returnJSResult(sHtml, strStart, strEnd, getStart, getEnd);
        }
        /// <summary>
        /// 先将一些特殊东西替换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string enReplaceStr(string str)
        {
            if ((str == null) || (str == ""))
            {
                return "stringgggg_空值";
            }
            return str.Replace("\r", "stringgggg_回车").Replace("\n", "stringgggg_换行").Replace("\"", "stringgggg_双引").Replace("\\", "stringgggg_反斜");
        }
        /// <summary>
        /// 最后还原那些特殊的东西
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string deReplaceStr(string str)
        {
            return str.Replace("stringgggg_回车", "\r").Replace("stringgggg_换行", "\n").Replace("stringgggg_双引", "\"").Replace("stringgggg_反斜", "\\").Replace("stringgggg_空值", "").Replace("stringgggg_空头", "").Replace("stringgggg_空尾", "");
        }

        private static string GetRegexResult(Regex re, string sign, string matchStr)
        {
            Match match = re.Match(matchStr);
            return re.Replace(match.ToString(), sign);
        }

        /// <summary>
        /// 获取截取内容数组:不包含头尾
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <returns></returns>
        private static ArrayList returnJSArrayResult(string sHtml, string strStart, string strEnd)
        {
            return returnJSArrayResult(sHtml, strStart, strEnd, false, false);
        }
        /// <summary>
        /// 获取截取内容数组:自定义头尾
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <param name="getStart">是否包含头内容</param>
        /// <param name="getEnd">是否包含尾内容</param>
        /// <returns></returns>
        private static ArrayList returnJSArrayResult(string sHtml, string strStart, string strEnd, bool getStart, bool getEnd)
        {
            if ((strEnd == null) || (strEnd == ""))
            {
                sHtml = sHtml + "stringgggg_空尾";
                strEnd = "stringgggg_空尾";
            }
            if ((strStart == null) || (strStart == ""))
            {
                sHtml = "stringgggg_空头" + sHtml;
                strStart = "stringgggg_空头";
            }
            ArrayList list = new ArrayList();
            Regex re = new Regex(returnRegexStr(enReplaceStr(strStart), enReplaceStr(strEnd)));
            MatchCollection matchs = re.Matches(enReplaceStr(sHtml));
            for (int i = 0; i < matchs.Count; i++)
            {
                string matchStr = deReplaceStr(GetRegexResult(re, "${url}", matchs[i].Value));
                if (matchStr.Contains(strStart))
                {//2011-03-07
                    matchStr = returnJSResult(matchStr + strEnd, strStart, strEnd, false, false);
                }
                if (getStart) matchStr = strStart + matchStr;
                if (getEnd) matchStr = matchStr + strEnd;
                list.Add(matchStr);
            }
            return list;
        }
        /// <summary>
        /// 获取截取内容:不包含头尾
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <returns></returns>
        private static string returnJSResult(string sHtml, string strStart, string strEnd)
        {
            return returnJSResult(sHtml, strStart, strEnd, false, false);
        }
        /// <summary>
        /// 获取截取内容:自定义头尾
        /// </summary>
        /// <param name="sHtml">原文内容</param>
        /// <param name="strStart">开头内容</param>
        /// <param name="strEnd">结束内容</param>
        /// <param name="getStart">是否包含头内容</param>
        /// <param name="getEnd">是否包含尾内容</param>
        /// <returns></returns>
        private static string returnJSResult(string sHtml, string strStart, string strEnd, bool getStart, bool getEnd)
        {
            if ((strEnd == null) || (strEnd == ""))
            {
                sHtml = sHtml + "stringgggg_空尾";
                strEnd = "stringgggg_空尾";
            }
            if ((strStart == null) || (strStart == ""))
            {
                sHtml = "stringgggg_空头" + sHtml;
                strStart = "stringgggg_空头";
            }
            string code = JSStr();
            ScriptControlClass class2 = new ScriptControlClass();
            class2.Language = "javascript";
            class2.AddCode(code);
            object obj2 = class2.Eval(string.Format("Main(\"{0}\",\"{1}\",\"{2}\")", enReplaceStr(strStart), enReplaceStr(strEnd), enReplaceStr(sHtml)));
            string matchStr = deReplaceStr(obj2.ToString());
            if (getStart) matchStr = strStart + matchStr;
            if (getEnd) matchStr = matchStr + strEnd;
            return matchStr;
        }
        private static string returnRegexStr(string strStart, string strEnd)
        {
            string code = JSStr();
            ScriptControlClass class2 = new ScriptControlClass();
            class2.Language = "javascript";
            class2.AddCode(code);
            return class2.Eval(string.Format("GetRegex(\"{0}\",\"{1}\",\"{2}\")", strStart, strEnd, "url")).ToString();
        }
        private static string JSStr()
        {
            //return "function Main(s1,s2,s3)\r\n{\r\n\tvar s = GetRegex(s1, s2);\r\n      return MatchString(s,s3);\r\n}\r\n\r\nfunction GetRegex(str1, str2, group)\r\n\t\t\t{\r\n\t\t\t\tif (str1.length==0 || str2.length==0){\r\n\t\t\t\t\treturn '';\r\n\t\t\t\t}\r\n\t\t\t\tvar exs = new Array(/\\\\/gi, /\\^/gi, /\\$/gi, /\\{/gi, /\\[/gi, /\\./gi, /\\(/gi,/\\)/gi, /\\*/gi, /\\+/gi, /\\?/gi, /\\!/gi, /\\#/gi,/\\|/gi);\r\n\t\t\t\tvar chars = new Array('\\\\', '^', '$', '{', '[', '.', '(',')', '*', '+', '?', '!', '#', '|');\r\n\t\t\t\tfor(i=0; i<exs.length; i++){\r\n\t\t\t\t\tstr1 = str1.replace(exs[i],'\\\\'+chars[i]);\r\n\t\t\t\t\tstr2 = str2.replace(exs[i],'\\\\'+chars[i]);\r\n\t\t\t\t}\r\n\t\t\t\tif (group==null){\r\n\t\t\t\t\tstr1 = str1.replace(/\\r/ig,'\\\\s').replace(/\\n/ig,'\\\\s');\r\n\t\t\t\t\tstr2 = str2.replace(/\\r/ig,'\\\\s').replace(/\\n/ig,'\\\\s');\r\n\t\t\t\t\treturn str1 +'((.|\\\\n)+?)'+ str2\r\n\t\t\t\t}else{\r\n\t\t\t\t\treturn str1 +'(?<'+group+'>.+?)'+ str2\r\n\t\t\t\t}\r\n\t\t\t}\r\n\r\nfunction MatchString(s,d)\r\n\t\t\t{\r\n\t\t\t\tvar re, arr;\r\n\t\t\t\teval('re=/'+ s.replace(/\\//g,'\\\\/') +'/igm;');\r\n\t\t\t\tarr = re.exec(d);\r\n\t\t\t\tif (arr!=null){\r\n\t\t\t\t\ts = '';\r\n\t\t\t\t\t\ts += arr[1];\r\n\t\t\t\t\treturn s;\r\n\t\t\t\t}else{\r\n\t\t\t\t\treturn ('没匹配的内容');\r\n\t\t\t\t}\r\n\t\t\t}";
            return "function Main(s1,s2,s3){var s = GetRegex(s1, s2);var Result = MatchString(s,s3);if(Result.length<2 || Result.indexOf(s1)<1){return Result;}else{return Main(s1,s2,Result+s2);}}\r\nfunction GetRegex(str1, str2, group)\r\n\t\t\t{\r\n\t\t\t\tif (str1.length==0 || str2.length==0){\r\n\t\t\t\t\treturn '';\r\n\t\t\t\t}\r\n\t\t\t\tvar exs = new Array(/\\\\/gi, /\\^/gi, /\\$/gi, /\\{/gi, /\\[/gi, /\\./gi, /\\(/gi,/\\)/gi, /\\*/gi, /\\+/gi, /\\?/gi, /\\!/gi, /\\#/gi,/\\|/gi);\r\n\t\t\t\tvar chars = new Array('\\\\', '^', '$', '{', '[', '.', '(',')', '*', '+', '?', '!', '#', '|');\r\n\t\t\t\tfor(i=0; i<exs.length; i++){\r\n\t\t\t\t\tstr1 = str1.replace(exs[i],'\\\\'+chars[i]);\r\n\t\t\t\t\tstr2 = str2.replace(exs[i],'\\\\'+chars[i]);\r\n\t\t\t\t}\r\n\t\t\t\tif (group==null){\r\n\t\t\t\t\tstr1 = str1.replace(/\\r/ig,'\\\\s').replace(/\\n/ig,'\\\\s');\r\n\t\t\t\t\tstr2 = str2.replace(/\\r/ig,'\\\\s').replace(/\\n/ig,'\\\\s');\r\n\t\t\t\t\treturn str1 +'((.|\\\\n)+?)'+ str2\r\n\t\t\t\t}else{\r\n\t\t\t\t\treturn str1 +'(?<'+group+'>.+?)'+ str2\r\n\t\t\t\t}\r\n\t\t\t}\r\n\r\nfunction MatchString(s,d)\r\n\t\t\t{\r\n\t\t\t\tvar re, arr;\r\n\t\t\t\teval('re=/'+ s.replace(/\\//g,'\\\\/') +'/igm;');\r\n\t\t\t\tarr = re.exec(d);\r\n\t\t\t\tif (arr!=null){\r\n\t\t\t\t\ts = '';\r\n\t\t\t\t\t\ts += arr[1];\r\n\t\t\t\t\treturn s;\r\n\t\t\t\t}else{\r\n\t\t\t\t\treturn ('');\r\n\t\t\t\t}\r\n\t\t\t}";//2011-03-07
        }
        #endregion
    }
}
