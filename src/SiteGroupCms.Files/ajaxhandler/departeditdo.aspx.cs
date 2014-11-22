using System;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 部门类新增、修改的控制层
    /// </summary>
    public partial class departeditdo : SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Entity.Depart depart = new SiteGroupCms.Entity.Depart();
        SiteGroupCms.Dal.DepartDal departobj = new SiteGroupCms.Dal.DepartDal();

        string departid = string.Empty;
        string departname = string.Empty;
        string departdescription = string.Empty;
        string method = string.Empty;
        string _response = "";
        int id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");

            }
            method = Request.QueryString["method"];
            departdescription = Request.Form["description"];
            departname = Request.Form["name"];
            departid = Request.Form["deptid"];
          
            switch (method)
            {
                case "add":
                    adddepart();
                    break;
                case "update":
                    updatedepart();
                    break;

            }
            Response.Write(_response);
        }
        private void adddepart()
        {
            depart.Description = departdescription;
            depart.Name = departname;
            id = departobj.insertEntity(depart);
            if (id != 0)
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(23);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";
        }
        public void updatedepart()
        {
          
            depart.Description = departdescription;
            depart.Name = departname;
            depart.Id =Str2Int(departid);
            if (departobj.UpdateEntity(depart))
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(24);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";

         
        }
    }
}
