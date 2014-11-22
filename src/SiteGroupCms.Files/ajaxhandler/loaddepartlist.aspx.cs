using System;
using System.Web;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 部门列表加载控制层
    /// </summary>
    public partial class loaddepartlist :SiteGroupCms.Ui.AdminCenter
    {
        int currentpage = 1;
        int pagesize = 1;
        string sortname = "id";
        string sortorder = "desc";
        string _response="";
        private SiteGroupCms.Entity.Depart depart=new SiteGroupCms.Entity.Depart();
        private SiteGroupCms.Dal.DepartDal departdal=new SiteGroupCms.Dal.DepartDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
               
            }
            if (Request.Form["sortname"] != null && Request.Form["sortname"] != "state")
                sortname = Request.Form["sortname"];
            if (Request.Form["sortorder"] != null && Request.Form["sortorder"] != "")
                sortorder = Request.Form["sortorder"];
            if (Request.Form["page"] != null && Request.Form["page"] != "")
                currentpage = Validator.StrToInt(Request.Form["page"], 1);
            if (Request.Form["pagesize"] != null && Request.Form["pagesize"] != "")
                pagesize = Validator.StrToInt(Request.Form["pagesize"], 1);
            departdal.GetListJSON(currentpage, pagesize, "1=1", ref _response, sortname, sortorder);
            Response.Write(_response);

        }
    }
}
