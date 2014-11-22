using System;
using System.Data;
using System.Web;
using System.Collections.Generic;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 文章类的辅助控制层
    /// 新增、修改文章
    /// </summary>
    public partial class articleEditdo :SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Entity.Article article = new SiteGroupCms.Entity.Article();
        SiteGroupCms.Dal.ArticleDal artobj = new SiteGroupCms.Dal.ArticleDal();
        string method = string.Empty;
        string abstraction=string.Empty;
        string addtime = string.Empty;
        string artid = string.Empty;
        string attslist = string.Empty;
        string attstitlelist = string.Empty;
        string catalogid = string.Empty;
        string color = string.Empty;
        string content = string.Empty;
        string imglist = string.Empty;
        string imgtitlelist = string.Empty;
        string isppt = string.Empty;
        string isrecommend = string.Empty;
        string isroll = string.Empty;
        string isshare = string.Empty;
        string keywords = string.Empty;
        string linkurl = string.Empty;
        string source = string.Empty;
        string subtitle = string.Empty;
        string title = string.Empty;
        string _response = "";
        int clickcount = 0;
        int id = 0;
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);
                
            }
            method = Request.QueryString["method"];
            addtime = Request.Form["addtime"];
            artid = Request.Form["artid"];
            attslist = Request.Form["attslist"];
            attstitlelist = Request.Form["attstitlelist"];
            catalogid = Request.Form["catalogid"];
            color = Request.Form["color"];
            content = Request.Form["content1"];
            abstraction = Request.Form["abstract"];
            imglist = Request.Form["imglist"];
            imgtitlelist = Request.Form["imgtitlelist"];
            isppt = Request.Form["isppt"];
            isrecommend = Request.Form["isrecommend"];
            isroll = Request.Form["isroll"];
            isshare = Request.Form["isshare"];
            keywords = Request.Form["keywords"];
            linkurl = Request.Form["linkurl"];
            source = Request.Form["source"];
            subtitle = Request.Form["subtitle"];
            title = Request.Form["title"];
            clickcount = Str2Int(Request.Form["clickcount"]);
            switch (method)
            {
                case "add":
                    addarticle();
                    break;
                case "update":
                    updatearicle();
                    break;

            }
            Response.Write(_response);
        }

        private void addarticle()
        {
            //插入article
            article.Abstract = abstraction;
            article.Addtime =SiteGroupCms.Utils.Validator.StrToDate(addtime,DateTime.Now);
            article.Catalogid =Str2Int(catalogid);
            article.Siteid = _admin.CurrentSite;
            article.Author = _admin.UserName;
            article.Color = color;
            article.Content = content;
            article.Isppt = isppt=="true"?1:0;
            article.Isrecommend = isrecommend == "true" ? 1 : 0;
            article.Isshare = isshare == "true" ? 1 : 0;
            article.Isroll = isroll == "true" ? 1 : 0;
            article.Keywords = keywords;
            article.Linkurl = linkurl;
            article.Source = source;
            article.Subtitle = subtitle;
            article.Title = title;
            article.Clickcount = clickcount;
            id=artobj.insertEntity(article);
            artobj.updatsort(id.ToString());  //将排序号变得跟id一样
            if (id != 0)
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(3);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";

            //插入图片库  暂时不用
           
            if (imglist != "")
            {
                SiteGroupCms.Dal.ArticlepicDal picdal = new SiteGroupCms.Dal.ArticlepicDal();
                SiteGroupCms.Entity.Articlepic pic=new SiteGroupCms.Entity.Articlepic();
                string[] imgl=imglist.Split(',');
                string[] imgtl=imgtitlelist.Split(',');
                for (int i = 0; i <imgl.Length-1; i++)
                {
                    pic.Title =imgtl[i];
                    pic.Artid = id;
                    pic.Url = imgl[i];
                    pic.Istop = i == 1 ? 1 : 0;
                    picdal.InsertEntity(pic);
                }
            }
           
            //插入附件库
            if (attslist!="")
            {
                SiteGroupCms.Dal.ArticleattsDal attdal = new SiteGroupCms.Dal.ArticleattsDal();
                SiteGroupCms.Entity.Articleatts att = new SiteGroupCms.Entity.Articleatts();
                string[] attl = attslist.Split(',');
                string[] atttl = attstitlelist.Split(',');
                for (int i = 0; i < attl.Length-1; i++)
                {
                    att.Artid = id;
                    att.Title = atttl[i];
                    att.Url = attl[i];
                    attdal.InsertEntity(att);
                    
                }
                
            }
        
        }
        public void updatearicle()
        {
            //更新article
            article.Id =Str2Int(artid);
            article.Abstract = abstraction;
            article.Addtime = SiteGroupCms.Utils.Validator.StrToDate(addtime, DateTime.Now);
            article.Catalogid = Str2Int(catalogid);
            article.Siteid = _admin.CurrentSite;
            //article.Author = _admin.UserName;
            article.Color = color;
            article.Content = content;
            article.Isppt = isppt == "true" ? 1 : 0;
            article.Isrecommend = isrecommend == "true" ? 1 : 0;
            article.Isshare = isshare == "true" ? 1 : 0;
            article.Isroll = isroll == "true" ? 1 : 0;
            article.Keywords = keywords;
            article.Linkurl = linkurl;
            article.Source = source;
            article.Subtitle = subtitle;
            article.Title = title;
            article.Clickcount = clickcount;
            if (artobj.UpdateArticle(article))
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(28);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";

           //插入图片库 暂时不用

            if (imglist != "")
            {
                SiteGroupCms.Dal.ArticlepicDal picdal = new SiteGroupCms.Dal.ArticlepicDal();
                SiteGroupCms.Entity.Articlepic pic = new SiteGroupCms.Entity.Articlepic();
                List<SiteGroupCms.Entity.Articlepic> piclist = picdal.getEntityList(article.Id.ToString());
                string[] imgl = imglist.Split(',');
                string[] imgtl = imgtitlelist.Split(',');
                int[] flag0 = new int[piclist.Count]; //用来标记是否有更改
                for (int k = 0; k < flag0.Length; k++)
                {
                    flag0[k] = 0;
                }

                for (int i = 0; i < imgl.Length - 1; i++)
                {
                    if (piclist.Count > 0)//包含图片时
                    {
                        for (int j = 0; j < piclist.Count; j++)//将插入的pic一一与数据库里面的对比，若存在则不改动，若不存在则插入
                        {
                            if (imgl[i] == piclist[j].Url)//存在则直接跳出这层for语句，检查下一个需要插入的
                            {
                                flag0[j] = 1;//表示没有改变
                                break;
                            }
                            else   //不存在则插入
                            {
                                pic.Title = imgtl[i];
                                pic.Artid = article.Id;
                                pic.Url = imgl[i];
                                pic.Istop = i == 1 ? 1 : 0;
                                picdal.InsertEntity(pic);
                            }
                        }
                    }
                    else//不含图片集则直接插入进去
                    {
                        pic.Title = imgtl[i];
                        pic.Artid = article.Id;
                        pic.Url = imgl[i];
                        pic.Istop = i == 1 ? 1 : 0;
                        picdal.InsertEntity(pic);
                    }
                    //对改变了的做处理 即删除就可以了
                    for (int l = 0; l < flag0.Length; l++)
                    {
                        if (flag0[l] == 0)
                            picdal.DelEntity(piclist[l].ID);
                    }

                }


            }
            else  //如果传递进来的是空 则判断数据库是否含有 如果含有则全部删除
            {
                SiteGroupCms.Dal.ArticlepicDal picdal = new SiteGroupCms.Dal.ArticlepicDal();
                List<SiteGroupCms.Entity.Articlepic> piclist = picdal.getEntityList(article.Id.ToString());
                if (piclist.Count > 0)
                    picdal.DelEntityByartid(article.Id);
            }
            
            //插入附件库    对于删除附件不行  本来有 现在要全部删除 就不起作用了
            if (attslist != "")
            {
                SiteGroupCms.Dal.ArticleattsDal attdal = new SiteGroupCms.Dal.ArticleattsDal();
                SiteGroupCms.Entity.Articleatts att = new SiteGroupCms.Entity.Articleatts();
                List<SiteGroupCms.Entity.Articleatts> attlists = attdal.getEntityList(article.Id.ToString());
                string[] attl = attslist.Split(',');
                string[] atttl = attstitlelist.Split(',');
                int[] flag = new int[attlists.Count]; //用来标记是否有更改
                for (int k = 0; k < flag.Length; k++)
                {
                    flag[k] = 0;
                }
                for (int i = 0; i < attl.Length - 1; i++)
                {
                    if (attlists.Count > 0)
                    {
                        for (int j = 0; j < attlists.Count; j++)
                        {
                            if (attlists[j].Url == attl[i])//如果存在则继续 
                            {
                                flag[j] = 1;//表示没有改变
                                break;
                            }
                            else
                            {
                                att.Artid = article.Id;
                                att.Title = atttl[i];
                                att.Url = attl[i];
                                attdal.InsertEntity(att);
                            }

                        }
                    }
                    else
                    {
                        att.Artid = article.Id;
                        att.Title = atttl[i];
                        att.Url = attl[i];
                        attdal.InsertEntity(att);
                    }
                    //对改变了的做处理 即删除就可以了
                    for (int l = 0; l < flag.Length; l++)
                    {
                        if (flag[l] == 0)
                            attdal.DelEntity(attlists[l].ID);
                    }
                }
            }
            else  //如果传递进来的是空 则判断数据库是否含有 如果含有则全部删除
            {
                SiteGroupCms.Dal.ArticleattsDal attdal = new SiteGroupCms.Dal.ArticleattsDal();
                List<SiteGroupCms.Entity.Articleatts> attlist = attdal.getEntityList(article.Id.ToString());
                if (attlist.Count > 0)
                    attdal.DelEntityByartid(article.Id);
            }
        }
    }
}
