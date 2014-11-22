using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace SiteGroupCms.ajaxhandler
{
    public partial class loadright1 : SiteGroupCms.Ui.AdminCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteGroupCms.Entity.Admin _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
            Response.Write(_admin.Rights);
        }
    }
}
