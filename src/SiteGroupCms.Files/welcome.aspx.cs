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
    public partial class welcome :SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        SiteGroupCms.Dal.ArticleDal articledal = new SiteGroupCms.Dal.ArticleDal();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Admin_Load("", "json");
                _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
                span0.InnerHtml =articledal.getcount("isdel!=1 and siteid="+_admin.CurrentSite);
                Span1.InnerHtml = articledal.getcount("isdel!=1 and siteid="+_admin.CurrentSite+" and ispassed!=1");
                Span2.InnerHtml = articledal.getcount("isdel!=1 and siteid=" + _admin.CurrentSite + " and ispublish!=1");
                Span3.InnerHtml = articledal.getcount("isdel=1 and siteid=" + _admin.CurrentSite);
                Span4.InnerHtml = articledal.getcount("isdel!=1 and siteid=" + _admin.CurrentSite + " and isshare=1");
            }
        }
    }
}
