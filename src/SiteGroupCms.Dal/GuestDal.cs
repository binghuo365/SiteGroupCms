using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
    public class GuestDal
    {
        public SiteGroupCms.Entity.Guest GetEntity(int id)
        {
            SiteGroupCms.Entity.Guest dos = new SiteGroupCms.Entity.Guest();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_postinfo] WHERE [id]=" + id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    dos.Id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                    dos.Title = dt.Rows[0]["title"].ToString();
                    dos.Content = dt.Rows[0]["content"].ToString();
                    dos.Addtime = Validator.StrToDate(dt.Rows[0]["addtime"].ToString(), DateTime.Now);
                    dos.Userip = dt.Rows[0]["userip"].ToString();
                    dos.Audit = Convert.ToInt32(dt.Rows[0]["audit"].ToString());
                    dos.Username = dt.Rows[0]["username"].ToString();
                }

            }
            return dos;
        }
        public DataTable GetDT(string where, string order)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_postinfo] WHERE " + where + " order by " + order;
                DataTable dt = _doh.GetDataTable();
                return dt;
            }
        }
        public int GetCount(string where)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT count(*) FROM [yy_postinfo] WHERE " + where;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                else
                    return 0;
            }
        }

        public DataTable GetDT(int currentpage, int pagesize, string where, string order)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = where;
                string sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_postinfo", pagesize, currentpage, order, where);
                _doh.Reset();
                _doh.SqlCmd = sqlStr;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
        }
        public int insertguest(SiteGroupCms.Entity.Guest obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.AddFieldItem("username", obj.Username);
                _doh.AddFieldItem("title", obj.Title);
                _doh.AddFieldItem("content", obj.Content);
                _doh.AddFieldItem("userip", IPHelp.ClientIP);
                return _doh.Insert("yy_postinfo");
            }
        }
        public int updateguest(SiteGroupCms.Entity.Guest obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", obj.Id);
                _doh.AddFieldItem("username", obj.Username);
                _doh.AddFieldItem("title", obj.Title);
                _doh.AddFieldItem("content", obj.Content);
                _doh.AddFieldItem("userip", IPHelp.ClientIP);
                _doh.AddFieldItem("audit", obj.Audit);
                if (obj.Addtime.ToString() != "0001/1/1 0:00:00")
                    _doh.AddFieldItem("addtime", obj.Addtime.ToString());
                return _doh.Update("yy_postinfo");
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
        public void GetListJSON(int _thispage, int _pagesize, string _wherestr, ref string _jsonstr)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = _wherestr;
                string sqlStr = "";
                int _countnum = _doh.Count("yy_postinfo");
                // sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_postinfo", "id", _pagesize, _thispage, "desc", _wherestr);
                sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_postinfo", _pagesize, _thispage, " audit asc , id desc", _wherestr);
                _doh.Reset();
                _doh.SqlCmd = sqlStr;
                DataTable dt = _doh.GetDataTable();
                DataTable dt2 = new DataTable();
                DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
                DataColumn col2 = new DataColumn("title", System.Type.GetType("System.String"));
                DataColumn col3 = new DataColumn("content", System.Type.GetType("System.String"));
                DataColumn col4 = new DataColumn("username", System.Type.GetType("System.String"));
                DataColumn col5 = new DataColumn("addtime", System.Type.GetType("System.String"));
                DataColumn col6 = new DataColumn("userip", System.Type.GetType("System.String"));
                DataColumn col7 = new DataColumn("repost", System.Type.GetType("System.String"));
                DataColumn col8 = new DataColumn("retime", System.Type.GetType("System.String"));
                DataColumn col9 = new DataColumn("audit", System.Type.GetType("System.String"));
                dt2.Columns.Add(col);
                dt2.Columns.Add(col2);
                dt2.Columns.Add(col3);
                dt2.Columns.Add(col4);
                dt2.Columns.Add(col5);
                dt2.Columns.Add(col6);
                dt2.Columns.Add(col7);
                dt2.Columns.Add(col8);
                dt2.Columns.Add(col9);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt2.NewRow();
                    dr["id"] = dt.Rows[i]["id"].ToString();
                    dr["title"] = dt.Rows[i]["title"].ToString();
                    dr["content"] = dt.Rows[i]["content"].ToString();
                    dr["username"] = dt.Rows[i]["username"].ToString();
                    dr["addtime"] = String.Format("{0:yy-MM-dd hh:mm}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["addtime"].ToString(), DateTime.Now));
                    dr["userip"] = dt.Rows[i]["userip"].ToString();
                    dr["audit"] = dt.Rows[i]["audit"].ToString() == "0" ? "<span style='color:red;'>否</span>&nbsp;" : "是";
                    _doh.Reset();
                    _doh.SqlCmd = "select * from yy_repostinfo where followid=" + dt.Rows[i]["id"].ToString();
                    DataTable tempdt = _doh.GetDataTable();
                    if (tempdt.Rows.Count > 0)
                    {
                        dr["repost"] = tempdt.Rows[0]["content"].ToString();
                        dr["retime"] = String.Format("{0:yy-MM-dd hh:mm}", SiteGroupCms.Utils.Validator.StrToDate(tempdt.Rows[0]["addtime"].ToString(), DateTime.Now));
                    }
                    else
                    {
                        dr["repost"] = "";
                        dr["retime"] = "";
                    }
                    dt2.Rows.Add(dr);
                }
                _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
                dt.Clear();
                dt.Dispose();
                dt2.Clear();
                dt2.Dispose();

            }
        }
        public bool deletes(string ids)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                //先删除guest再删除repost表
                _doh.Reset();
                _doh.ConditionExpress = "id in (" + ids + ")";
                int _del = _doh.Delete("yy_postinfo");
                _doh.Reset();
                _doh.ConditionExpress = "followid in (" + ids + ")";
                int _del2 = _doh.Delete("yy_repostinfo");
                return (_del + _del2 > 0);
            }

        }

        public bool updatestatues(string ids, int type) //0 为未审核 1为通过
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                //先删除guest再删除repost表
                _doh.Reset();
                _doh.ConditionExpress = "id in (" + ids + ")";
                _doh.AddFieldItem("audit", type);
                return _doh.Update("yy_postinfo") > 0;
            }
        }
    }
}
