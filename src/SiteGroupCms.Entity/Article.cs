using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{    /// <summary>
    /// 文章-------信息映射实体
    /// </summary>
  public  class Article
    {
      private int id;
      private string title;
      private string subtitle;
      private string keywords;
      private string abstraction;
      private int siteid;
      private int catalogid;
      private string linkurl;
      private string author;
      private int passuserid;
      private string source;
      private DateTime addtime;
      private DateTime passtime;
      private DateTime publishtime;
      private int ispass;
      private int isdel;
      private int isrecommend;
      private int isppt;
      private int isroll;
      private int ispublish;
      private int isshare;
      private int templates;
      private string color;
      private string content;
      private int clickcount;
      private int yyarticleid;

      /// <summary>
      /// 文章的ID
      /// </summary>
      public int Id
      {
          set { id = value; }
          get { return id; }
      }
      public int Clickcount
      {
          set { clickcount = value; }
          get { return clickcount; }
      }
      public int Yyarticleid
      {
          set { yyarticleid = value; }
          get { return yyarticleid; }
      }
      /// <summary>
      /// 标题
      /// </summary>
      public string Title
      {
          set { title = value; }
          get { return title; }
      }
      /// <summary>
      ///颜色
      /// </summary>
      public string Color
      {
          set { color = value; }
          get { return color; }
      }
      /// <summary>
      ///内容
      /// </summary>
      public string Content
      {
          set { content = value; }
          get { return content; }
      }
      /// <summary>
      /// 子标题
      /// </summary>
      public string Subtitle
      {
          set { subtitle = value; }
          get { return subtitle; }
      }
      /// <summary>
      /// 关键字
      /// </summary>
      public string Keywords
      {
          set { keywords = value; }
          get { return keywords; }
      }
      /// <summary>
      /// 摘要
      /// </summary>
      public string Abstract
      {
          set { abstraction = value; }
          get { return abstraction; }
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
      /// 栏目ID
      /// </summary>
      public int Catalogid
      {
          set { catalogid = value; }
          get { return catalogid; }
      }
      /// <summary>
      /// 链接
      /// </summary>
      public string Linkurl
      {
          set { linkurl = value; }
          get { return linkurl; }
      }
      /// <summary>
      /// 作者
      /// </summary>
      public string Author
      {
          set { author = value; }
          get { return author; }
      }
      /// <summary>
      /// 审核者id
      /// </summary>
      public int Passuserid
      {
          set { passuserid = value; }
          get { return passuserid; }
      }
      /// <summary>
      /// 来源
      /// </summary>
      public string Source
      {
          set { source = value; }
          get { return source; }
      }
      /// <summary>
      /// 添加时间
      /// </summary>
      public DateTime Addtime
      {
          set { addtime = value; }
          get { return addtime; }
      }
      /// <summary>
      /// 通过时间
      /// </summary>
      public DateTime PassTime
      {
          set { passtime = value; }
          get { return passtime; }
      }
      /// <summary>
      /// 发布时间
      /// </summary>
      public DateTime Publishtime
      {
          set { publishtime = value; }
          get { return publishtime; }
      }
      /// <summary>
      /// 是否通过
      /// </summary>
      public int Ispass
      {
          set { ispass = value; }
          get { return ispass; }
      }
      /// <summary>
      /// 是否删除
      /// </summary>
      public int IsDel
      {
          set { isdel = value; }
          get { return isdel; }
      }

      /// <summary>
      /// 是否为推荐
      /// </summary>
      public int Isrecommend
      {
          set { isrecommend = value; }
          get { return isrecommend; }
      }
      /// <summary>
      /// 是否为幻灯片
      /// </summary>
      public int Isppt
      {
          set { isppt = value; }
          get { return isppt; }
      }
      /// <summary>
      /// 是否为滚动新闻
      /// </summary>
      public int Isroll
      {
          set { isroll = value; }
          get { return isroll; }
      }
      /// <summary>
      ///是否已经发布了
      /// </summary>
      public int Ispublish
      {
          set { ispublish = value; }
          get { return ispublish; }
      }
      /// <summary>
      /// 是否共享
      /// </summary>
      public int Isshare
      {
          set { isshare = value; }
          get { return isshare; }
      }
      /// <summary>
      /// 模板id
      /// </summary>
      public int Templateid
      {
          set { templates = value; }
          get { return templates; }
      }

    }
}
