using System;
using System.Collections.Generic;
using System.Text;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
using System.Data;
using System.Web;

namespace SiteGroupCms.Dal
{
    /// <summary>
    /// 操作日志
    /// </summary>
  public  class LogDal :Common
    {
      public LogDal()
      { 
            base.SetupSystemDate();
      }
      /// <summary>
      /// 保存管理日志
      /// </summary>
      /// <param name="_adminid">管理员ID</param>
      /// <param name="_info">保存信息</param>
      public void SaveLog(int _adminid, int _type)
      {
          SiteGroupCms.Entity.Admin currentadmin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.AddFieldItem("douserid", _adminid.ToString());
              _doh.AddFieldItem("dotype", _type);
              _doh.AddFieldItem("doip", IPHelp.ClientIP);
              if(currentadmin!=null)
              _doh.AddFieldItem("siteid", currentadmin.CurrentSite);
              _doh.Insert("yy_loginfo");
          }
      }
      public void SaveLog(int _type)
      {
          SiteGroupCms.Entity.Admin currentadmin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.AddFieldItem("douserid", currentadmin.Id);
              _doh.AddFieldItem("dotype", _type);
              _doh.AddFieldItem("doip", IPHelp.ClientIP);
              _doh.AddFieldItem("siteid", currentadmin.CurrentSite);
              _doh.Insert("yy_loginfo");
          }
      }
      /// <summary>
      /// 得到列表JSON数据
      /// </summary>
      /// <param name="_thispage">当前页码</param>
      /// <param name="_pagesize">每页记录条数</param>
      /// <param name="_joinstr">关联条件</param>
      /// <param name="_wherestr">分页条件(不带A.)</param>
      /// <param name="_jsonstr">返回值</param>
      public void GetListJSON(int _thispage, int _pagesize, string _wherestr,ref string _jsonstr)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = _wherestr;
              string sqlStr = "";
              int _countnum = _doh.Count("yy_loginfo");
              sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_loginfo", "id", _pagesize, _thispage, "desc", _wherestr);
              _doh.Reset();
              _doh.SqlCmd = sqlStr;
              DataTable dt = _doh.GetDataTable();
              DataTable  dt2 = new DataTable();
              DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
              DataColumn col2 = new DataColumn("type", System.Type.GetType("System.String"));
              DataColumn col3 = new DataColumn("douser", System.Type.GetType("System.String"));
              DataColumn col4 = new DataColumn("doip", System.Type.GetType("System.String"));
              DataColumn col5 = new DataColumn("dotime", System.Type.GetType("System.String"));
              dt2.Columns.Add(col);
              dt2.Columns.Add(col2);
              dt2.Columns.Add(col3);
              dt2.Columns.Add(col4);
              dt2.Columns.Add(col5);
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  DataRow dr = dt2.NewRow();
                  dr["id"] = dt.Rows[i]["id"].ToString();
                  dr["type"] =new DoDal().GetEntity(Str2Int(dt.Rows[i]["dotype"].ToString())).Dotype;
                  dr["dotime"] = String.Format("{0:g}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["dotime"].ToString(), DateTime.Now));
                  dr["doip"] = dt.Rows[i]["doip"].ToString();
                  dr["douser"] =new AdminDal().GetEntity(dt.Rows[i]["douserid"].ToString()).UserName;

                  dt2.Rows.Add(dr);
              }
              _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
              dt.Clear();
              dt.Dispose();
              dt2.Clear();
              dt2.Dispose();
          }
      }
      /// <summary>
      /// 清空管理日志
      /// </summary>
      public bool DeleteLogs()
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "1=1";
              return _doh.Delete("yy_loginfo") > 0;
          }
      }
      /// <summary>
      /// 删除日志ids=1,2
      /// </summary>
      public bool DeleteLog(string ids)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "id in ("+ids+")";
              return _doh.Delete("yy_loginfo") > 0;
          }
      }


      public SiteGroupCms.Entity.Log GetEntity(string _id)
      {
          SiteGroupCms.Entity.Log Log = new SiteGroupCms.Entity.Log();
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.SqlCmd = "SELECT * FROM [yy_loginfo] WHERE [id]=" + _id;
              DataTable dt = _doh.GetDataTable();
              if (dt.Rows.Count > 0)
              {
                  Log.Id = Str2Int(_id);
                  Log.Dotime = Validator.StrToDate(dt.Rows[0]["dotime"].ToString(), DateTime.Now);
                  Log.UserId = dt.Rows[0]["douserid"].ToString();
                  Log.Doip = dt.Rows[0]["doip"].ToString();
                  Log.Dotype = dt.Rows[0]["dotype"].ToString();
              }   
          }
          return Log;
      }
    }
}
