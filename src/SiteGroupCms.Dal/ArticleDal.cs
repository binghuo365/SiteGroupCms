using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
namespace SiteGroupCms.Dal
{
   public class ArticleDal:Common
    {
        public ArticleDal()
        {
            base.SetupSystemDate();
        }
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="_wherestr">条件</param>
        /// <returns></returns>
        public bool Exists(string _wherestr)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                int _ext = 0;
                _doh.Reset();
                _doh.ConditionExpress = _wherestr;
                if (_doh.Exist("yy_articleinfo"))
                    _ext = 1;
                return (_ext == 1);
            }
        }
        public DataTable getDT(string where)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select * from yy_articleinfo where "+where;
                return _doh.GetDataTable();

            }
        }


        public DataTable getdistinctcatalogidDT(string where)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select distinct catalogid from yy_articleinfo where " + where;
                return _doh.GetDataTable();

            }
        }

        public bool ispassed(string id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select * from yy_articleinfo where id= " + id;
                DataTable dt= _doh.GetDataTable();
                if (dt.Rows[0]["ispassed"].ToString() == "1")
                    return true;
                else
                    return false;

            }
        }
        public bool ipublish(string id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select * from yy_articleinfo where id= " + id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows[0]["ispublish"].ToString() == "1")
                    return true;
                else
                    return false;

            }
        }
        public string getcount(string where)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select * from yy_articleinfo where " + where;
                DataTable dt = _doh.GetDataTable();
                if (dt!=null)
                    return dt.Rows.Count.ToString();
                else
                    return "0";

            }
        }
        /// <summary>
        /// 判断重复性(标题是否存在)
        /// </summary>
        /// <param name="_title">需要检索的标题</param>
        /// <param name="_id">除外的ID</param>
        /// <param name="_wherestr">其他条件</param>
        /// <returns></returns>
        public bool ExistTitle(string _title, string _id, string _wherestr)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                int _ext = 0;
                _doh.Reset();
                _doh.ConditionExpress = "title=@title and id<>" + _id;
                if (_wherestr != "") _doh.ConditionExpress += " and " + _wherestr;
                _doh.AddConditionParameter("@title", _title);
                if (_doh.Exist("yy_articleinfo"))
                    _ext = 1;
                return (_ext == 1);
            }
        }
        /// <summary>
        /// 得到列表JSON数据
        /// </summary>
        /// <param name="_thispage">当前页码</param>
        /// <param name="_pagesize">每页记录条数</param>
        /// <param name="_wherestr">搜索条件</param>
        /// <param name="_jsonstr">返回值</param>
        public void GetListJSON(int _thispage, int _pagesize, string _wherestr, ref string _jsonstr,string orderstr)
        {
            SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
            AdminDal _adminobj =new AdminDal();
            CatalogDal catadalobj = new CatalogDal();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = _wherestr;
                string sqlStr = "";
                int _countnum = _doh.Count("yy_articleinfo");
                sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_articleinfo", _pagesize, _thispage, orderstr, _wherestr);
                _doh.Reset();
                _doh.SqlCmd = sqlStr;
                DataTable dt = _doh.GetDataTable();
                DataTable dt2 = new DataTable();
                DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
                DataColumn col2 = new DataColumn("title", System.Type.GetType("System.String"));
                DataColumn col3 = new DataColumn("author", System.Type.GetType("System.String"));
                DataColumn col4 = new DataColumn("addtime", System.Type.GetType("System.String"));
                DataColumn col5 = new DataColumn("catalogid", System.Type.GetType("System.String"));
                DataColumn col6 = new DataColumn("state", System.Type.GetType("System.String"));
                DataColumn col7 = new DataColumn("clickcount", System.Type.GetType("System.String"));
                DataColumn col8 = new DataColumn("articletype", System.Type.GetType("System.String"));
                dt2.Columns.Add(col);
                dt2.Columns.Add(col2);
                dt2.Columns.Add(col3);
                dt2.Columns.Add(col4);
                dt2.Columns.Add(col5);
                dt2.Columns.Add(col6);
                dt2.Columns.Add(col7);
                dt2.Columns.Add(col8);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt2.NewRow();
                    dr["id"] = dt.Rows[i]["id"];
                    if (dt.Rows[i]["yyarticleid"].ToString() == "" || dt.Rows[i]["yyarticleid"].ToString() == "0")
                    {
                        if (dt.Rows[i]["linkurl"] != null && dt.Rows[i]["linkurl"].ToString()!="")//为连接文章
                        dr["articletype"] = "链接";
                        else
                          dr["articletype"] = "普通";

                        dr["title"] = dt.Rows[i]["title"];
                        if (dt.Rows[i]["author"].ToString() == "")
                            dr["author"] = "匿名";
                        else
                            dr["author"] = dt.Rows[i]["author"].ToString();

                        dr["addtime"] = String.Format("{0:d}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["addtime"].ToString(), DateTime.Now));

                        if (dt.Rows[i]["catalogid"].ToString() != "")
                        {
                            if (catadalobj.GetEntity(dt.Rows[i]["catalogid"].ToString()) != null)
                                dr["catalogid"] = catadalobj.GetEntity(dt.Rows[i]["catalogid"].ToString()).Title;
                            else
                                dr["catalogid"] = "栏目不存在了";
                        }
                        else
                            dr["catalogid"] = "无栏目";

                        dr["state"] = "";
                        if (dt.Rows[i]["ispassed"].ToString() == "0")
                            dr["state"] += "<span style='color:red;'>未审核</span>&nbsp;";
                        else
                            dr["state"] += "已审核&nbsp;";
                        if (dt.Rows[i]["isrecommend"].ToString() != "0")
                            dr["state"] += "推荐&nbsp;";
                        if (dt.Rows[i]["isppt"].ToString() != "0")
                            dr["state"] += "幻灯片&nbsp;";
                        if (dt.Rows[i]["isroll"].ToString() != "0")
                            dr["state"] += "滚动&nbsp;";
                        if (dt.Rows[i]["ispublish"].ToString() != "0")
                            dr["state"] += "已发布&nbsp;";
                        else
                            dr["state"] += "<span style='color:red;'>未发布</span>&nbsp;";
                        if (dt.Rows[i]["isshare"].ToString() != "0")
                            dr["state"] += "共享&nbsp;";
                        dr["clickcount"] = dt.Rows[i]["clickcount"].ToString();
                    }
                    else //如果是引用 则查找源文章  
                    {
                        dr["articletype"] = "引用";
                        SiteGroupCms.Entity.Article yuanarticle = new ArticleDal().GetEntity(dt.Rows[i]["yyarticleid"].ToString());
                        if (yuanarticle.Title ==null)//源文章不存在了
                            dr["title"] = "源文章不存在了";
                        else
                            dr["title"] = yuanarticle.Title;

                        if (dt.Rows[i]["author"].ToString() == "")
                            dr["author"] = "匿名";
                        else
                            dr["author"] = dt.Rows[i]["author"].ToString();

                        dr["addtime"] = String.Format("{0:g}", yuanarticle.Addtime);

                        if (yuanarticle.Catalogid.ToString()!= "")
                        {
                            if (catadalobj.GetEntity(yuanarticle.Catalogid.ToString()) != null)
                                dr["catalogid"] = catadalobj.GetEntity(yuanarticle.Catalogid.ToString()).Title;
                            else
                                dr["catalogid"] = "栏目不存在了";
                        }
                        else
                            dr["catalogid"] = "无栏目";

                        dr["state"] = "";
                        if (yuanarticle.Ispass.ToString() == "0")
                            dr["state"] += "未审核&nbsp;";
                        else
                            dr["state"] += "已审核&nbsp;";
                        if (yuanarticle.Isrecommend.ToString()!= "0")
                            dr["state"] += "推荐&nbsp;";
                        if (yuanarticle.Isppt.ToString() != "0")
                            dr["state"] += "幻灯片&nbsp;";
                        if (yuanarticle.Isroll.ToString() != "0")
                            dr["state"] += "滚动&nbsp;";
                        if (yuanarticle.Ispublish.ToString() != "0")
                            dr["state"] += "已发布&nbsp;";
                        else
                            dr["state"] += "未发布&nbsp;";
                        if (yuanarticle.Isshare.ToString() != "0")
                            dr["state"] += "共享&nbsp;";
                        dr["clickcount"] = yuanarticle.Clickcount.ToString();
                    }
                        
    
                    dt2.Rows.Add(dr);

                    
                }
                _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2,_countnum);
                dt.Clear();
                dt.Dispose();
            }
        }
        /// <summary>
        /// 彻底删除一条数据
        /// </summary>
        public bool DeleteByID(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                int _del = _doh.Delete("yy_articleinfo");
                return (_del == 1);
            }
        }
        /// <summary>
        /// 通过审核
        /// </summary>
        public bool PassById(string _id,int type,string passuserid)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                _doh.AddFieldItem("ispassed", type);
                _doh.AddFieldItem("passuserid", passuserid);
                int _update = _doh.Update("yy_articleinfo");
                return (_update == 1);
            }
        }

        /// <summary>
        /// 共享相关
        /// </summary>
        public bool ShareById(string _id, int type)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                _doh.AddFieldItem("isshare", type);
                int _update = _doh.Update("yy_articleinfo");
                return (_update == 1);
            }
        }
        /// <summary>
        /// 更新发布状态
        /// </summary>
        public bool publish(string _id,int tys)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                _doh.AddFieldItem("ispublish", tys);
                int _update = _doh.Update("yy_articleinfo");
                return (_update == 1);
            }
        }
        public bool updatsort(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                _doh.AddFieldItem("sort", _id);
                int _update = _doh.Update("yy_articleinfo");
                return (_update == 1);
            }
        }

        public bool addsort(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                string sql= @"SELECT   TOP 1 sort -  (SELECT     sort
                            FROM          yy_articleinfo
                            WHERE      (id = "+_id+")) AS sorts";
                sql+=@" FROM         yy_articleinfo WHERE     (sort -
                          (SELECT     sort
                            FROM          yy_articleinfo   WHERE      (id = "+_id+")) >= 0)";
                sql += @" ORDER BY sorts asc";
                _doh.SqlCmd = sql;
                int chasort = Convert.ToInt32(_doh.GetDataTable().Rows[0]["sorts"].ToString())+1;
                _doh.Reset();
                _doh.SqlCmd = "update yy_articleinfo set sort=sort+"+chasort+" where id=" + _id;
                return (_doh.ExecuteSqlNonQuery()>=1);
            }
        }

        public bool missort(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                string sql = @"SELECT   TOP 1 sort -  (SELECT     sort
                            FROM          yy_articleinfo
                            WHERE      (id = " + _id + ")) AS sorts";
                sql += @" FROM         yy_articleinfo WHERE     (sort -
                          (SELECT     sort
                            FROM          yy_articleinfo   WHERE      (id = " + _id + ")) <= 0)";
                sql += @" ORDER BY sorts desc";
                _doh.SqlCmd = sql;
                int chasort = Convert.ToInt32(_doh.GetDataTable().Rows[0]["sorts"].ToString())-1 ;
                _doh.Reset();
                _doh.SqlCmd = "update yy_articleinfo set sort=sort+" + chasort + " where id=" + _id;
                return (_doh.ExecuteSqlNonQuery() >= 1);
            }
        }
        public bool topsort(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select max(sort) as sort from yy_articleinfo where catalogid=(select catalogid from yy_articleinfo where id="+_id+")";
                int maxsort = Convert.ToInt32(_doh.GetDataTable().Rows[0]["sort"].ToString());
                int topsort = maxsort + 1; 
                _doh.Reset();
                _doh.SqlCmd = "update yy_articleinfo set sort="+topsort+" where id=" + _id;
                return (_doh.ExecuteSqlNonQuery() >= 1);
            }
        }
        /// <summary>
        /// 删除和回收一条数据
        /// </summary>
        public bool huihouById(string _id,int type)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                _doh.AddFieldItem("isdel", type);
                int _update = _doh.Update("yy_articleinfo");
                return (_update == 1);
            }
        }

        /// <summary>
        /// 获得单页内容的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        public SiteGroupCms.Entity.Article GetEntity(string _id)
        {
            SiteGroupCms.Entity.Article article = new SiteGroupCms.Entity.Article();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_articleinfo] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    article.Id =Str2Int(_id);
                    article.Title = dt.Rows[0]["title"].ToString();
                    article.Subtitle = dt.Rows[0]["subtitle"].ToString();
                    article.Keywords = dt.Rows[0]["keywords"].ToString();
                    article.Abstract = dt.Rows[0]["abstract"].ToString();
                    article.Siteid =Str2Int(dt.Rows[0]["siteid"].ToString());
                    article.Catalogid = Str2Int(dt.Rows[0]["catalogid"].ToString());
                    article.Linkurl = dt.Rows[0]["linkurl"].ToString();
                    article.Author = dt.Rows[0]["author"].ToString();
                    article.Passuserid = Str2Int(dt.Rows[0]["passuserid"].ToString());
                    article.Source = dt.Rows[0]["source"].ToString();
                    article.Addtime = Validator.StrToDate(dt.Rows[0]["addtime"].ToString(), DateTime.Now);
                    article.PassTime = Validator.StrToDate(dt.Rows[0]["passtime"].ToString(), DateTime.Now);
                    article.Publishtime = Validator.StrToDate(dt.Rows[0]["publishtime"].ToString(), DateTime.Now);
                    article.Ispass = Str2Int(dt.Rows[0]["ispassed"].ToString());
                    article.IsDel = Str2Int(dt.Rows[0]["isdel"].ToString());
                    article.Isrecommend = Str2Int(dt.Rows[0]["isrecommend"].ToString());
                    article.Isppt = Str2Int(dt.Rows[0]["isppt"].ToString());
                    article.Isroll = Str2Int(dt.Rows[0]["isroll"].ToString());
                    article.Ispublish = Str2Int(dt.Rows[0]["ispublish"].ToString());
                    article.Isshare = Str2Int(dt.Rows[0]["isshare"].ToString());
                    article.Templateid=Str2Int(dt.Rows[0]["templates"].ToString());
                    article.Color = dt.Rows[0]["color"].ToString();
                    article.Content = dt.Rows[0]["content"].ToString();
                    article.Clickcount =Str2Int(dt.Rows[0]["clickcount"].ToString());
                    article.Yyarticleid = Str2Int(dt.Rows[0]["yyarticleid"].ToString());
                    
                   

                }
                return article;
            }
        }
        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="object">对象</param>
        public bool UpdateArticle (SiteGroupCms.Entity.Article obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", obj.Id);
                _doh.AddFieldItem("title",obj.Title);
                 _doh.AddFieldItem("subtitle",obj.Subtitle);
                 _doh.AddFieldItem("keywords",obj.Keywords);
                 _doh.AddFieldItem("abstract",obj.Abstract);
                 _doh.AddFieldItem("siteid",obj.Siteid);
                 _doh.AddFieldItem("catalogid",obj.Catalogid);
                 _doh.AddFieldItem("linkurl",obj.Linkurl);
                // _doh.AddFieldItem("author",obj.Author);
                 _doh.AddFieldItem("passuserid",obj.Passuserid);
                 _doh.AddFieldItem("source",obj.Source);
                 if (obj.Addtime.ToString() != "0001/1/1 0:00:00")
                    _doh.AddFieldItem("addtime", obj.Addtime.ToString());
               //  if (obj.PassTime.ToString() != "0001/1/1 0:00:00")
                //     _doh.AddFieldItem("passtime", obj.PassTime.ToString());
                // if (obj.Publishtime.ToString() != "0001/1/1 0:00:00")
                //     _doh.AddFieldItem("publishtime", obj.Publishtime.ToString());
                 _doh.AddFieldItem("ispassed",obj.Ispass);
                 _doh.AddFieldItem("isdel",obj.IsDel);
                 _doh.AddFieldItem("isrecommend",obj.Isrecommend);
                 _doh.AddFieldItem("isppt",obj.Isppt);
                 _doh.AddFieldItem("isroll",obj.Isroll);
                 _doh.AddFieldItem("ispublish",obj.Ispublish);
                 _doh.AddFieldItem("isshare",obj.Isshare);
                 _doh.AddFieldItem("templates",obj.Templateid);
                _doh.AddFieldItem("color",obj.Color);
                _doh.AddFieldItem("content",obj.Content);
                _doh.AddFieldItem("clickcount", obj.Clickcount);
                _doh.AddFieldItem("yyarticleid", obj.Yyarticleid);


                int _update = _doh.Update("yy_articleinfo");
                return (_update == 1);
            }
        }
        /// <summary>
        /// 插入文章
        /// </summary>
        /// <param name="object">对象</param>
        public int insertEntity(SiteGroupCms.Entity.Article obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.AddFieldItem("title", obj.Title);
                _doh.AddFieldItem("subtitle", obj.Subtitle);
                _doh.AddFieldItem("keywords", obj.Keywords);
                _doh.AddFieldItem("abstract", obj.Abstract);
                _doh.AddFieldItem("siteid", obj.Siteid);
                _doh.AddFieldItem("catalogid", obj.Catalogid);
                _doh.AddFieldItem("linkurl", obj.Linkurl);
                _doh.AddFieldItem("author", obj.Author);
                _doh.AddFieldItem("passuserid", obj.Passuserid);
                _doh.AddFieldItem("source", obj.Source);
               if (obj.Addtime.ToString()!= "0001/1/1 0:00:00")
                _doh.AddFieldItem("addtime",obj.Addtime.ToString());
               // if (obj.PassTime.ToString() != "0001/1/1 0:00:00")
              //  _doh.AddFieldItem("passtime", obj.PassTime.ToString());
              //  if (obj.Publishtime.ToString() != "0001/1/1 0:00:00")
              //  _doh.AddFieldItem("publishtime", obj.Publishtime.ToString());
                _doh.AddFieldItem("ispassed", obj.Ispass);
                _doh.AddFieldItem("isdel", obj.IsDel);
                _doh.AddFieldItem("isrecommend", obj.
                    
                    Isrecommend);
                _doh.AddFieldItem("isppt", obj.Isppt);
                _doh.AddFieldItem("isroll", obj.Isroll);
                _doh.AddFieldItem("ispublish", obj.Ispublish);
                _doh.AddFieldItem("isshare", obj.Isshare);
                _doh.AddFieldItem("templates", obj.Templateid);
                _doh.AddFieldItem("color",obj.Color);
                _doh.AddFieldItem("content", obj.Content);
                _doh.AddFieldItem("clickcount", obj.Clickcount);
                int _insert = _doh.Insert("yy_articleinfo");
                return _insert;
            }
        }
        public int movetoCatalog(string ids, string catalogid)//移动到新的栏目必须要重新发布 ，所以发布状态为未发布
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id in (" + ids + ")";
                _doh.AddFieldItem("catalogid", catalogid);
                _doh.AddFieldItem("ispublish", 0);
                return _doh.Update("yy_articleinfo") ;
            }
        
        }
         public int copytocatalog(string ids, string catalogid)  //移动到新的栏目必须要重新发布 ，所以发布状态为未发布
         {
             using (DbOperHandler _doh = new Common().Doh())
             {
                 _doh.Reset();
                 _doh.SqlCmd = "INSERT INTO yy_articleinfo (title, color, [content], subtitle, keywords, abstract, siteid, catalogid, linkurl, author, passuserid, Source, addtime, passtime, publishtime, ispassed, isdel, isrecommend,"; 
                   _doh.SqlCmd+=" isppt, isroll, isshare, templates, sort) SELECT title, color, [content], subtitle, keywords, abstract, siteid, "+catalogid+" as catalogid, linkurl, author, passuserid, Source, addtime, passtime, publishtime, ispassed, isdel, isrecommend,"; 
                   _doh.SqlCmd+="  isppt, isroll, isshare, templates, sort FROM   yy_articleinfo AS yy_articleinfo_1 WHERE     id in (" +ids+")";

                 return _doh.ExecuteSqlNonQuery() ;
             }

         }
         public int yiyongtocatalog(string ids, string catalogid)  //引用到新栏目  设定引用的articleid 为这个id 然后新建一个文章
         {
             using (DbOperHandler _doh = new Common().Doh())
             {
                 _doh.Reset();
                 _doh.SqlCmd = "INSERT INTO yy_articleinfo (title, siteid, catalogid, linkurl, author, passuserid, Source, addtime, passtime, publishtime, ispassed, isdel, isrecommend,";
                 _doh.SqlCmd += " isppt, isroll, isshare, templates, sort,yyarticleid) SELECT title,siteid,  " + catalogid + " as catalogid, linkurl, author, passuserid, Source, addtime, passtime, publishtime, 1 as ispassed, isdel, isrecommend,";
                 _doh.SqlCmd += "  isppt, isroll, isshare, templates, sort,id FROM   yy_articleinfo AS yy_articleinfo_1 WHERE     id in (" + ids + ")";

                 return _doh.ExecuteSqlNonQuery();
             }

         }
         public int gethitcount(int articleid)//先加1 在取出
         {
             using (DbOperHandler _doh = new Common().Doh())
             {
                 _doh.Reset();
                 string sql = "update yy_articleinfo set clickcount=clickcount+1 where id="+articleid;
                 _doh.SqlCmd = sql;
                 _doh.ExecuteSqlNonQuery();
                 _doh.Reset();
                 string sql2 = "select clickcount from yy_articleinfo where id="+articleid;
                 _doh.SqlCmd = sql2;
                 return Str2Int(_doh.GetDataTable().Rows[0]["clickcount"].ToString());
             }
         }
    }
}
