using System;
using System.Web;


namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 首页树形栏目生成控制层
    /// </summary>
    public partial class loadcatalogtree :SiteGroupCms.Ui.AdminCenter
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                SiteGroupCms.Entity.Admin _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
                SiteGroupCms.Entity.Catalogtree tree = new SiteGroupCms.Dal.CatalogDal().GetClassTree(_admin.CurrentSite.ToString(), "0", true);
               // if(type=="notincludeshare")
               //     Response.Write("[" + SiteGroupCms.Dal.Treejson.tree2json(tree, true) + "]");
               // else
               // Response.Write("[" + SiteGroupCms.Dal.Treejson.tree2json(tree, true) + ",{text:'共享文档库',url:'22',id:'0'}]");
                

                //以下代码解决ie中不可更新栏目的问题，不保留缓存
                Response.Buffer = true;
                Response.ExpiresAbsolute = System.DateTime.Now.AddMonths(-120);
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.AddHeader("pragma", "no=cache");
                Response.Write("[" + SiteGroupCms.Dal.Treejson.tree2json(tree, true) + "]");

            }
           
          
        }
        
    }
}
