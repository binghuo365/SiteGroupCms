using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using SiteGroupCms.Utils;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 文章-------表映射实体
    /// </summary>
    public class Module_Article
    {
        public Module_Article()
        { }
        private string _id;
        private string _channelid;
        private string _title;
        private string _subtitle;
        private string _color;
        private DateTime _adddate;
        private DateTime _addtime;
        private string _abstract;
        private string _author;
        private string _keywords;
        private int _viewnum;
        private int _ispass;
        private int _isimg;
        private string _img;
        private int _istop;
        private string _atts;
        private int _isfocus;
        private string _source;
        private string _content;
        private int _clickcount;
        private string _linkurl;

        public string Atts
        {
            get { return _atts; }
            set { _atts = value; }
        }

        public string Linkurl
        {
            get { return _linkurl; }
            set { _linkurl = value; }
        }
        
        
        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        public int Clickcount
        {
            set { _clickcount = value; }
            get { return _clickcount; }
        }
        /// <summary>
        /// 频道编号
        /// </summary>
        public string catalogid
        {
            set { _channelid = value; }
            get { return _channelid; }
        }  
       
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 文章子标题
        /// </summary>
        public string Subtitle
        {
            set { _subtitle = value; }
            get { return _subtitle; }
        }
        /// <summary>
        /// 标题颜色
        /// </summary>
        public string Color
        {
            set { _color = value; }
            get { return _color; }
        }
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime AddDate
        {
            set { _adddate = value; }
            get { return _adddate; }
        }
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 简介
        /// </summary>
        public string Abstract
        {
            set { _abstract = value; }
            get { return _abstract; }
        }
       
        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            set { _author = value; }
            get { return _author; }
        }
        /// <summary>
        /// 标签
        /// </summary>
        public string keywords
        {
            set { _keywords = value; }
            get { return _keywords; }
        }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewNum
        {
            set { _viewnum = value; }
            get { return _viewnum; }
        }
        /// <summary>
        /// 状态(0表示未审,1表示审核)
        /// </summary>
        public int IsPassed
        {
            set { _ispass = value; }
            get { return _ispass; }
        }
        /// <summary>
        /// 是否有图片
        /// </summary>
        public int IsImg
        {
            set { _isimg = value; }
            get { return _isimg; }
        }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Img
        {
            set { _img = value; }
            get { return _img; }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public int IsTop
        {
            set { _istop = value; }
            get { return _istop; }
        }
        /// <summary>
        /// 是否焦点
        /// </summary>
        public int IsFocus
        {
            set { _isfocus = value; }
            get { return _isfocus; }
        }

        /// <summary>
        /// 内容来源/出处
        /// </summary>
        public string Source
        {
            set { _source = value; }
            get { return _source; }
        }
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
       
    }
}

