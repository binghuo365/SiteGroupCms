using System;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 用于下拉的树形栏目生成控制层
    /// </summary>
    public partial class loadcataloglist :SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Dal.CatalogDal catalogdal = new SiteGroupCms.Dal.CatalogDal();
        SiteGroupCms.Entity.Catalog catalog = new SiteGroupCms.Entity.Catalog();
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
            }
            Response.Write(catalogdal.getcataloglist(_admin.CurrentSite.ToString()));
        }
    }
}
