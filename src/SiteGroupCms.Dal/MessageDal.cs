using System;
using System.Collections.Generic;
using System.Text;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
using System.Data;
using System.Web;

namespace SiteGroupCms.Dal
{
   public class MessageDal :Common
    {
      public MessageDal()
       {
           base.SetupSystemDate();
       }
      public SiteGroupCms.Entity.Message GetEntity(string id)
      {
          SiteGroupCms.Entity.Message Message = new SiteGroupCms.Entity.Message();
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.SqlCmd = "SELECT * FROM [yy_noticeinfo] WHERE [id]=" + id;
              DataTable dt = _doh.GetDataTable();
              if (dt.Rows.Count > 0)
              {
                  Message.Id = Str2Int(id);
                  Message.Title = dt.Rows[0]["title"].ToString();
                  Message.Senduserid =Str2Int( dt.Rows[0]["senduserid"].ToString());
                  Message.Content = dt.Rows[0]["content"].ToString();
                  Message.Type =Str2Int(dt.Rows[0]["type"].ToString());
                  Message.DeptId = Str2Int(dt.Rows[0]["deptid"].ToString());
                  Message.Userid = Str2Int(dt.Rows[0]["userid"].ToString());
                  Message.Isread = Str2Int(dt.Rows[0]["isread"].ToString());
                  Message.Sort = Str2Int(dt.Rows[0]["sort"].ToString());
                  Message.Sendtime = Validator.StrToDate(dt.Rows[0]["sendtime"].ToString(), DateTime.Now);

              }
          }
             
          return Message;
      }

       //发布消息
      public int addmessage(string title,string content)
      { 
      //向这个站点的所有用户发一条消息
          SiteGroupCms.Entity.Admin _admin=(SiteGroupCms.Entity.Admin)System.Web.HttpContext.Current.Session["admin"];
          DataTable dt = new SiteGroupCms.Dal.AdminDal().GetDT("1=1");
          for (int i = 0; i < dt.Rows.Count; i++)
          {
              addonemessage(_admin.Id.ToString(), dt.Rows[i]["id"].ToString(), title, content);
          }

          return dt.Rows.Count;

      }
      public int addonemessage(string senderid,string receverid,string title,string content)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.AddFieldItem("title", title);
              _doh.AddFieldItem("senduserid", senderid);
              _doh.AddFieldItem("content", content);
              _doh.AddFieldItem("userid", receverid);
             return _doh.Insert("yy_noticeinfo");
          }
      }
       //更新消息为已经读
      public bool updateread(string id,int type)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "id=@id";
              _doh.AddConditionParameter("@id", id);
              _doh.AddFieldItem("isread",type);
              int _update = _doh.Update("yy_noticeinfo");
              return (_update == 1);
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
              int _del = _doh.Delete("yy_noticeinfo");
              return (_del == 1);
          }
      }
      /// <summary>
      /// 彻底删除一些数据
      /// </summary>
      public bool DeleteByIds(string _ids)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "id in (" + _ids + ")";
              int _del = _doh.Delete("yy_noticeinfo");
              return (_del >= 1);
          }
      }
      public bool updatereadbyids(string ids)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "id in ("+ids+")";
              _doh.AddFieldItem("isread", 1);
              int _update = _doh.Update("yy_noticeinfo");
              return (_update >= 1);
          }
      }
      /// <summary>
      /// 得到列表JSON数据
      /// </summary>
      /// <param name="_thispage">当前页码</param>
      /// <param name="_pagesize">每页记录条数</param>
      /// <param name="_wherestr">搜索条件</param>
      /// <param name="_jsonstr">返回值</param>
      public void GetListJSON(int _thispage, int _pagesize, string _wherestr, ref string _jsonstr, string ordercol, string ordertype)
      {
          SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
          AdminDal _adminobj = new AdminDal();
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = _wherestr;
              string sqlStr = "";
              int _countnum = _doh.Count("yy_noticeinfo");
              sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_noticeinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr);
              _doh.Reset();
              _doh.SqlCmd = sqlStr;
              DataTable dt = _doh.GetDataTable();
              DataTable dt2 = new DataTable();
              DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
              DataColumn col2 = new DataColumn("title", System.Type.GetType("System.String"));
              DataColumn col3 = new DataColumn("senduser", System.Type.GetType("System.String"));
              DataColumn col4 = new DataColumn("sendtime", System.Type.GetType("System.String"));
              DataColumn col5 = new DataColumn("isread",System.Type.GetType("System.String"));
              dt2.Columns.Add(col);
              dt2.Columns.Add(col2);
              dt2.Columns.Add(col3);
              dt2.Columns.Add(col4);
              dt2.Columns.Add(col5);
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  DataRow dr = dt2.NewRow();
                  dr["id"] = dt.Rows[i]["id"];
                  if (dt.Rows[i]["isread"].ToString() == "0")
                      dr["title"] = "<span style=color:red>" + dt.Rows[i]["title"].ToString() + "</span>";
                  else
                      dr["title"] = dt.Rows[i]["title"].ToString();
                  if (dt.Rows[i]["senduserid"].ToString() != "" && dt.Rows[i]["senduserid"].ToString() != "0")
                  {
                      if (_adminobj.GetEntity(dt.Rows[i]["senduserid"].ToString()) != null)
                          dr["senduser"] = _adminobj.GetEntity(dt.Rows[i]["senduserid"].ToString()).UserName;
                      else
                          dr["senduser"] = "查无此人";
                  }
                  else
                      dr["senduser"] = "佚名";
                  if (dt.Rows[i]["isread"].ToString() == "0")
                      dr["isread"] = "未读";
                  else
                      dr["isread"] = "已读";
                  dr["sendtime"] = String.Format("{0:g}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["sendtime"].ToString(), DateTime.Now));
                  dt2.Rows.Add(dr);
              }
              _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
              dt.Clear();
              dt.Dispose();
          }
      }
    }
}
