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
    public partial class articleEidt :SiteGroupCms.Ui.AdminCenter
    {
       public string id = "0";
       public string content = "";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    id = Request.QueryString["id"];
                    SiteGroupCms.Dal.ArticleDal articledal = new SiteGroupCms.Dal.ArticleDal();
                    SiteGroupCms.Entity.Article article = articledal.GetEntity(id);
                    if (article != null)
                        content2.Value = article.Content;
                }
                
            }
        }
    }
}
