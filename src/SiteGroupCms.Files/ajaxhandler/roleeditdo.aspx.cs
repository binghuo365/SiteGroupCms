using System;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 角色类的修改控制层
    /// </summary>
    public partial class roleeditdo :SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Entity.Role role = new SiteGroupCms.Entity.Role();
        SiteGroupCms.Dal.RoleDal roledal = new SiteGroupCms.Dal.RoleDal();
        string title = string.Empty;
        string description = string.Empty;
        string[] qs=new String[11];
        string departdescription = string.Empty;
        string _response = "";
        string id ="";
        string method = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");

            }
            method = Request.QueryString["method"];
            title = Request.Form["title"];
            description = Request.Form["description"];
            id = Request.QueryString["id"];
                qs[1] = Request.Form["q1"];
                qs[2] = Request.Form["q2"];
                qs[3] = Request.Form["q3"];
                qs[4] = Request.Form["q4"];
                qs[5] = Request.Form["q5"];
                qs[6] = Request.Form["q6"];
                qs[7] = Request.Form["q7"];
                qs[8] = Request.Form["q8"];
                qs[9] = Request.Form["q9"];
                qs[10] = Request.Form["q10"];
            switch (method)
            {
                case "add":
                    addrole();
                    break;
                case "update":
                    updaterole();
                    break;
                case "delete":
                    deleterole();
                    break;

            }
            Response.Write(_response);
        }
        public void addrole()
        {
            string rights = "";
            for (int i = 0; i < 10; i++)
            {
                if (qs[i + 1] == "true")
                    rights += i + 1 + ",";
            }
            role.Rights = rights;
            role.Title = title;
            role.Description = description;
            if (roledal.addrole(role) > 0)
            {
                _response = "{\"IsError\":false,\"Message\":\"添加成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(17);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"添加失败\",\"Data\":0}";
            
        }
        public void updaterole()
        {
            string rights = "";
            for (int i = 0; i < 10; i++)
            {
                if (qs[i + 1] == "true")
                    rights += i + 1 + ",";
            }
            role.Rights = rights;
            role.Title = title;
            role.Id =Str2Int(id);
            role.Description = description;
            if (roledal.updaterole(role))
            {
                _response = "{\"IsError\":false,\"Message\":\"修改成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(18);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"修改失败\",\"Data\":0}";
        }
        public void deleterole() //删除
        {
            new SiteGroupCms.Dal.LogDal().SaveLog(19);
        }
    }
}
