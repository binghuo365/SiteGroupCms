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
    public partial class _Default :SiteGroupCms.Ui.AdminCenter
    {
        public string sites = "";
       public string weburl = "";
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        SiteGroupCms.Entity.Site site = new SiteGroupCms.Entity.Site();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("", "html");
                _admin = (SiteGroupCms.Entity.Admin) HttpContext.Current.Session["admin"];
                site=(SiteGroupCms.Entity.Site)HttpContext.Current.Session["site"];
                loadmenu();
                //loadsite();
                weburl ="/sites/"+site.Location+"/pub/index.html"; 
            }
        }

        private void loadmenu()
        { 
           //登录后取权限判断
            SiteGroupCms.Entity.Admin admin = (SiteGroupCms.Entity.Admin)Session["admin"];
            if (admin.Rights.Contains("2"))
                q2.Visible = true;
            if(admin.Rights.Contains("3,"))
            q3.Visible = true;
               if(admin.Rights.Contains("4,"))
            q4.Visible = true;
               if (admin.Rights.Contains("6,"))
            q6.Visible = true;
               if (admin.Rights.Contains("7,"))
               {
                   q7.Visible = true;
                   q71.Visible = true;
               }
               if (admin.Rights.Contains("8,"))//站务管理权限  
               {
                   q8.Visible = true;
                   //对文件管理进行初始化
                   /****************************************************************************************************************/
                   long spaceCapacity = 1000000000;   //用户存储空间大小；
                   SiteGroupCms.Utils.DirFile.CreateDir("/sites/"+site.Location+"/templates/atts");
                   string userSpaceHomeDir = System.Web.HttpContext.Current.Server.MapPath("/sites/" + site.Location + "/templates/atts");
                      //用户存储空间主目录
                   //声明以下几个变量；
                   long spaceUsed = 0;  //已使用的空间大小；
                   int fsObjCount = 0;  //用户空间内的文件和文件夹总数；
                   int fileCount = 0;  //用户空间内的文件总数；
                   //计算出已使用的空间大小；
                   spaceUsed = FSManager.Components.API.GetSpaceUsed(userSpaceHomeDir, ref fsObjCount, ref fileCount);
                   //将数据保存到cookie中，以便文件管理模块的各个页面使用；
                   FSManager.Components.API.Prepare(admin.UserName, userSpaceHomeDir, spaceCapacity, spaceUsed, fsObjCount, fileCount);
                   /****************************************************************************************************************/
               }
                   if (admin.Rights.Contains("9,"))
                   q9.Visible = true;
                   if (admin.Rights.Contains("10,"))
                       q10.Visible = true;
        }
        public void loadsite()
        { 

            SiteGroupCms.Dal.SiteDal sitedal=new SiteGroupCms.Dal.SiteDal();
            DataTable dt=sitedal.GetDT("iswork=1");
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
