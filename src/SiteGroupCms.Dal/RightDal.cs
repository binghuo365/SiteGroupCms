using System;
using System.Collections.Generic;
using System.Text;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
using System.Data;
using System.Web;

namespace SiteGroupCms.Dal
{
   public class RightDal :Common
    {
       public SiteGroupCms.Entity.Right GetEntity(int _rightid)
       {
           SiteGroupCms.Entity.Right right = new SiteGroupCms.Entity.Right();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_rightinfo] WHERE [id]=" + _rightid;
               DataTable dt = _doh.GetDataTable();
               if (dt.Rows.Count > 0)
               {
                   right.Title = dt.Rows[0]["title"].ToString();
                   right.Id = _rightid;
                   right.Sort = Str2Int(dt.Rows[0]["sort"].ToString());
                   right.Description = dt.Rows[0]["description"].ToString();

               }

           }
           return right;
       }
       public DataTable GetDT(string where)
       {
           SiteGroupCms.Entity.Right right = new SiteGroupCms.Entity.Right();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_rightinfo] WHERE " + where;
               DataTable dt = _doh.GetDataTable();
                  return dt;
           }
           
       }

    }
}
