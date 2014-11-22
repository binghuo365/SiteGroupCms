using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
namespace SiteGroupCms.Utils
{
    /// <summary>
    /// 用来和聊天机器人xiaoi通信的类
    /// </summary>
    public class Xiaoi
    {
        private String m_strSID;

        public static Xiaoi xiaoi = new Xiaoi();

        /// <summary>
        /// 在构造函数中申请了一个SID，并初始化这个SID
        /// </summary>
        private Xiaoi()
        {
            //! 用以取得SID
            HttpWebRequest hwrSIDReq = (HttpWebRequest)HttpWebRequest.Create("http://122.226.240.164/engine/flashrobot2/webbot.js?encoding=utf-8");
            hwrSIDReq.Method = "GET";
            WebResponse wrSID = hwrSIDReq.GetResponse();
            WebHeaderCollection wcHeaders = wrSID.Headers;
            string strSetCookie = wcHeaders["Set-Cookie"];
            wrSID.Close();

            Regex RegexSID = new Regex("ibotsessionid=(\\d+)");

            Match mValues = RegexSID.Match(strSetCookie);
            GroupCollection gcValues = mValues.Groups;
            if (gcValues.Count > 1)
            {
                string strSid = gcValues[1].Captures[0].Value;
                m_strSID = strSid;

                //! 初始化对话
                String strJoinAddr = "http://122.226.240.164/engine/flashrobot2/send.js?encoding=utf-8&SID=" + strSid + "&USR=" + strSid + "&CMD=JOIN&r=";
                HttpWebRequest hwrJoinReq = (HttpWebRequest)HttpWebRequest.Create(strJoinAddr);
                hwrJoinReq.Method = "GET";
                hwrJoinReq.GetResponse().Close();

                String strRecTAddr = "http://122.226.240.164/engine/flashrobot2/recv.js?encoding=utf-8&SID=" + strSid + "&USR=" + strSid + "&r=";
                HttpWebRequest hwrRecTReq = (HttpWebRequest)HttpWebRequest.Create(strRecTAddr);
                hwrRecTReq.Method = "GET";
                hwrRecTReq.KeepAlive = true;
                hwrRecTReq.GetResponse().Close();
            }
        }

        /// <summary>
        /// 使用此函数和小I聊天
        /// </summary>
        /// <param name="Message">聊天的内容</param>
        /// <returns>小I的回答</returns>
        public String chatXiaoi(String Message)
        {
            //! 发送消息
            String strSendAddr = "http://122.226.240.164/engine/flashrobot2/send.js?encoding=utf-8&SID=" + m_strSID + "&USR=" + m_strSID + "&CMD=CHAT&SIG=You&MSG=" + Message + "&FTN=&FTS=&FTC=&r=";
            HttpWebRequest hwrSendReq = (HttpWebRequest)HttpWebRequest.Create(strSendAddr);
            hwrSendReq.Method = "GET";
            hwrSendReq.KeepAlive = false;
            hwrSendReq.GetResponse().Close();

            //! 接受消息
            String strRecAddr = "http://122.226.240.164/engine/flashrobot2/recv.js?encoding=utf-8&SID=" + m_strSID + "&USR=" + m_strSID + "&r=";
            HttpWebRequest hwrRecReq = (HttpWebRequest)HttpWebRequest.Create(strRecAddr);
            hwrSendReq.Method = "GET";
            hwrSendReq.KeepAlive = false;
            WebResponse wrRecMessage = hwrRecReq.GetResponse();

            Stream ReceiveStream = wrRecMessage.GetResponseStream();
            StreamReader readStream = new StreamReader(ReceiveStream, Encoding.UTF8);
            Char[] read = new Char[256];

            // Read 256 charcters at a time.    
            int count = readStream.Read(read, 0, 256);
            string str = "";
            while (count > 0)
            {
                str += new String(read, 0, count);
                count = readStream.Read(read, 0, 256);
            }

            //! 把读出的消息处理，删除无用部分
            Regex RegexMSG = new Regex("\"MSG\":\"(.*?)\"");
            String strContent = null;
            Match msgValues = RegexMSG.Match(str);
            GroupCollection gcMsgValues = msgValues.Groups;
            if (gcMsgValues.Count > 1)
            {
                strContent = gcMsgValues[1].Captures[0].Value;
            }

            // Release the resources of stream object.
            readStream.Close();

            // Release the resources of response object.
            wrRecMessage.Close();

            if (string.IsNullOrEmpty(strContent))
            {
                xiaoi = new Xiaoi();
                return "正在睡觉，别打扰我。。。";
            }

            return strContent;
        }

    }
}
