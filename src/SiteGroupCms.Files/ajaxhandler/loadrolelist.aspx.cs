using System;
using System.Web;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 角色加载控制层
    /// </summary>
    public partial class loadrolelist :SiteGroupCms.Ui.AdminCenter
    {
        string type = String.Empty;
        int currentpage = 1;
        int pagesize = 1;
        string sortname = "id";
        string sortorder = "desc";
        string where = "1=1";
        string _response = String.Empty;
        SiteGroupCms.Dal.RoleDal roledal = new SiteGroupCms.Dal.RoleDal();
        SiteGroupCms.Entity.Role role = new SiteGroupCms.Entity.Role();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");

            }
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                  type = Request.QueryString["type"];
            if (Request.Form["sortname"] != null && Request.Form["sortname"] != "state")
                sortname = Request.Form["sortname"];
            if (Request.Form["sortorder"] != null && Request.Form["sortorder"] != "")
                sortorder = Request.Form["sortorder"];
            if (Request.Form["page"] != null && Request.Form["page"] != "")
                currentpage = Validator.StrToInt(Request.Form["page"], 1);
            if (Request.Form["pagesize"] != null && Request.Form["pagesize"] != "")
                pagesize = Validator.StrToInt(Request.Form["pagesize"], 1);

            if (Request.Form["where"] != null)
                where = Request.Form["where"];
            roledal.GetListJSON(currentpage, pagesize, where, ref _response, sortname, sortorder);
            Response.Write(_response);
        }
    }
}
