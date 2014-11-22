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

namespace SiteGroupCms
{
    public partial class index:SiteGroupCms.Ui.AdminCenter
    {
        public string sites = "";
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("", "html");
                _admin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
                loadmenu();
                loadsite();

            }
        }

        private void loadmenu()
        {
            //登录后取权限判断
            SiteGroupCms.Entity.Admin admin = (SiteGroupCms.Entity.Admin)Session["admin"];
            if (admin.Rights.Contains("3,"))
                q3.Visible = true;
            if (admin.Rights.Contains("4,"))
                q4.Visible = true;
            if (admin.Rights.Contains("6,"))
                q6.Visible = true;
            if (admin.Rights.Contains("7,"))
            {
                q7.Visible = true;
                q71.Visible = true;
            }
            if (admin.Rights.Contains("8,"))
                q8.Visible = true;
            if (admin.Rights.Contains("9,"))
            {
                q9.Visible = true;
                changesites.Visible = true;
            }
        }
        public void loadsite()
        {

            SiteGroupCms.Dal.SiteDal sitedal = new SiteGroupCms.Dal.SiteDal();
            DataTable dt = sitedal.GetDT("iswork=1");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["id"].ToString() == _admin.CurrentSite.ToString())
                    sites += "<option value='" + dt.Rows[i]["id"].ToString() + "' selected=selected>" + dt.Rows[i]["title"].ToString() + "</option>";
                else
                    sites += "<option value='" + dt.Rows[i]["id"].ToString() + "'>" + dt.Rows[i]["title"].ToString() + "</option>";
            }

        }
    }
}
