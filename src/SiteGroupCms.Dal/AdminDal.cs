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
    /// 管理员表信息
    /// </summary>
    public class AdminDal : Common
    {
        public AdminDal()
        {
            base.SetupSystemDate();
        }

        public SiteGroupCms.Entity.Admin GetEntity(string _adminid)
        {
            SiteGroupCms.Entity.Admin admin = new SiteGroupCms.Entity.Admin();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_userinfo] WHERE [id]=" + _adminid;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    admin.Id = Str2Int(_adminid);
                    admin.Password = dt.Rows[0]["password"].ToString();
                    admin.UserName = dt.Rows[0]["username"].ToString();
                    admin.LastLoginIp = dt.Rows[0]["lastloginip"].ToString();
                    admin.LastLoginTime = Validator.StrToDate(dt.Rows[0]["lastlogintime"].ToString(), DateTime.Now);
                    admin.islock = Str2Int(dt.Rows[0]["islock"].ToString());
                    admin.AddTime = Validator.StrToDate(dt.Rows[0]["addtime"].ToString(), DateTime.Now);
                    admin.SiteId = Str2Int(dt.Rows[0]["siteid"].ToString());
                    admin.RoleId = Str2Int(dt.Rows[0]["roleid"].ToString());
                    admin.Catalogid = dt.Rows[0]["catalogid"].ToString();
                }
                _doh.Reset();
                _doh.SqlCmd = "select * from [yy_personinfo] where [uid] =" + _adminid;
                dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    admin.TrueName = dt.Rows[0]["truename"].ToString();
                    admin.DeptId = Str2Int(dt.Rows[0]["deptid"].ToString());
                    /* if (dt.Rows[0]["sex"].ToString() == "0")
                         admin.Sex = "未知";
                     else if (dt.Rows[0]["sex"].ToString() == "1")
                         admin.Sex = "男";
                     else
                         admin.Sex = "女";
                     * */
                    admin.Sex = dt.Rows[0]["sex"].ToString();
                    admin.Job = dt.Rows[0]["job"].ToString();
                    admin.Email = dt.Rows[0]["email"].ToString();
                    admin.Telphone = dt.Rows[0]["telphone"].ToString();
                    admin.MobilePhone = dt.Rows[0]["mobilephone"].ToString();
                    admin.Imgurl = dt.Rows[0]["imgurl"].ToString();
                    admin.Sort = Str2Int(dt.Rows[0]["sort"].ToString());
                }
                _doh.Reset();
                _doh.SqlCmd = "select * from [yy_deptinfo] where [id]=" + admin.DeptId;
                dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    admin.DeptName = dt.Rows[0]["dept"].ToString();
                }
                _doh.Reset();
                _doh.SqlCmd = "select * from [yy_roleinfo] where [id]=" + admin.RoleId;
                dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    admin.Rights = dt.Rows[0]["rights"].ToString();

                }
            }
            return admin;
        }
        /// <summary>
        /// 验证管理员登录
        /// </summary>
        /// <param name="_adminname">登录名</param>
        /// <param name="_adminpass">密码</param>
        /// <returns></returns>
        public string ChkAdminLogin(string _adminname, string _adminpass)
        {
            _adminname = _adminname.Replace("\'", "");
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "username=@username and islock=0";
                _doh.AddConditionParameter("@username", _adminname);
                string _adminid = _doh.GetField("yy_userinfo", "id").ToString();
                if (_adminid != "0" && _adminid != "")
                {
                    SiteGroupCms.Entity.Admin _Admin = GetEntity(_adminid);
                    //判断是否锁定
                    bool islock = _Admin.islock == 0 ? false : true;
                    if (islock)
                        return "用户锁定";
                    if (_Admin.Password.Length == 16)//16加密
                    {
                        if (_Admin.Password.ToLower() != SiteGroupCms.Utils.MD5.Lower16(_adminpass))
                        {
                            _doh.Dispose();
                            return "密码错误";
                        }
                    }
                    else
                    {
                        if (_Admin.Password.ToLower() != SiteGroupCms.Utils.MD5.Lower32(_adminpass))
                        {
                            _doh.Dispose();
                            return "密码错误";
                        }
                    }
                    /* string _adminCookiess = "c" + (new Random().Next(10000000, 99999999)).ToString();
                     //设置Cookies
                     System.Collections.Specialized.NameValueCollection myCol = new System.Collections.Specialized.NameValueCollection();
                     myCol.Add("id", _adminid);
                     myCol.Add("name", _adminname);
                     myCol.Add("password", SiteGroupCms.Utils.MD5.Lower32(_adminpass));
                     myCol.Add("cookies", _adminCookiess);
                     SiteGroupCms.Utils.Cookie.SetObj(site.CookiePrev + "admin", iExpires, myCol, site.CookieDomain, site.CookiePath);
                     */


                    //写入session
                    if (_Admin.Rights.Contains("9"))//如果是超级管理员 对定位到主站点
                        _Admin.CurrentSite = new SiteDal().GetBaseEntity().ID;
                    else
                        _Admin.CurrentSite = _Admin.SiteId;

                    HttpContext.Current.Session["site"] = new SiteGroupCms.Dal.SiteDal().GetEntity(_Admin.CurrentSite);
                    HttpContext.Current.Session["admin"] = _Admin;
                    HttpContext.Current.Session.Timeout = 100000;//设置session过期的时间

                    //更新管理员登陆信息 包括最近登录ip 最近登录时间等
                    _doh.Reset();
                    _doh.ConditionExpress = "id=@userid and islock=0";
                    _doh.AddConditionParameter("@userid", _adminid);
                    _doh.AddFieldItem("lastlogintime", DateTime.Now.ToString());
                    _doh.AddFieldItem("lastloginip", IPHelp.ClientIP);
                    _doh.Update("yy_userinfo");
                    //插入日志窗口



                    _doh.Dispose();
                    return "ok";
                }
                else
                {
                    _doh.Dispose();
                    return "帐号不存在";
                }
            }

        }
        /// <summary>
        /// 管理员退出登录
        /// </summary>
        public void ChkAdminLogout()
        {
            if (HttpContext.Current.Session["admin"] != null || HttpContext.Current.Session["admin"].ToString() != "")
            {
                HttpContext.Current.Session["admin"] = null;
            }

        }
        /// <summary>
        /// 判断adminsign是否正确
        /// </summary>
        /// <param name="_adminid"></param>
        /// <param name="_adminsign">长度一定是32位</param>
        /// <returns></returns>
        public bool ChkAdminSign(string _adminid, string _adminsign)
        {
            if (_adminsign.Length != 32 || _adminid == "")
            {
                return false;
            }
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "adminid=@adminid and adminsign=@adminsign and adminstate=1";
                _doh.AddConditionParameter("@adminid", _adminid);
                _doh.AddConditionParameter("@adminsign", _adminsign);
                return (_doh.Exist("jcms_normal_user"));
            }
        }
        //获取某个部门的人员数名
        public int GetDeptNum(string deptid)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "deptid=" + deptid;
                return _doh.Count("yy_personinfo");
            }
        }

        //更新管理员
        public bool Updates(SiteGroupCms.Entity.Admin obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "uid=@id";
                _doh.AddConditionParameter("@id", obj.Id);
                _doh.AddFieldItem("truename", obj.TrueName);
                _doh.AddFieldItem("deptid", obj.DeptId);
                _doh.AddFieldItem("sex", obj.Sex);
                _doh.AddFieldItem("job", obj.Job);
                _doh.AddFieldItem("email", obj.Email);
                _doh.AddFieldItem("telphone", obj.Telphone);
                _doh.AddFieldItem("mobilephone", obj.MobilePhone);
                _doh.AddFieldItem("imgurl", obj.Imgurl);
                _doh.AddFieldItem("sort", obj.Sort);
                int _update = _doh.Update("yy_personinfo");


                //更新user表
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", obj.Id);
                _doh.AddFieldItem("password", obj.Password);
                _doh.AddFieldItem("roleid", obj.RoleId);
                _doh.AddFieldItem("siteid", obj.SiteId);
                _doh.AddFieldItem("catalogid", obj.Catalogid);
                int _update2 = _doh.Update("yy_userinfo");
                return (_update + _update2 == 2);
            }
        }

        public void GetListJSON(int type, int _thispage, int _pagesize, string _wherestr, ref string _jsonstr, string ordercol, string ordertype)
        {
            //type 用来表示是选择站点的还是所有的 0，all，1 当前站点
            //特别注意一定要保证user表和person表的一致性 为了保证在检索的时候的方便，以personinfo作为基准
            SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
            SiteGroupCms.Entity.Admin currentadmin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
            AdminDal _adminobj = new AdminDal();
            using (DbOperHandler _doh = new Common().Doh())
            {
                string sqlStr = "";
                string sqlStr2 = "";
                string sqlStr3 = "";
                sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_personinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr);
                string wheres = "";
                if (type == 1)//显示当前站点的
                    wheres = " id in (SELECT uid From yy_personinfo Where " + _wherestr + ") and siteid=" + currentadmin.CurrentSite;
                //wheres = " id in (" + SiteGroupCms.Utils.SqlHelp.GetSql("uid", "yy_personinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr) + ") and siteid=" + currentadmin.CurrentSite;
                else
                    wheres = " id in (SELECT uid From yy_personinfo Where " + _wherestr + ")";
                // wheres = " id in (" + SiteGroupCms.Utils.SqlHelp.GetSql("uid", "yy_personinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr) + ")";

                sqlStr2 = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_userinfo", ordercol, _pagesize, _thispage, ordertype, wheres);

                sqlStr3 = "SELECT  * From yy_userinfo Where  " + wheres;
                //用来没有输入检索条件的数据显示
                _doh.Reset();
                _doh.SqlCmd = sqlStr;
                DataTable dt3 = _doh.GetDataTable();//personinfo表

                //用来统计总条数
                _doh.Reset();
                _doh.SqlCmd = sqlStr3;
                int _countnum = _doh.GetDataTable().Rows.Count;

                //用来输入检索条件的数据显示
                _doh.Reset();
                _doh.SqlCmd = sqlStr2;
                DataTable dt = _doh.GetDataTable();//userinfo表的

                DataTable dt2 = new DataTable();
                DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
                DataColumn col2 = new DataColumn("username", System.Type.GetType("System.String"));
                DataColumn col3 = new DataColumn("truename", System.Type.GetType("System.String"));
                DataColumn col4 = new DataColumn("depttitle", System.Type.GetType("System.String"));
                DataColumn col5 = new DataColumn("job", System.Type.GetType("System.String"));
                DataColumn col9 = new DataColumn("state", System.Type.GetType("System.String"));
                DataColumn col6 = new DataColumn("role", System.Type.GetType("System.String"));
                DataColumn col7 = new DataColumn("addtime", System.Type.GetType("System.String"));
                DataColumn col8 = new DataColumn("logintime", System.Type.GetType("System.String"));
                DataColumn col10 = new DataColumn("sort", System.Type.GetType("System.String"));
                dt2.Columns.Add(col);
                dt2.Columns.Add(col2);
                dt2.Columns.Add(col3);
                dt2.Columns.Add(col4);
                dt2.Columns.Add(col5);
                dt2.Columns.Add(col6);
                dt2.Columns.Add(col7);
                dt2.Columns.Add(col8);
                dt2.Columns.Add(col9);
                dt2.Columns.Add(col10);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt2.NewRow();
                    dr["id"] = dt.Rows[i]["id"];
                    dr["username"] = dt.Rows[i]["username"];
                    if (dt.Rows[i]["islock"].ToString() == "0")
                        dr["state"] = "未锁定";
                    else
                        dr["state"] = "<span style='color:red'>已锁定</span>";
                    dr["truename"] = dt3.Rows[i]["truename"];
                    DepartDal departdal = new DepartDal();
                    dr["depttitle"] = departdal.GetEntity(dt3.Rows[i]["deptid"].ToString()).Name;
                    dr["job"] = dt3.Rows[i]["job"].ToString();
                    dr["role"] = new RoleDal().GetEntity(Str2Int(dt.Rows[i]["roleid"].ToString())).Title;
                    dr["addtime"] = String.Format("{0:g}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["addtime"].ToString(), DateTime.Now));
                    dr["logintime"] = String.Format("{0:g}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["lastlogintime"].ToString(), DateTime.Now));
                    dr["sort"] = dt3.Rows[i]["sort"].ToString();
                    dt2.Rows.Add(dr);
                }
                _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
                dt.Clear();
                dt.Dispose();
            }
        }
        public int AddAdmin(SiteGroupCms.Entity.Admin obj)
        {
            // 先检查是否存在同样的username 相同的登录名 先添加到user表 在添加到person表
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select * from yy_userinfo where username='" + obj.UserName + "'";
                if (_doh.GetDataTable().Rows.Count > 0)
                    return -1;//已经存在该用户名了
                _doh.Reset();
                _doh.AddFieldItem("username", obj.UserName);
                _doh.AddFieldItem("password", obj.Password);
                _doh.AddFieldItem("roleid", obj.RoleId);
                _doh.AddFieldItem("siteid", obj.SiteId);
                _doh.AddFieldItem("catalogid", obj.Catalogid);
                int _insert = _doh.Insert("yy_userinfo");
                if (_insert > 0)//前面插入成功
                {
                    _doh.Reset();
                    _doh.AddFieldItem("uid", _insert);
                    _doh.AddFieldItem("truename", obj.TrueName);
                    _doh.AddFieldItem("deptid", obj.DeptId);
                    _doh.AddFieldItem("sex", obj.Sex);
                    _doh.AddFieldItem("job", obj.Job);
                    _doh.AddFieldItem("email", obj.Email);
                    _doh.AddFieldItem("telphone", obj.Telphone);
                    _doh.AddFieldItem("mobilephone", obj.MobilePhone);
                    _doh.AddFieldItem("imgurl", obj.Imgurl);
                    if (_doh.Insert("yy_personinfo") > 0)
                        return _insert;
                    else //常态下不让直行，可回滚操作 否则会导致user表和person表不一致
                        return 0;
                }
                else
                    return 0;
            }
        }
        public bool deletes(string ids)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                //先删除person再删除user表
                _doh.Reset();
                _doh.ConditionExpress = "uid in (" + ids + ")";
                int _del = _doh.Delete("yy_personinfo");
                _doh.Reset();
                _doh.ConditionExpress = "id in (" + ids + ")";
                int _del2 = _doh.Delete("yy_userinfo");
                return (_del + _del2 > 0);
            }

        }

        public bool setlock(string ids, int types) //0解锁 1，锁定
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id in (" + ids + ")";
                _doh.AddFieldItem("islock", types);
                return _doh.Update("yy_userinfo") >= 1;
            }

        }
        public DataTable GetDT(string where)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select * from yy_userinfo where " + where;
                return _doh.GetDataTable();
            }
        }

        public DataTable GetpersonDT(string where, string order)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select * from yy_personinfo where " + where + " order by " + order;
                return _doh.GetDataTable();
            }
        }

        public DataTable Getdepartdt(string departname, string order)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "select id from yy_deptinfo where dept like '%" + departname + "%'";
                DataTable temp = _doh.GetDataTable();
                if (temp != null)
                {
                    _doh.SqlCmd = "select * from yy_personinfo where deptid = " + temp.Rows[0]["id"].ToString() + " and uid in (select id from yy_userinfo where islock=0) order by " + order;
                    return _doh.GetDataTable();
                }
                else
                    return null;
            }
        }

    }
}
