using System;
using System.Net;
using System.IO;
using System.Text;
namespace SiteGroupCms.Utils
{
    /// <summary>
    /// SMS操作类
    /// </summary>
    public static class smsHelp
    {
        public static string ApiUid = XmlCOM.ReadConfig("~/_data/config/sms", "ApiUid");//用户名
        public static string ApiKey = XmlCOM.ReadConfig("~/_data/config/sms", "ApiKey");//密钥
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="smsMob">目的手机号码（多个手机号请用半角逗号隔开）</param>
        /// <param name="smsText">短信内容，最多支持300个字，63个字按一条短信计费</param>
        /// <returns></returns>
        public static string SendSMS(string smsMob, string smsText)
        {
            string _return = GetHtmlFromUrl("http://utf8.sms.webchinese.cn/?Uid=" + ApiUid + "&Key=" + ApiKey + "&smsMob=" + smsMob + "&smsText=" + smsText);
            switch (_return)
            {
                case "-1":
                    return "没有该用户账户";
                case "-2":
                    return "密钥不正确";
                case "-3":
                    return "短信数量不足";
                case "-11":
                    return "该用户被禁用";
                case "-14":
                    return "短信内容出现非法字符";
                case "-41":
                    return "手机号码为空";
                case "-42":
                    return "短信内容为空";
                default:
                    return "成功发送短信" + _return + "条";
            }
        }
        private static string GetHtmlFromUrl(string url)
        {
            string strRet = null;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = null;
            }
            return strRet;
        }

    }
}
