using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
namespace SiteGroupCms.Dal
{
   public class RepostDal
    {
       public List<SiteGroupCms.Entity.Repost> GetList(int followid)
       {
           List<SiteGroupCms.Entity.Repost> ilist = new List<SiteGroupCms.Entity.Repost>();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_repostinfo] WHERE [followid]=" + followid;
               DataTable dt = _doh.GetDataTable();
               if(dt.Rows.Count==0) return null;
               for (int i = 0; i < dt.Rows.Count; i++)
			{
			   SiteGroupCms.Entity.Repost pic = new SiteGroupCms.Entity.Repost();
               pic.Id =Convert.ToInt32(dt.Rows[i]["id"].ToString());
               pic.Type = Convert.ToInt32(dt.Rows[i]["type"].ToString());
               pic.Content = dt.Rows[i]["content"].ToString();
               pic.Followid = followid;
               pic.Addtime = Validator.StrToDate(dt.Rows[i]["addtime"].ToString(), DateTime.Now);
               ilist.Add(pic);
			}
           }
           return ilist;
       }
       public int Updateentity(SiteGroupCms.Entity.Repost obj)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.ConditionExpress = "followid=@id";
               _doh.AddConditionParameter("@id", obj.Followid);
               _doh.AddFieldItem("content", obj.Content);
               if (obj.Addtime.ToString() != "0001/1/1 0:00:00")
                   _doh.AddFieldItem("addtime", obj.Addtime.ToString());
               return _doh.Update("yy_repostinfo");
           }
       }
       public int Insertentity(SiteGroupCms.Entity.Repost obj)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.AddFieldItem("followid", obj.Followid);
               _doh.AddFieldItem("content", obj.Content);
               if (obj.Addtime.ToString() != "0001/1/1 0:00:00")
                   _doh.AddFieldItem("addtime", obj.Addtime.ToString());
               return _doh.Insert("yy_repostinfo");
           }
       }
       public int deletebyfollowid(SiteGroupCms.Entity.Repost obj)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "delete from yy_repostinfo where followid=" + obj.Followid;
               return _doh.ExecuteSqlNonQuery();
           }
       }
    }
}
