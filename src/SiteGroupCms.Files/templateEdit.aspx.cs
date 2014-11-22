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

namespace SiteGroupCms
{
    public partial class templateEdit :SiteGroupCms.Ui.AdminCenter
    {
        public string id = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    id = Request.QueryString["id"];
                    SiteGroupCms.Dal.Normal_TemplateDAL templatedal = new SiteGroupCms.Dal.Normal_TemplateDAL();
                    SiteGroupCms.Entity.Normal_Template template = templatedal.GetEntity(id);
                    if (template != null)
                    {
                        try
                        {
                            content2.Value= SiteGroupCms.Utils.DirFile.ReadFile(template.Source);
                        }
                        catch (Exception)
                        {
                            content2.Value = "";
                        }
                    }
                }
            }
        }
    }
}
