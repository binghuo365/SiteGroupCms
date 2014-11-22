using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
   public class ArticlepicDal :Common
    {
       public ArticlepicDal()
       {
           base.SetupSystemDate();
       }
       /// <summary>
       /// 插入图附件
       /// </summary>
       /// <param name="object">对象</param>
       public int InsertEntity(SiteGroupCms.Entity.Articlepic obj)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.AddFieldItem("artid", obj.Artid);
               _doh.AddFieldItem("title", obj.Title);
               _doh.AddFieldItem("url", obj.Url);
               if(obj.Type!=null)
               _doh.AddFieldItem("type", obj.Type);
               if (obj.Size != 0)
               _doh.AddFieldItem("size", obj.Size);
               _doh.AddFieldItem("istop",obj.Istop);
               int _insert = _doh.Insert("yy_articlepicinfo");
               return _insert;
           }
       }
        /// <summary>
        /// 取出list
        /// </summary>
        /// <param name="object">文章id</param>
       public List<SiteGroupCms.Entity.Articlepic>  getEntityList(string id)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_articlepicinfo] WHERE [artid]=" + id;
                DataTable dt = _doh.GetDataTable();
               List<SiteGroupCms.Entity.Articlepic> ilist=new List<SiteGroupCms.Entity.Articlepic>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SiteGroupCms.Entity.Articlepic pic = new SiteGroupCms.Entity.Articlepic();
                    pic.Artid =Str2Int(id);
                    pic.ID = Str2Int(dt.Rows[i]["id"].ToString());
                    pic.Url = dt.Rows[i]["url"].ToString();
                    pic.Title = dt.Rows[i]["title"].ToString();
                    ilist.Add(pic);
                }
                return ilist;
           }
       
       }

       public int DelEntity(int imgid)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "delete from yy_articlepicinfo where id=" + imgid;
               return _doh.ExecuteSqlNonQuery();
           }
       }

       public int DelEntityByartid(int artid)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "delete from yy_articlepicinfo where artid=" + artid;
               return _doh.ExecuteSqlNonQuery();
           }
       }
    }
}
