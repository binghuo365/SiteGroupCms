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
    /// <summary>
    /// 模板类的新增、修改、删除控制层
    /// </summary>
    public partial class templatedo :SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Entity.Normal_Template template = new SiteGroupCms.Entity.Normal_Template();
        SiteGroupCms.Dal.Normal_TemplateDAL templatedal = new SiteGroupCms.Dal.Normal_TemplateDAL();
        string title = string.Empty;
        string temcontent = string.Empty;
        string _response = "";
        string type = "";
        string id = "";
        string method = "";
        string source = "";
        string filename = "";
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        SiteGroupCms.Entity.Site site = new SiteGroupCms.Entity.Site();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
               _admin=(SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
               site=(SiteGroupCms.Entity.Site)HttpContext.Current.Session["site"];
            }
            method = Request.QueryString["method"];
            title = Request.Form["title"];
            temcontent = Request.Form["temcontent"];
            type = Request.Form["type_val"];
            filename=Request.Form["filename"];
            source=Request.Form["source"];
            id = Request.Form["id"];
            switch (method)
            {
                case "add":
                    addtemplate();
                    break;
                case "update":
                    updatetemplate();
                    break;
                case "delete":
                    deletetemplate();
                    break;

            }
            Response.Write(_response);
        }
        public void addtemplate()
        {
            template.Title = title;
            template.Siteid=_admin.CurrentSite;
            template.Type = Convert.ToInt32(type);
            template.FileName=DateTime.Now.Year.ToString()+"-"+DateTime.Now.Month.ToString()+"-"+DateTime.Now.Day.ToString()+"--"+DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString()+DateTime.Now.Second.ToString()+".htm";
           template.Source="/sites/"+site.Location+"/templates/"+template.FileName;
           SiteGroupCms.Utils.DirFile.SaveFile(temcontent,template.Source);
            if (templatedal.Addtemplate(template) )
            {
                _response = "{\"IsError\":false,\"Message\":\"添加成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(10);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"添加失败\",\"Data\":0}";
        }
        public void updatetemplate()
        { 
            template.Id=Str2Int(id);
            template.Title = title;
            template.Siteid=_admin.CurrentSite;
            template.Type = Convert.ToInt32(type);
            template.FileName = filename;
            template.Source = source;
            SiteGroupCms.Utils.DirFile.SaveFile(temcontent, template.Source);
           if (templatedal.updatetemplate(template))
           {
               _response = "{\"IsError\":false,\"Message\":\"修改成功\",\"Data\":0}";
               new SiteGroupCms.Dal.LogDal().SaveLog(11);
           }
           else
               _response = "{\"IsError\":true,\"Message\":\"修改失败\",\"Data\":0}";
        }
        public void deletetemplate()
        { 
        
        }
    }
}
