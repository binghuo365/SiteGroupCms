using System;
using System.Web;
namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 用户设置控制层
    /// 如新增、修改、锁定、解锁用户
    /// </summary>
    public partial class personinfodo : SiteGroupCms.Ui.AdminCenter
    {
        string truename = "";
        string password = "";
        string id = "";//接收query或者form传来的id
        string ids = "";//接收删除时传来的字符组
        string deptid = "";
        string sex = "";
        string job = "";
        string email = "";
        string telphone = "";
        string mobilephone = "";
        string roleid = "";
        string _response = "";
        string method = "";
        string username = "";
        string siteid = "";
        string imglist = "";
        int sort = 0;
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);
            }
            method = Request.QueryString["methods"];
            id = Request.QueryString["id"] == "" ? Request.Form["userid"] : Request.QueryString["id"];
            ids = Request.Form["ids"];
            password = Request.Form["password"];
            truename = Request.Form["truename"];
            deptid = Request.Form["depttitle_val"];
            sex = Request.Form["sextitle_val"];
            job = Request.Form["job"];
            email = Request.Form["email"];
            telphone = Request.Form["telphone"];
            username = Request.Form["username"];
            mobilephone = Request.Form["mobilephone"];
            method = Request.QueryString["method"];
            siteid = Request.Form["sitetitle_val"];
            roleid = Request.Form["roletitle_val"];
            imglist = Request.Form["imglist"];
            sort = Str2Int(Request.Form["sort"]);
            switch (method)
            {
                case "add":
                    adduser();
                    break;
                case "update":
                    updateuser();
                    break;
                case "delete":
                    deleteuser();
                    break;
                case "lock":
                    lockuser();
                    break;
                case "unlock":
                    unlockuser();
                    break;
            }
            Response.Write(_response);
        }
        public void updateuser()
        {
            SiteGroupCms.Dal.AdminDal admindal = new SiteGroupCms.Dal.AdminDal();
            SiteGroupCms.Entity.Admin admin = new SiteGroupCms.Entity.Admin();
            if (id == "0")
                admin = _admin;
            else
                admin = admindal.GetEntity(id);
            if (password != "")
                admin.Password = SiteGroupCms.Utils.MD5.Lower32(password);
            admin.TrueName = truename;
            admin.DeptId = Str2Int(deptid);
            admin.Sex = sex;
            admin.Job = job;
            admin.Email = email;
            admin.Telphone = telphone;
            admin.MobilePhone = mobilephone;
            admin.SiteId = Str2Int(siteid);
            admin.RoleId = Str2Int(roleid);
            admin.Imgurl = imglist;
            admin.Sort = sort;
            if (admindal.Updates(admin))
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(15);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";
        }
        public void adduser()
        {
            SiteGroupCms.Entity.Admin admin = new SiteGroupCms.Entity.Admin();
            SiteGroupCms.Dal.AdminDal admindal = new SiteGroupCms.Dal.AdminDal();
            if (password == "")
                admin.Password = SiteGroupCms.Utils.MD5.Lower32("123456");//没密码则默认123456
            else
                admin.Password = SiteGroupCms.Utils.MD5.Lower32(password);
            admin.TrueName = truename;
            admin.UserName = username;
            admin.DeptId = Str2Int(deptid);
            admin.Sex = sex;
            admin.Job = job;
            admin.Email = email;
            admin.Telphone = telphone;
            admin.MobilePhone = mobilephone;
            admin.SiteId = Str2Int(siteid);  //若不是超级则不可用
            admin.RoleId = Str2Int(roleid);
            admin.Imgurl = imglist;
            admin.Sort = sort;
            if (admindal.AddAdmin(admin) > 0)
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(14);
            }
            else if (admindal.AddAdmin(admin) == -1)
                _response = "{\"IsError\":true,\"Message\":\"存在相同用户名的用户\",\"Data\":0}";
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";
        }
        public void deleteuser()
        {
            SiteGroupCms.Dal.AdminDal admindal = new SiteGroupCms.Dal.AdminDal();
            if (admindal.deletes(ids))
            {
                _response = "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(16);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"删除失败\",\"Data\":0}";
        }
        public void lockuser()
        {
            SiteGroupCms.Dal.AdminDal admindal = new SiteGroupCms.Dal.AdminDal();
            if (admindal.setlock(ids, 1))
                _response = "{\"IsError\":false,\"Message\":\"锁定成功\",\"Data\":0}";
            else
                _response = "{\"IsError\":true,\"Message\":\"锁定失败\",\"Data\":0}";
        }
        public void unlockuser()
        {
            SiteGroupCms.Dal.AdminDal admindal = new SiteGroupCms.Dal.AdminDal();
            if (admindal.setlock(ids, 0))
                _response = "{\"IsError\":false,\"Message\":\"解锁成功\",\"Data\":0}";
            else
                _response = "{\"IsError\":true,\"Message\":\"解锁失败\",\"Data\":0}";
        }
    }
}
