using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
    public class Module_articleDAL : Common, IModule
    {
        public Module_articleDAL()
        {
            base.SetupSystemDate();
        }
        public virtual void CreateContent(string _ContentId)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT [Content],[linkurl],[yyarticleid] FROM [yy_articleinfo] WHERE  [Id]=" + _ContentId;
                DataTable dtContent = _doh.GetDataTable();
                string ArticleContent = dtContent.Rows[0]["Content"].ToString();
                string linkurl = dtContent.Rows[0]["linkurl"].ToString();
                string yyarticleid = dtContent.Rows[0]["yyarticleid"].ToString();
                dtContent.Clear();
                dtContent.Dispose();
                if (yyarticleid=="0"||yyarticleid=="")   //如果不是引用文章
                {
                  if(linkurl=="")  //且不是连接文章
                      SiteGroupCms.Utils.DirFile.SaveFile(GetContent(_ContentId), Go2View(_ContentId));
                }
             
//同时上传到服务器上 出现问题时 不进行处理 程序照常进行
//上传服务器暂时不使用上传ftp功能
     /*
                try
             {
                 FtpDal ftpdal = new FtpDal();
                 ftpdal.UploadFile(Go2View(_ContentId, true));
             }
             catch (Exception e )
             {
                 throw;
             }*/
//更新数据库的内容为已经发布
             SiteGroupCms.Dal.ArticleDal articledal = new ArticleDal();
             articledal.publish(_ContentId,1);
            }
        }
        public virtual string GetContent(string _ContentId)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_articleinfo] WHERE  [Id]=" + _ContentId;
                DataTable dtSearch = _doh.GetDataTable();
                if (dtSearch.Rows.Count == 0)
                {
                    dtSearch.Clear();
                    dtSearch.Dispose();
                    return "内容错误";
                }
                SiteGroupCms.Dal.ArticleDal articledal = new ArticleDal();
                SiteGroupCms.Entity.Article article = articledal.GetEntity(_ContentId);
                SiteGroupCms.Entity.Site site=(SiteGroupCms.Entity.Site)HttpContext.Current.Session["site"];
                TemplateEngineDAL te = new TemplateEngineDAL(article.Catalogid.ToString());
                string PageStr = string.Empty;
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_articleinfo] WHERE  [Id]=" + _ContentId;
                DataTable dtContent = _doh.GetDataTable();
                string ChannelId = dtContent.Rows[0]["catalogid"].ToString();
                te.PageNav = Go2Site() + " &raquo;" + Go2Channel(ChannelId,0);
                te.PageTitle = site.WebTitle + "--" + dtContent.Rows[0]["title"].ToString();
                te.PageKeywords = dtContent.Rows[0]["keywords"].ToString();
                te.PageDescription = dtContent.Rows[0]["abstract"].ToString();
                p__GetChannel_Article(te, dtContent, ref PageStr);//提取出模板的内容
                te.ReplacePublicTag(ref PageStr);//包含include解析 siteconfig解析 已经注释掉了  replaceTag_ChannelLoop解析
               //te.ReplaceChannelTag(ref PageStr, ChannelId); 
               // PageStr = PageStr.Replace("{$channelid}",ChannelId);
                te.replaceTag_ChannelLoop(ref PageStr, ChannelId);//替换频道循环标签
                te.ReplaceContentTag(ref PageStr, _ContentId);//解释单片文档的所有标签
                te.ReplaceContentLoopTag(ref PageStr);//
                te.ReplaceChannelTag(ref PageStr, ChannelId);//替换
                te.ExcuteLastHTML(ref PageStr);//替换注释标签
                PageStr = SiteGroupCms.Utils.Strings.UBB2HTML(PageStr);//替换ubb标签
                return PageStr.Replace("{$Content}", dtContent.Rows[0]["content"].ToString());
                   // .Replace("{$_getPageBar()}", getPageBar(1, "html", 7, ContentList.Count - 1, 1, _CurrentPage, Go2View(true, _ChannelId, _ContentId, false), Go2View(true, _ChannelId, _ContentId, false), Go2View((true), _ChannelId, _ContentId, false), 0));
            }
        }
        /// <summary>
        /// 得到内容页地址
        /// </summary>
        /// <param name="_contentid"></param>
        /// <param name="_truefile"></param>
        /// <returns></returns>
        public string GetContentLink(string _contentid)
        {
            SiteGroupCms.Dal.Normal_ChannelDAL _ChannelDal=new Normal_ChannelDAL();
            SiteGroupCms.Dal.ArticleDal articledal = new ArticleDal();
            SiteGroupCms.Entity.Article article = articledal.GetEntity(_contentid);
            if (article.Yyarticleid.ToString() != "" && article.Yyarticleid != 0)//如果引用文章
                _contentid = article.Yyarticleid.ToString();
            article = articledal.GetEntity(_contentid); //重新取引用的文章

            if (article.Linkurl != "" && article.Linkurl != null)//如果是连接文章
                // return article.Linkurl; //这个直接得到连接地址 
                return "/ajaxhandler/Gethits.aspx?articleid=" + article.Id; //得到可以统计点击次数的连接地址
            SiteGroupCms.Entity.Normal_Channel _Channel = _ChannelDal.GetEntity(article.Catalogid.ToString());
            site=(SiteGroupCms.Entity.Site)HttpContext.Current.Session["site"];
            string qianstr="/sites/"+site.Location+"/pub";
            string centerstr = "";
            //检查对应的文件夹是否存在，若不存在则创建，若存在则以id为标题生成
            while (_Channel.Father!=0)//不为跟节点就继续往上走
            {
                centerstr = "/"+_Channel.Dirname+centerstr;
                _Channel = _ChannelDal.GetEntity(_Channel.Father.ToString());
            }
            centerstr = "/" + _Channel.Dirname + centerstr+"/";
            string title = article.Addtime.Year.ToString() + article.Addtime.Month.ToString() + article.Addtime.Day.ToString() + _contentid;
            return qianstr + centerstr + title+ ".html";
        }
        /// <summary>
        /// 删除内容页
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <param name="_ContentId"></param>
        public void DeleteContent(string _ContentId)
        {

            SiteGroupCms.Dal.ArticleDal articledal = new ArticleDal();
            SiteGroupCms.Entity.Article article = articledal.GetEntity(_ContentId);
            SiteGroupCms.Entity.Normal_Channel _Channel = new SiteGroupCms.Dal.Normal_ChannelDAL().GetEntity(article.Catalogid.ToString());

            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "[Id]=" + _ContentId;
                object[] _value = _doh.GetFields("yy_articleinfo", "AddDate,FirstPage");
                string _date = _value[0].ToString();
                string _firstpage = _value[1].ToString();
                if (_firstpage.Length > 0)
                {
                    string _folderName = String.Format("/detail_{0}_{1}/{2}",
                        DateTime.Parse(_date).ToString("yyyy"),
                        DateTime.Parse(_date).ToString("MM"),
                        DateTime.Parse(_date).ToString("dd")
                        );
                    if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(site.Location + _Channel.Dirname + _folderName)))
                    {
                        string htmFile = HttpContext.Current.Server.MapPath(Go2View( _ContentId));
                        if (System.IO.File.Exists(htmFile))
                            System.IO.File.Delete(htmFile);
                        string[] htmFiles = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(site.Location + _Channel.Dirname + _folderName), _ContentId + "_*" +"htm");
                        foreach (string fileName in htmFiles)
                        {
                            if (System.IO.File.Exists(fileName))
                                System.IO.File.Delete(fileName);
                        }
                    }
                    _doh.Reset();
                    _doh.SqlCmd = "UPDATE [jcms_module_" + _Channel.Type + "] SET [FirstPage]='' WHERE [ChannelId]=" + article.Catalogid + " AND [Id]=" + _ContentId;
                    _doh.ExecuteSqlNonQuery();
                }
            }
        }
        private void p__GetChannel_Article(TemplateEngineDAL te, DataTable dt, ref string PageStr)
        {
            string TempId, ClassName;
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=" + dt.Rows[0]["catalogid"].ToString();
                TempId = _doh.GetField("yy_cataloginfo", "contenttemplate").ToString();
                ClassName = _doh.GetField("yy_cataloginfo", "Title").ToString();
            }
            string pId = string.Empty;
            //得到模板方案组ID/主题ID/模板内容
            
            new SiteGroupCms.Dal.Normal_TemplateDAL().GetTemplateContent(TempId, ref PageStr);
           
            //te.ReplaceContentLoopTag(ref PageStr);//先不要解析
        }
        private void p__replaceSingleArticle(DataTable dt, ref string PageStr, ref System.Collections.ArrayList ContentList)
        {
            string ArticleContent = dt.Rows[0]["Content"].ToString();
            //处理UBB
            ArticleContent = SiteGroupCms.Utils.Strings.UBB2HTML(ArticleContent);
            ContentList.Add(ArticleContent);
            //处理文章内容分页
           /* if (ArticleContent.Contains("[Jumbot_PageBreak]"))
            {
                string[] ContentArr = ArticleContent.Split(new string[] { "[Jumbot_PageBreak]" }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < ContentArr.Length; j++)
                    ContentList.Add(ContentArr[j]);
            }
            else
                ContentList.Add(ArticleContent);
            if (_CurrentPage < 1 || _CurrentPage > (ContentList.Count))
                _CurrentPage = 1;
            * */
        }

        public List<SiteGroupCms.Entity.Module_Article> DT2List(DataTable _dt)
        {
            if (_dt == null) return null;
            //对其中一些数据进行替换 如点击量 作者  图片 附件等
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                if (_dt.Rows[i]["yyarticleid"].ToString() != "" && _dt.Rows[i]["yyarticleid"].ToString() != "0")//是引用
                {
                    DataRow dttemp = new ArticleDal().getDT("id=" + _dt.Rows[i]["yyarticleid"].ToString()).Rows[0];
                    for (int j = 0; j < _dt.Columns.Count; j++)
                    {
                        _dt.Rows[i][j] = dttemp[j];
                    }    
                }

            }
            return this.DT2List<SiteGroupCms.Entity.Module_Article>(_dt,true,true);
        }

        /// <summary>
        /// 将DataTable转换为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public  List<T> DT2List<T>(DataTable dt, bool is2imgs,bool is2atts)//is2imgs 表示在转换list时是否附加img
        {
            if (dt == null)
                return null;
            List<T> result = new List<T>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                System.Reflection.PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.ToLower().Equals(dt.Columns[i].ColumnName.ToLower()))
                        {
                            if (dt.Rows[j][i] != DBNull.Value)
                            {
                                if (pi.PropertyType.ToString() == "System.Int32")
                                {
                                    pi.SetValue(_t, Int32.Parse(dt.Rows[j][i].ToString()), null);
                                }
                                else if (pi.PropertyType.ToString() == "System.DateTime")
                                {
                                    pi.SetValue(_t, Convert.ToDateTime(dt.Rows[j][i].ToString()), null);
                                }
                                else if (pi.PropertyType.ToString() == "System.Boolean")
                                {
                                    pi.SetValue(_t, Convert.ToBoolean(dt.Rows[j][i].ToString()), null);
                                }
                                else if (pi.PropertyType.ToString() == "System.Single")
                                {
                                    pi.SetValue(_t, Convert.ToSingle(dt.Rows[j][i].ToString()), null);
                                }
                                else if (pi.PropertyType.ToString() == "System.Double")
                                {
                                    pi.SetValue(_t, Convert.ToDouble(dt.Rows[j][i].ToString()), null);
                                }
                                else
                                {
                                    pi.SetValue(_t, dt.Rows[j][i].ToString(), null);
                                }
                            }
                            else
                                pi.SetValue(_t, "", null);//为空，但不为Null
                            break;
                        }
                    }// end for

                    if (is2imgs)  //将图片集附加上去
                    {
                        SiteGroupCms.Dal.ArticlepicDal picdal = new ArticlepicDal();
                        
                        string artid = dt.Rows[j]["id"].ToString();
                        if (pi.Name.ToLower().Equals("img"))
                        {
                          List <SiteGroupCms.Entity.Articlepic> pic = picdal.getEntityList(artid);
                           if(pic.Count>0)
                           pi.SetValue(_t, pic[0].Url, null);
                            else
                           pi.SetValue(_t, "/lib/images/nopic.jpg", null);
                        }
                    }
                    if (is2atts)//将附件附加上去
                    {
                        SiteGroupCms.Dal.ArticleattsDal attdal = new ArticleattsDal();

                        string artid = dt.Rows[j]["id"].ToString();
                        if (pi.Name.ToLower().Equals("atts"))
                        {
                            List<SiteGroupCms.Entity.Articleatts> atts = attdal.getEntityList(artid);
                            if (atts.Count > 0)
                                pi.SetValue(_t, atts[0].Url, null);
                            else
                                pi.SetValue(_t, "#", null);
                        }
                    }
                } //end foreache
                result.Add(_t);
            }

            return result;
        }

    }
}