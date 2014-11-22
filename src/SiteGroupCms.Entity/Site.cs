using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 当前操作的站点信息
    /// </summary>
    public class Site
    {
        public Site()
        { }
        private int id;
        private string title;
        private string location;
        private string webtitle;
        private string description;
        private string m_Keywords;
        private string m_Description;
        private int uploadsize;
        private string uploadtype;
        private string emailserver;
        private string emailuser;
        private string emailpwd;
        private string ftpserver;
        private int ftpport;
        private string ftpuser;
        private string ftppwd;
        private string ftpdir;
        private int type;
        private int iswork;
        private string domain;
        private int indextemplate;
        private int listtemplate;
        private int contenttemplate;


        /// <summary>
        /// 网站id
        /// </summary>
        public int ID
        {
            set { id = value; }
            get { return id; }
        }
        /// <summary>
        /// 网站后台显示的名字
        /// </summary>
        public string Title
        {
            set { title = value; }
            get { return title; }
        }
        /// <summary>
        /// 网站文件存放位置
        /// </summary>
        public string Location
        {
            set { location = value; }
            get { return location; }
        }
        /// <summary>
        /// 网站前台显示的名称
        /// </summary>
        public string WebTitle
        {
            set { webtitle = value; }
            get { return webtitle; }
        }
        /// <summary>
        /// 网站简介
        /// </summary>
        public string Description
        {
            set { description = value; }
            get { return description; }
        }
        /// <summary>
        /// 网站关键字
        /// </summary>
        public string Keyword
        {
            set { m_Keywords = value; }
            get { return m_Keywords; }
        }
        /// <summary>
        /// 网站供引擎首艘的简介
        /// </summary>
        public string Meta_Description
        {
            set { m_Description = value; }
            get { return m_Description; }
        }
        /// <summary>
        /// 网站允许上传的文件大小限制
        /// </summary>
        public int UploadSize
        {
            set { uploadsize = value; }
            get { return uploadsize; }
        }
        /// <summary>
        /// 网站允许上传的文件格式
        /// </summary>
        public string UploadType
        {
            set { uploadtype = value; }
            get { return uploadtype; }
        }
        /// <summary>
        /// 网站emailserver
        /// </summary>
        public string EmailServer
        {
            set { emailserver = value; }
            get { return emailserver; }
        }
        /// <summary>
        /// 网站全email user
        /// </summary>
        public string EmailUser
        {
            set { emailuser = value; }
            get { return emailuser; }
        }
        /// <summary>
        /// 网站email pwd
        /// </summary>
        public string EmailPwd
        {
            set { emailpwd = value; }
            get { return emailpwd; }
        }
        /// <summary>
        /// 网站Ftpserver
        /// </summary>
        public string FtpServer
        {
            set { ftpserver = value; }
            get { return ftpserver; }
        }
        /// <summary>
        /// 网站ftpport
        /// </summary>
        public int FtpPort
        {
            set { ftpport = value; }
            get { return ftpport; }
        }
        /// <summary>
        /// 网站ftp密码
        /// </summary>
        public string FtpPwd
        {
            set { ftppwd = value; }
            get { return ftppwd; }
        }
        /// <summary>
        /// 网站ftp上传路径
        /// </summary>
        public string FtpDir
        {
            set { ftpdir = value; }
            get { return ftpdir; }
        }
        /// <summary>
        /// 网站ftp用户名
        /// </summary>
        public string FtpUser
        {
            set { ftpuser = value; }
            get { return ftpuser; }
        }
        /// <summary>
        /// 网站类型
        /// </summary>
        public int Type
        {
            set { type = value; }
            get { return type; }
        }
        /// <summary>
        /// 网站是否工作
        /// </summary>
        public int IsWork
        {
            set { iswork = value; }
            get { return iswork; }
        }
        /// <summary>
        /// 网站域名
        /// </summary>
        public string Domain
        {
            set { domain = value; }
            get { return domain; }
        }
        /// <summary>
        /// 首页模板
        /// </summary>
        public int Indextemplate    
        {
            set { indextemplate = value; }
            get { return indextemplate; }
        }
        /// <summary>
        /// 频道页模板
        /// </summary>
        public int Listtemplate
        {
            set { listtemplate = value; }
            get { return listtemplate; }
        }
        /// <summary>
        /// 内同页模板
        /// </summary>
        public int Contenttemplate
        {
            set { contenttemplate = value; }
            get { return contenttemplate; }
        }
    }
}
