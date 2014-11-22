using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 栏目-------表映射实体
    /// </summary>

    public class Normal_Class
    {
        public Normal_Class()
        { }

        private string _id;
        private int _channelid;
        private int _parentid;
        private string _title;
        private string _info;
        private string _img;
        private string _filepath;
        private string _code;
        private bool _ispost;
        private bool _istop;
        private int _topicnum;
        private string _templateid;
        private string _contenttemp;
        private int _pagesize;
        private bool _isout;
        private string _firstpage;
        private string _aliaspage;
        private int _readgroup;
        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 所属频道ID
        /// </summary>
        public int ChannelId
        {
            set { _channelid = value; }
            get { return _channelid; }
        }
        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 栏目简介
        /// </summary>
        public string Info
        {
            set { _info = value; }
            get { return _info; }
        }
        /// <summary>
        /// 封面图
        /// </summary>
        public string Img
        {
            set { _img = value; }
            get { return _img; }
        }
        /// <summary>
        /// 栏目目录
        /// </summary>
        public string FilePath
        {
            set { _filepath = value; }
            get { return _filepath; }
        }
        /// <summary>
        /// 栏目代码，用其来关联父子和兄弟关系
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
        /// <summary>
        /// 是否会员可投稿
        /// </summary>
        public bool IsPost
        {
            set { _ispost = value; }
            get { return _ispost; }
        }
        /// <summary>
        /// 是否导航
        /// </summary>
        public bool IsTop
        {
            set { _istop = value; }
            get { return _istop; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TopicNum
        {
            set { _topicnum = value; }
            get { return _topicnum; }
        }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string TemplateId
        {
            set { _templateid = value; }
            get { return _templateid; }
        }
        /// <summary>
        /// 内容页模板ID
        /// </summary>
        public string ContentTemp
        {
            set { _contenttemp = value; }
            get { return _contenttemp; }
        }
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            set { _pagesize = value; }
            get { return _pagesize; }
        }
        /// <summary>
        /// 外部栏目
        /// </summary>
        public bool IsOut
        {
            set { _isout = value; }
            get { return _isout; }
        }
        /// <summary>
        /// 外部链接地址
        /// </summary>
        public string FirstPage
        {
            set { _firstpage = value; }
            get { return _firstpage; }
        }
        public string AliasPage
        {
            set { _aliaspage = value; }
            get { return _aliaspage; }
        }
        /// <summary>
        /// 最低阅读会员组
        /// </summary>
        public int ReadGroup
        {
            set { _readgroup = value; }
            get { return _readgroup; }
        }
    }
}
