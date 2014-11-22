/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序作者: SiteGroupCms高伟 254850396#qq.com 
 */

using System;
namespace SiteGroupCms.DBUtility
{
    public class DbOperEventArgs : System.EventArgs
    {
        public int id;
        public DbOperEventArgs(int _id)
        {
            id = _id;
        }
    }
}
