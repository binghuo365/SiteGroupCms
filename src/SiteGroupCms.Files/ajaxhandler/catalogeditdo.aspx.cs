using System;
using System.Collections;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 栏目类新增、修改、删除的控制层
    /// </summary>
    public partial class catalogeditdo :SiteGroupCms.Ui.AdminCenter
    {
        SiteGroupCms.Entity.Catalog catalog = new SiteGroupCms.Entity.Catalog();
        SiteGroupCms.Dal.CatalogDal catalogdal = new SiteGroupCms.Dal.CatalogDal();
        string method = string.Empty;
        string description = string.Empty;
        string dirname = string.Empty;
        string contenttemplate = string.Empty;
        string listtemplate = string.Empty;
        string catalogid = string.Empty;
        string isshare = string.Empty;
        string keywords = string.Empty;
        string title = string.Empty;
        string _response = "";
        string id = "0";
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);

            }
            method = Request.QueryString["method"];
            description = Request.Form["description"];
            dirname = Request.Form["dirname"];
            contenttemplate = Request.Form["contenttemplate_val"];
            listtemplate = Request.Form["listtemplate_val"];
            catalogid = Request.Form["fathercatalog_val"];
            isshare = Request.Form["isshare"];
            keywords = Request.Form["keywords"];
            title = Request.Form["title"];
            id=Request.QueryString["id"];
            switch (method)
            {
                case "add":
                    addcatalog();
                    break;
                case "update":
                    updatecatalog();
                    break;
                case "delete":
                    deletecatalog();
                    break;

            }
            Response.Write(_response);
        }
        public void addcatalog()
        {

            catalog.Title = title;
            catalog.Description = description;
            catalog.Dirname = dirname;
            catalog.Father =Str2Int(catalogid);
            catalog.IsShare = isshare=="true"?1:0;
            catalog.Listtemplate =Str2Int(listtemplate);
            catalog.ContentTemplate =Str2Int(contenttemplate);
            catalog.Meta_Keywords = keywords;
            catalog.Siteid =_admin.CurrentSite;
            if (catalogdal.Exists("dirname='" + catalog.Dirname.Trim()+"'"))//如果存在相同的文件夹，则返回错误
            {
                _response = "{\"IsError\":true,\"Message\":\"栏目路径已经存在\",\"Data\":0}";
                return;
            }

            id = catalogdal.insertcatalog(catalog).ToString();
            if (id != "0")
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(30);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";
        }
        public void updatecatalog()
        {
            catalog=catalogdal.GetEntity(id);
            catalog.ID = Str2Int(id);
            catalog.Title = title;
            catalog.Description = description;
            catalog.IsShare = isshare == "true" ? 1 : 0;
            catalog.Listtemplate = Str2Int(listtemplate);
            catalog.ContentTemplate = Str2Int(contenttemplate);
            catalog.Meta_Keywords = keywords;
            if (catalogdal.update(catalog))
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(31);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";
        }
        public void deletecatalog()
        {
            int result = catalogdal.DeleteByID(id);
            if (result == 1)
            {
                _response = "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(32);
            }
            else if (result == 2)
                _response = "{\"IsError\":true,\"Message\":\"栏目含有子栏目，不可删除\",\"Data\":0}";
            else
                _response = "{\"IsError\":true,\"Message\":\"栏目含有文章，不可删除\",\"Data\":0}";
        }
    }
}
