using System;
using System.Web;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 日志列表加载控制层
    /// </summary>
    public partial class loadloglist :SiteGroupCms.Ui.AdminCenter
    {
        string type = string.Empty;
        int currentpage = 1;
        int pagesize = 1;
        string sortname = "id";
        string sortorder = "desc";
        string where = "";
        string _response = String.Empty;
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        SiteGroupCms.Entity.Log log = new SiteGroupCms.Entity.Log();
        SiteGroupCms.Dal.LogDal logdal = new SiteGroupCms.Dal.LogDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
                where += "siteid="+_admin.CurrentSite;
            }
            if (Request.Form["sortname"] != null && Request.Form["sortname"] != "state")
                sortname = Request.Form["sortname"];
            if (Request.Form["sortorder"] != null && Request.Form["sortorder"] != "")
                sortorder = Request.Form["sortorder"];
            if (Request.Form["page"] != null && Request.Form["page"] != "")
                currentpage = Validator.StrToInt(Request.Form["page"], 1);
            if (Request.Form["pagesize"] != null && Request.Form["pagesize"] != "")
                pagesize = Validator.StrToInt(Request.Form["pagesize"], 1);
            logdal.GetListJSON(currentpage, pagesize, where,ref  _response);
            Response.Write(_response);

        }
    }
}
