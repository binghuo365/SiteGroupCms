using System;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
namespace SiteGroupCms.Dal
{   
    /// <summary>
    /// 模板表信息
    /// </summary>
    public class Normal_TemplateDAL :Common
    {
        public Normal_TemplateDAL()
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
                if (_doh.Exist("yy_templateinfo"))
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
                _doh.ConditionExpress = "title=@title and id<>" + _id;
                if (_wherestr != "") _doh.ConditionExpress += " and " + _wherestr;
                _doh.AddConditionParameter("@title", _title);
                if (_doh.Exist("yy_templateinfo"))
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
            CatalogDal catadalobj = new CatalogDal();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = _wherestr;
                string sqlStr = "";
                int _countnum = _doh.Count("yy_templateinfo");
                sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_templateinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr);
                _doh.Reset();
                _doh.SqlCmd = sqlStr;
                DataTable dt = _doh.GetDataTable();
                DataTable dt2 = new DataTable();
                DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
                DataColumn col2 = new DataColumn("type", System.Type.GetType("System.String"));
                DataColumn col3 = new DataColumn("title", System.Type.GetType("System.String"));
                DataColumn col4 = new DataColumn("source", System.Type.GetType("System.String"));
                DataColumn col5 = new DataColumn("addtime", System.Type.GetType("System.String"));
                DataColumn col6 = new DataColumn("filename", System.Type.GetType("System.String"));
                dt2.Columns.Add(col);
                dt2.Columns.Add(col2);
                dt2.Columns.Add(col3);
                dt2.Columns.Add(col4);
                dt2.Columns.Add(col5);
                dt2.Columns.Add(col6);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt2.NewRow();
                    dr["id"] = dt.Rows[i]["id"];
                    dr["title"] = dt.Rows[i]["title"];
                    dr["source"] = dt.Rows[i]["source"];
                    if (dt.Rows[i]["type"].ToString() == "1")
                        dr["type"] = "系统首页";
                    else if (dt.Rows[i]["type"].ToString() == "2")
                        dr["type"] = "栏目页";
                    else if (dt.Rows[i]["type"].ToString() == "3")
                        dr["type"] = "内容页";
                    else
                        dr["type"] = "公共页";
                    dr["addtime"] = String.Format("{0:g}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["addtime"].ToString(), DateTime.Now));
                    dr["filename"] = dt.Rows[i]["filename"];
                    dt2.Rows.Add(dr);


                }
                _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
                dt.Clear();
                dt.Dispose();
            }
        }

        /// <summary>
        /// 删除一条数据  
        /// </summary>
        public bool DeleteByID(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                int _del = _doh.Delete("yy_templateinfo");
                return (_del == 1);
            }
        }

        public bool DeleteByIDs(string _ids)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id in ("+_ids+")";
                int _del = _doh.Delete("yy_templateinfo");
                return (_del == 1);
            }
        }

        /// <summary>
        /// 获得单页内容的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        public SiteGroupCms.Entity.Normal_Template GetEntity(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                SiteGroupCms.Entity.Normal_Template template = new SiteGroupCms.Entity.Normal_Template();
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_templateinfo] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    template.Id = Str2Int(_id);
                    template.Title = dt.Rows[0]["title"].ToString();
                    template.Siteid = Str2Int(dt.Rows[0]["siteid"].ToString());
                    template.Type =Str2Int(dt.Rows[0]["type"].ToString());
                    template.Source = dt.Rows[0]["source"].ToString();
                    template.Imagedirname = dt.Rows[0]["imagedirname"].ToString();
                    template.Addtime = SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[0]["addtime"].ToString(), DateTime.Now);
                }
                return template;
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
                SiteGroupCms.Entity.Normal_Template template = new SiteGroupCms.Entity.Normal_Template();
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_templateinfo] WHERE " + where;
                DataTable dt = _doh.GetDataTable();
                return dt;
            }
        }
        /// <summary>
        /// 获得模板内容
        /// </summary>
        /// <param name="_id">模板ID，0表示获得默认的首页模板</param>
        /// <param name="_pagestr">输出模板内容</param>
        public void GetTemplateContent(string _id, ref string _pagestr)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                if (_id != "0")
                    _doh.SqlCmd = "SELECT TOP 1 * FROM [yy_templateinfo] WHERE [Id]=" + _id;
                else
                    _doh.SqlCmd = "SELECT TOP 1 * FROM [yy_templateinfo] WHERE [Type]='1'";
                DataTable dtTemplate = _doh.GetDataTable();
                if (dtTemplate.Rows.Count > 0)
                {
                    _pagestr = SiteGroupCms.Utils.DirFile.ReadFile(dtTemplate.Rows[0]["source"].ToString());
                    //_projectid = dtTemplate.Rows[0]["pid"].ToString();
                    //_pagestr = SiteGroupCms.Utils.DirFile.ReadFile("~/templates/" + (new Normal_TemplateProjectDAL()).GetDir(_projectid) + "/" + dtTemplate.Rows[0]["Source"].ToString());
                }
                dtTemplate.Clear();
                dtTemplate.Dispose();
            }
        }
        public string GetSource(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT [Source] FROM [yy_templateinfo] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Source"].ToString();
                }
                return string.Empty;
            }
        }

        public bool Addtemplate(SiteGroupCms.Entity.Normal_Template obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.AddFieldItem("title", obj.Title);
                _doh.AddFieldItem("siteid", obj.Siteid);
                _doh.AddFieldItem("type", obj.Type);
                _doh.AddFieldItem("source", obj.Source);
                //_doh.AddFieldItem("imagedirname", obj.Imagedirname);
                _doh.AddFieldItem("filename", obj.FileName);
                int _insert = _doh.Insert("yy_templateinfo");
                return _insert>0;
            }
        
        }
        public bool updatetemplate(SiteGroupCms.Entity.Normal_Template obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", obj.Id);
                _doh.AddFieldItem("title", obj.Title);
                _doh.AddFieldItem("siteid", obj.Siteid);
                _doh.AddFieldItem("type", obj.Type);
                _doh.AddFieldItem("source", obj.Source);
               // _doh.AddFieldItem("imagedirname", obj.Imagedirname);
                _doh.AddFieldItem("filename", obj.FileName);
                int _insert = _doh.Update("yy_templateinfo");
                return _insert > 0;
            }
        }
    }
}
