using System;
using System.Data;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 模板列表页加载控制层
    /// </summary>
    public partial class loadtemplatelist :SiteGroupCms.Ui.AdminCenter
    {
        string type = String.Empty;
        int currentpage = 1;
        int pagesize = 1;
        string sortname = "id";
        string sortorder = "desc";
        string where = "1=1";
        string _response = String.Empty;
        SiteGroupCms.Dal.Normal_TemplateDAL templatedal = new SiteGroupCms.Dal.Normal_TemplateDAL();
        SiteGroupCms.Entity.Normal_Template template = new SiteGroupCms.Entity.Normal_Template();
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
                where += " and siteid=" + _admin.CurrentSite;
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
            templatedal.GetListJSON(currentpage, pagesize, where, ref _response, sortname, sortorder);
            Response.Write(_response);
        }
    }
}
