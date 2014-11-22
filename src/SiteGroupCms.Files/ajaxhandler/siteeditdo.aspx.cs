using System;
using System.Data;
using System.Web;


namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 站点类的新增、修改、删除类
    /// </summary>
    public partial class siteeditdo :SiteGroupCms.Ui.AdminCenter
    {
        string title = "";
        string webtitle = "";
        string id = "";//接收query或者form传来的id
        string location = "";//接收删除时传来的字符组
        string domain = "";
        string iswork = "";
        string keywords = "";
        string description = "";
        string indextemplate = "";
        string listtemplate = "";
        string contenttemplate = "";
        string ftpserver = "";
        string ftpuser = "";
        string ftppwd = "";
        string ftpport = "";
        string ftpdir = "";
        string mailserver = "";
        string mailuser = "";
        string mailpwd = "";
        string _response = "";
        string method = "";
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);
            }
            method = Request.QueryString["method"];
            id = Request.QueryString["id"] == "" ? Request.Form["userid"] : Request.QueryString["id"];
             title =Request.Form["title"];
             webtitle = Request.Form["webtitle"];
             location = Request.Form["location"];
             domain = Request.Form["domain"];
             iswork = Request.Form["iswork_val"];
             keywords = Request.Form["keywords"];
             description = Request.Form["description"];
             indextemplate = Request.Form["indextemplate_val"];
             listtemplate = Request.Form["listtemplate_val"];
             contenttemplate = Request.Form["contenttemplate_val"];
             ftpserver = Request.Form["ftpserver"];
             ftpuser = Request.Form["ftpuser"];
             ftppwd = Request.Form["ftppwd"];
             ftpport = Request.Form["ftpport"];
             ftpdir = Request.Form["ftpdir"];
             mailserver = Request.Form["emailserver"];
             mailuser = Request.Form["emailuser"];
             mailpwd = Request.Form["emailpwd"];
            switch (method)
            {
                case "add":
                    addsite();
                    break;
                case "update":
                    updatesite();
                    break;
                case "delete":
                    deletesite();
                    break;
            }
            Response.Write(_response);
        }
        public void addsite()
        {
            SiteGroupCms.Entity.Site site = new SiteGroupCms.Entity.Site();
            SiteGroupCms.Dal.SiteDal sitedal = new SiteGroupCms.Dal.SiteDal();

            site.Title = title;
            site.WebTitle = webtitle;
            site.Keyword = keywords;
            site.Description = description;
            site.Location = location;  //不可修改
            site.Domain = domain;
            site.IsWork = Str2Int(iswork);
            site.Indextemplate = Str2Int(indextemplate);
            site.Listtemplate = Str2Int(listtemplate);
            site.Contenttemplate = Str2Int(contenttemplate);
            site.EmailServer = mailserver;
            site.EmailUser = mailuser;
            site.EmailPwd = mailpwd;
            site.FtpDir = ftpdir;
            site.FtpPort = Str2Int(ftpport);
            site.FtpUser = ftpuser;
            site.FtpPwd = ftppwd;
            site.FtpServer = ftpserver;
            if (sitedal.Exists("location='" + site.Location + "'"))
            {
                _response = "{\"IsError\":true,\"Message\":\"存在相同路径了\",\"Data\":0}";
                return;
            }
            if (sitedal.addsite(site)>0)
            {
                _response = "{\"IsError\":false,\"Message\":\"新建成功\",\"Data\":0}";
                SiteGroupCms.Utils.DirFile.CreateDir("/sites/" + site.Location + "/pub");
                SiteGroupCms.Utils.DirFile.CreateDir("/sites/" + site.Location + "/templates/atts");
                new SiteGroupCms.Dal.LogDal().SaveLog(21);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"新建失败\",\"Data\":0}";
        }
        public void updatesite()
        {
            SiteGroupCms.Entity.Site site = new SiteGroupCms.Entity.Site();
            SiteGroupCms.Dal.SiteDal sitedal = new SiteGroupCms.Dal.SiteDal();
            if (id == "0")//是当前站点
                site.ID = ((SiteGroupCms.Entity.Admin)(Session["admin"])).CurrentSite;
            else
            site.ID = Str2Int(id);
            site.Title = title;
            site.WebTitle = webtitle;
            site.Keyword = keywords;
            site.Description = description;
           // site.Location = location;  //不可修改
            site.Domain = domain;
            site.IsWork =Str2Int(iswork);
            site.Indextemplate = Str2Int(indextemplate);
            site.Listtemplate = Str2Int(listtemplate);
            site.Contenttemplate = Str2Int(contenttemplate);
            site.EmailServer = mailserver;
            site.EmailUser = mailuser;
            site.EmailPwd = mailpwd;
            site.FtpDir = ftpdir;
            site.FtpPort = Str2Int(ftpport);
            site.FtpUser = ftpuser;
            site.FtpPwd = ftppwd;
            site.FtpServer = ftpserver;
            if (sitedal.updatesite(site))
            {
                _response = "{\"IsError\":false,\"Message\":\"修改成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(20);
                if(id=="0")
                Session["site"] = sitedal.GetEntity(site.ID);  //更新session的值
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"修改失败\",\"Data\":0}";
        }
        public void deletesite()
        {
            new SiteGroupCms.Dal.LogDal().SaveLog(33);
        }
    }
}
