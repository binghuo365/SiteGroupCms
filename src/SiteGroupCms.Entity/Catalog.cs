using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 栏目信息类
    /// </summary>
   public class Catalog
    {
        private int id;
        private string title;
        private int fatherid;
        private string linkurl;
        private string type;
        private string picurl;
        private string description;
        private int siteid;
        private string dirname;
        private string meta_keywords;
        private string meta_description;
        private int ishare;
        private int listtemplate;
        private string contentfileex;
        private int contenttemplate;

        /// <summary>
        /// 栏目id
        /// </summary>
        public int ID
        {
            set { id = value; }
            get { return id; }
        }
        /// <summary>
        /// 站点id
        /// </summary>
        public int Siteid
        {
            set { siteid = value; }
            get { return siteid; }
        }
        /// <summary>
        /// 站点位置
        /// </summary>
        public string Dirname
        {
            set { dirname = value; }
            get { return dirname; }
        }
        /// <summary>
        /// 栏目标题
        /// </summary>
        public string Title
        {
            set { title = value; }
            get { return title; }
        }
        /// <summary>
        /// 伏击栏目id
        /// </summary>
        public int Father
        {
            set { fatherid = value; }
            get { return fatherid; }
        }
        /// <summary>
        /// 栏目链接地址
        /// </summary>
        public string Linkurl
        {
            set { linkurl = value; }
            get { return linkurl; }
        }
        public int ContentTemplate
        {
            set { contenttemplate = value; }
            get { return contenttemplate; }
        }
        /// <summary>
        /// 栏目类型
        /// </summary>
        public string Type
        {
            set { type = value; }
            get { return type; }
        }
        /// <summary>
        /// 栏目缩略图的url
        /// </summary>
        public string Picurl
        {
            set { picurl = value; }
            get { return picurl; }
        }
        /// <summary>
        /// 栏目简介
        /// </summary>
        public string Description
        {
            set { description = value; }
            get { return description; }
        }
        /// <summary>
        /// 栏目关键字
        /// </summary>
        public string Meta_Keywords
        {
            set { meta_keywords = value; }
            get { return meta_keywords; }
        }
        /// <summary>
        /// 栏目首艘简介
        /// </summary>
        public string Meta_description
        {
            set { meta_description = value; }
            get { return meta_description; }
        }
        /// <summary>
        /// 栏目是否共享
        /// </summary>
        public int IsShare
        {
            set { ishare = value; }
            get { return ishare; }
        }
        /// <summary>
        /// 栏目列表模板id
        /// </summary>
        public int Listtemplate
        {
            set { listtemplate = value; }
            get { return listtemplate; }
        }
        /// <summary>
        /// 栏目内容页文件扩展名
        /// </summary>
        public string Contentfileex
        {
            set { contentfileex = value; }
            get { return contentfileex; }
        }

    }

}
