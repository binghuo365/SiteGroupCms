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
using System.Web;
using System.Web.Caching;
using System.Text;

namespace SiteGroupCms.Utils
{
    /// <summary>
    /// App操作类
    /// </summary>
    public static class App
    {
        public static string Url
        {
            get
            {
                if (HttpContext.Current.Request.Url.Port == 80)
                    return "http://" + HttpContext.Current.Request.Url.Host;
                else
                    return "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            }
        }
        /// <summary>
        /// 应用程序路径，以/结尾
        /// </summary>
        /// <returns>如:/，/cms/</returns>
        public static string Path
        {
            get
            {
                string _ApplicationPath = System.Web.HttpContext.Current.Request.ApplicationPath;
                if (_ApplicationPath != "/")
                    _ApplicationPath += "/";
                return _ApplicationPath;
            }
        }
    }
}
