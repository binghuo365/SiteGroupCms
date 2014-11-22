/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序版本: 5.2.X
 * 
 * 程序作者: 高伟 ( 合作请联系：254860396#qq.com)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.IO;
namespace SiteGroupCms.Utils
{    /// <summary>
    /// 提供对XML文档尤其以config结尾的xml文档操作接口。
    /// </summary>
    public static class XmlCOM
    {
        public static DataSet ReadXml(string path)
        {
            DataSet ds = new DataSet();
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fs, System.Text.Encoding.UTF8);
                ds.ReadXml(reader);
                return ds;
            }
            finally
            {
                fs.Close();
                reader.Close();
            }
        }
        public static DataSet ReadxdXml(string path)
        {
            DataSet ds = new DataSet();
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                fs = new FileStream(HttpContext.Current.Server.MapPath(path), FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fs, System.Text.Encoding.UTF8);
                ds.ReadXml(reader);
                return ds;
            }
            finally
            {
                fs.Close();
                reader.Close();
            }
        }
        /// <summary>
        /// 读取Config参数
        /// </summary>
        public static string ReadConfig(string name, string key)
        {
            System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
            xd.Load(HttpContext.Current.Server.MapPath(name + ".config"));
            System.Xml.XmlNodeList xnl = xd.GetElementsByTagName(key);
            if (xnl.Count == 0)
                return "";
            else
            {
                System.Xml.XmlNode mNode = xnl[0];
                return mNode.InnerText;
            }
        }
        /// <summary>
        /// 读取Config参数
        /// </summary>
        public static string[] ReadConfigs(string name, string key)
        {
            System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
            xd.Load(HttpContext.Current.Server.MapPath(name + ".config"));
            System.Xml.XmlNodeList xnl = xd.GetElementsByTagName(key);
            string[] str = new string[50];
            if (xnl.Count == 0)
                return null;
            else
            {
                for (int i = 0; i < xnl.Count; i++)
                {
                    str[i] = xnl[i].InnerText;
                }
                return str;
            }
        }

        /// <summary>
        /// 保存Config参数
        /// </summary>
        public static void UpdateConfig(string name, string nKey, string nValue)
        {
            if (ReadConfig(name, nKey) != "")
            {
                System.Xml.XmlDocument XmlDoc = new System.Xml.XmlDocument();
                XmlDoc.Load(HttpContext.Current.Server.MapPath(name + ".config"));
                System.Xml.XmlNodeList elemList = XmlDoc.GetElementsByTagName(nKey);
                System.Xml.XmlNode mNode = elemList[0];
                mNode.InnerText = nValue;
                System.Xml.XmlTextWriter xw = new System.Xml.XmlTextWriter(new System.IO.StreamWriter(HttpContext.Current.Server.MapPath(name + ".config")));
                xw.Formatting = System.Xml.Formatting.Indented;
                XmlDoc.WriteTo(xw);
                xw.Close();
            }
        }
    }
}
