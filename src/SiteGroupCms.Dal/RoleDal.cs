using System;
using System.Collections.Generic;
using System.Text;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
using System.Data;
using System.Web;
namespace SiteGroupCms.Dal
{
   public class RoleDal:Common
    {
       public SiteGroupCms.Entity.Role GetEntity(int _roleid)
       {
           SiteGroupCms.Entity.Role role = new SiteGroupCms.Entity.Role();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_roleinfo] WHERE [id]=" + _roleid;
               DataTable dt = _doh.GetDataTable();
               SiteGroupCms.Dal.RightDal rightdal = new RightDal();
               SiteGroupCms.Entity.Right right = new SiteGroupCms.Entity.Right();
               if (dt.Rows.Count > 0)
               {
                   role.Title = dt.Rows[0]["role"].ToString();
                   role.Id = _roleid;
                   role.Rights = dt.Rows[0]["rights"].ToString();
                   role.Sort =Str2Int(dt.Rows[0]["sort"].ToString());
                   role.Description = dt.Rows[0]["description"].ToString();
                   string[] strs = role.Rights.Split(',');
                   for (int i = 0; i < strs.Length-1; i++)
                   {
                       role.Righttitle += rightdal.GetEntity(Str2Int(strs[i])).Title + ";";
                   }
               }


           }
           return role;
       }
       public DataTable GetDT(string where)
       { 

          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.SqlCmd = "SELECT * FROM [yy_roleinfo] WHERE " + where;
              DataTable dt = _doh.GetDataTable();
              if (dt.Rows.Count > 0)
                  return dt;
              else
                  return null;
          }

       }


     public void GetListJSON(int _thispage, int _pagesize, string _wherestr, ref string _jsonstr, string ordercol, string ordertype)
       {
           SiteGroupCms.Dal.RightDal rightdal = new RightDal();
           SiteGroupCms.Entity.Right right = new SiteGroupCms.Entity.Right();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.ConditionExpress = _wherestr;
               string sqlStr = "";
               int _countnum = _doh.Count("yy_roleinfo");
               sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_roleinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr);
               _doh.Reset();
               _doh.SqlCmd = sqlStr;
               DataTable dt = _doh.GetDataTable();
               DataTable dt2 = new DataTable();
               DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
               DataColumn col2 = new DataColumn("title", System.Type.GetType("System.String"));
               DataColumn col3 = new DataColumn("rights", System.Type.GetType("System.String"));
               DataColumn col4 = new DataColumn("description", System.Type.GetType("System.String"));
               dt2.Columns.Add(col);
               dt2.Columns.Add(col2);
               dt2.Columns.Add(col3);
               dt2.Columns.Add(col4);
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   DataRow dr = dt2.NewRow();
                   dr["id"] = dt.Rows[i]["id"];
                   dr["title"] = dt.Rows[i]["role"];
                   dr["rights"] = Getrighttitle(Str2Int(dt.Rows[i]["id"].ToString()));
                   dr["description"] = dt.Rows[i]["description"].ToString();
                   dt2.Rows.Add(dr);
               }
               _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
               dt.Clear();
               dt.Dispose();
           }
       }
     public string Getrighttitle(int _roleid)
     {
         string str = "";
         string returns = "";
         RightDal rightdal=new RightDal();
         using (DbOperHandler _doh = new Common().Doh())
         {

             _doh.Reset();
             _doh.SqlCmd = "SELECT * FROM [yy_roleinfo] WHERE [id]=" + _roleid;
             DataTable dt = _doh.GetDataTable();
             if (dt.Rows.Count > 0)
             {

                 str = dt.Rows[0]["rights"].ToString();
                 string[] strs = str.Split(',');
                 for (int i = 0; i < strs.Length-1; i++)
                 {
                     returns += rightdal.GetEntity(Str2Int(strs[i])).Title+"  ,";
                 }

             }

         }
         return returns;
     }
     public int addrole(SiteGroupCms.Entity.Role obj)
     {
         using (DbOperHandler _doh = new Common().Doh())
         {
             _doh.Reset();
             _doh.AddFieldItem("role", obj.Title);
             _doh.AddFieldItem("rights", obj.Rights);
             _doh.AddFieldItem("description", obj.Description);
             int _insert = _doh.Insert("yy_roleinfo");
             return _insert;
         }
     }
     public bool updaterole(SiteGroupCms.Entity.Role obj)
     {
         using (DbOperHandler _doh = new Common().Doh())
         {
             _doh.Reset();
             _doh.ConditionExpress = "id=@id";
             _doh.AddConditionParameter("@id", obj.Id);
             _doh.AddFieldItem("role", obj.Title);
             _doh.AddFieldItem("rights", obj.Rights);
             _doh.AddFieldItem("description", obj.Description);
             int _update = _doh.Update("yy_roleinfo");
             return _update>0;
         }
     }
     public int deleterole(string id)//返回1.成功，2，用户表有用户使用该角色，0，其他原因删除失败
     {
         using (DbOperHandler _doh = new Common().Doh())
         {
             _doh.Reset();
             _doh.SqlCmd = "select * from [yy_userinfo] where [roleid]=" + id;
            DataTable dt = _doh.GetDataTable();
             if (dt.Rows.Count > 0)
                 return 2;
             _doh.ConditionExpress = "id=@id";
             _doh.AddConditionParameter("@id", id);
             int _del = _doh.Delete("yy_roleinfo");
             return _del;
         }    
     }
   }
}
