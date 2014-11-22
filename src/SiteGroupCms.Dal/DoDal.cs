using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
   public class DoDal
    {
       public SiteGroupCms.Entity.Do GetEntity(int doid)
       {
           SiteGroupCms.Entity.Do dos = new SiteGroupCms.Entity.Do();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_doinfo] WHERE [id]=" + doid;
               DataTable dt = _doh.GetDataTable();
               if (dt.Rows.Count > 0)
               {
                   dos.Id = doid;
                   dos.Dotype = dt.Rows[0]["dotype"].ToString();

               }

           }
           return dos;
       }
    }
}
