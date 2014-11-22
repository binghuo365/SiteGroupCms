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
    public partial class personinfo :SiteGroupCms.Ui.AdminCenter
    {
        public string catalogs = ",";
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);
                if (_admin.Rights.Contains("7,"))//如果包含用户管理权则出现栏目权限信息
                    loadcatalog();
                else
                    catalogs = "";
            }   
        }
        public void loadcatalog()
        {
            SiteGroupCms.Dal.CatalogDal catalogdal = new SiteGroupCms.Dal.CatalogDal();
            DataTable dt = catalogdal.GetDT("fatherid=0", "sort asc");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                    catalogs += " {display:'" + dt.Rows[i]["title"].ToString() + "',name:'catalog" + dt.Rows[i]["id"].ToString() + "',newline:true,labelWidth:200,width:200,space:30,type:'checkbox',group:'频道权限', groupicon:groupicon},";
                else if (i < dt.Rows.Count - 1)
                    catalogs += " {display:'" + dt.Rows[i]["title"].ToString() + "',name:'catalog" + dt.Rows[i]["id"].ToString() + "',newline:true,labelWidth:200,width:200,space:30,type:'checkbox', groupicon:groupicon},";
                else
                    catalogs += " {display:'" + dt.Rows[i]["title"].ToString() + "',name:'catalog" + dt.Rows[i]["id"].ToString() + "',newline:true,labelWidth:200,width:200,space:30,type:'checkbox', groupicon:groupicon}";
            }
        }
    }
}
