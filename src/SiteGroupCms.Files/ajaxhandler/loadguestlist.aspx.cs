using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    public partial class loadguestlist : SiteGroupCms.Ui.AdminCenter
    {
        string type = string.Empty;
        int currentpage = 1;
        int pagesize = 1;
        string where = " 1=1";
        string _response = String.Empty;
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        SiteGroupCms.Entity.Guest guest = new SiteGroupCms.Entity.Guest();
        SiteGroupCms.Dal.GuestDal guestdal = new SiteGroupCms.Dal.GuestDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
            }
            if (Request.Form["page"] != null && Request.Form["page"] != "")
                currentpage = Validator.StrToInt(Request.Form["page"], 1);
            if (Request.Form["pagesize"] != null && Request.Form["pagesize"] != "")
                pagesize = Validator.StrToInt(Request.Form["pagesize"], 1);
            guestdal.GetListJSON(currentpage, pagesize, where, ref  _response);
            Response.Write(_response);
        }
    }
}