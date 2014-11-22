using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
   public class DepartDal :Common
    {
       public DepartDal()
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
               if (_doh.Exist("yy_deptinfo"))
                   _ext = 1;
               return (_ext == 1);
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
               _doh.ConditionExpress = "dept=@dept and id<>" + _id;
               if (_wherestr != "") _doh.ConditionExpress += " and " + _wherestr;
               _doh.AddConditionParameter("@dept", _title);
               if (_doh.Exist("yy_deptinfo"))
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
       public void GetListJSON(int _thispage, int _pagesize, string _wherestr, ref string _jsonstr, string ordercol, string ordertype)
       {
           SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
           AdminDal _adminobj = new AdminDal();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.ConditionExpress = _wherestr;
               string sqlStr = "";
               int _countnum = _doh.Count("yy_deptinfo");
               sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_deptinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr);
               _doh.Reset();
               _doh.SqlCmd = sqlStr;
               DataTable dt = _doh.GetDataTable();
               DataTable dt2 = new DataTable();
               DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
               DataColumn col2 = new DataColumn("name", System.Type.GetType("System.String"));
               DataColumn col3 = new DataColumn("TotalNum", System.Type.GetType("System.String"));
               DataColumn col4 = new DataColumn("description", System.Type.GetType("System.String"));
               dt2.Columns.Add(col);
               dt2.Columns.Add(col2);
               dt2.Columns.Add(col3);
               dt2.Columns.Add(col4);
               SiteGroupCms.Dal.AdminDal Admindal = new SiteGroupCms.Dal.AdminDal();
               
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   DataRow dr = dt2.NewRow();
                   dr["id"] = dt.Rows[i]["id"];
                   dr["name"] = dt.Rows[i]["dept"];
                   dr["description"]=dt.Rows[i]["description"];
                   dr["TotalNum"] = Admindal.GetDeptNum(dt.Rows[i]["id"].ToString()).ToString();
                   dt2.Rows.Add(dr);
               }
               _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
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
               int _del = _doh.Delete("yy_deptinfo");
               return (_del == 1);
           }
       }
       public bool DeleteByIDs(string _ids)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.ConditionExpress = "id in ("+_ids+")";
               int _del = _doh.Delete("yy_deptinfo");
               return (_del >=1);
           }
       }
     

       /// <summary>
       /// 获得单页内容的单条记录实体
       /// </summary>
       /// <param name="_id"></param>
       public SiteGroupCms.Entity.Depart GetEntity(string _id)
       {
           SiteGroupCms.Entity.Depart dept = new SiteGroupCms.Entity.Depart();
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_deptinfo] WHERE [Id]=" + _id;
               DataTable dt = _doh.GetDataTable();
               if (dt.Rows.Count > 0)
               {
                   dept.Id = Str2Int(_id);
                   dept.Name = dt.Rows[0]["dept"].ToString();
                   dept.Description = dt.Rows[0]["description"].ToString();
                   //部门人数
                   dept.TotalNum = 12;

               }
               return dept;
           }
       }

       /// <summary>
       /// 获得单页内容的单条记录实体
       /// </summary>
       /// <param name="_id"></param>
       public DataTable GetDT(string where)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.SqlCmd = "SELECT * FROM [yy_deptinfo] WHERE " + where;
               DataTable dt = _doh.GetDataTable();
               if (dt.Rows.Count > 0)
                   return dt;
               else
                   return null;
           }
       }
       /// <summary>
       /// 更新文章
       /// </summary>
       /// <param name="object">对象</param>
       public bool UpdateEntity(SiteGroupCms.Entity.Depart obj)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.ConditionExpress = "id=@id";
               _doh.AddConditionParameter("@id", obj.Id);
               _doh.AddFieldItem("dept", obj.Name);
               _doh.AddFieldItem("description", obj.Description);
               int _update = _doh.Update("yy_deptinfo");
               return (_update == 1);
           }
       }
       /// <summary>
       /// 插入文章
       /// </summary>
       /// <param name="object">对象</param>
       public int insertEntity(SiteGroupCms.Entity.Depart obj)
       {
           using (DbOperHandler _doh = new Common().Doh())
           {
               _doh.Reset();
               _doh.AddFieldItem("dept", obj.Name);
               _doh.AddFieldItem("description", obj.Description);
               int _insert = _doh.Insert("yy_deptinfo");
               return _insert;
           }
       }
    }
}
