using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
   public class ArticleattsDal :Common
    {
       public ArticleattsDal()
       {
           base.SetupSystemDate();
       }
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="object">对象</param>
       public int InsertEntity(SiteGroupCms.Entity.Articleatts obj)
       { 
          using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.AddFieldItem("artid", obj.Artid);
                _doh.AddFieldItem("title", obj.Title);
                _doh.AddFieldItem("url", obj.Url);
                if (obj.Type != null)
                _doh.AddFieldItem("type", obj.Type);
                if (obj.Size != 0)
                _doh.AddFieldItem("size", obj.Size);
              int _insert = _doh.Insert("yy_articleattinfo");
               return _insert;
            }
       }


       public int DelEntity(int attid)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "delete from yy_articleattinfo where id="+attid;
               return _doh.ExecuteSqlNonQuery();
           }

       }


       public int DelEntityByartid(int artid)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "delete from yy_articleattinfo where artid=" + artid;
               return _doh.ExecuteSqlNonQuery();
           }
       }

       /// <summary>
       /// 取出list
       /// </summary>
       /// <param name="object">文章id</param>
       public List<SiteGroupCms.Entity.Articleatts> getEntityList(string id)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_articleattinfo] WHERE [artid]=" + id;
               DataTable dt = _doh.GetDataTable();
               List<SiteGroupCms.Entity.Articleatts> ilist=new List<SiteGroupCms.Entity.Articleatts>();
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   SiteGroupCms.Entity.Articleatts pic = new SiteGroupCms.Entity.Articleatts();
                   pic.Artid = Str2Int(id);
                   pic.ID = Str2Int(dt.Rows[i]["id"].ToString());
                   pic.Url = dt.Rows[i]["url"].ToString();
                   pic.Title = dt.Rows[i]["title"].ToString();
                   ilist.Add(pic);
               }
               return ilist;
           }

       }
    }
}
