using System;
using System.Collections;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// ftp类修改控制层
    /// </summary>
    public partial class ftpeditdo : SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Entity.Site site = new SiteGroupCms.Entity.Site();
        SiteGroupCms.Dal.SiteDal siteobj = new SiteGroupCms.Dal.SiteDal();

        string ftpserver = string.Empty;
        string ftpport = string.Empty;
        string ftpuser = string.Empty;
        string ftppwd = string.Empty;
        string ftpdir = string.Empty;
        string _response = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");

            }
            SiteGroupCms.Entity.Ftp ftp=new SiteGroupCms.Entity.Ftp();

            ftp.Ftpserver = Request.Form["ftpserver"];
            ftp.Ftpport = Request.Form["ftpport"];
            ftp.Ftpuser = Request.Form["ftpuser"];
            ftp.Ftppwd = Request.Form["ftppwd"];
            ftp.Ftpdir = Request.Form["ftpdir"];
            if (siteobj.updateftp(ftp))
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(22);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";
    
            Response.Write(_response);
        }
    }
}
