using System;
using System.Web;
using System.Text;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
using System.Data;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 跟发布相关的控制层
    /// </summary>
    public partial class publishajax : SiteGroupCms.Ui.AdminCenter
    {
        string _response = "";
        string ids = "";
        string type = "";
        string catalogid = "";
        SiteGroupCms.Entity.Site site = new SiteGroupCms.Entity.Site();
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = 99999;
            Admin_Load("1", "json");
            type = Request.QueryString["type"];
            ids = Request.Form["ids"];
            catalogid = Request.Form["catalogid"];
            site = (SiteGroupCms.Entity.Site)Session["site"];
            switch (type)
            {
                case "some":
                    publishsome(ids);
                    break;
                case "catalog":
                    publishcatalog(catalogid, true);
                    _response += "{\"IsError\":false,\"Message\":\"发布成功\",\"Data\":{\"art\":0}}";
                    Response.Write(_response);
                    break;
                case "site":
                    publishsite();
                    break;
                case "someall":
                    publishallcontent();
                    break;
                case "viewcatalog":
                    viewcatalog();
                    break;
                case "viewsite":
                    viewsite();
                    break;
                case "allsite":
                    // System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(allsite));
                   //  thread.Start();  
                     allsite();
                   /* SiteGroupCms.Dal.work w = new Dal.work();
                    if (w.State != 1)
                    {
                        w.runwork();
                        //Page.RegisterStartupScript("", "<script>alert('hello world')</script>");
                    }*/
              //  SiteGroupCms.Entity.Publicprocess publisprocess = (SiteGroupCms.Entity.Publicprocess)Session["publishprocess"];
               // _response = HttpContext.Current.Session["publishprocess"].ToString();
           
                        break;
                   
            }
        }


        public void publishsome(string ids)
        {
            //判断是否通过审核，通过审核则发布，否则不
            string[] artid = ids.Split(',');
            ids = "";
            SiteGroupCms.Dal.ArticleDal articleobj = new SiteGroupCms.Dal.ArticleDal();
            for (int i = 0; i < artid.Length; i++)
            {
                if (i == 0)//
                {
                    if (articleobj.ispassed(artid[i]))
                        ids += artid[i];
                }
                else
                {
                    if (articleobj.ispassed(artid[i]))
                        ids += "," + artid[i];
                }

            }

            MakeView(ids);
            //更新栏目页
            DataTable dt = articleobj.getdistinctcatalogidDT(" id in (" + ids + ")");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MakeList(dt.Rows[i]["catalogid"].ToString(),0);
                }
            }
            //更新首页
            Makeindex(0);
            new SiteGroupCms.Dal.LogDal().SaveLog(5);
            _response += "{\"IsError\":false,\"Message\":\"发布成功\",\"Data\":{\"art\":0}}";
            Response.Write(_response);

        }
        public void publishcatalog(string catalogid,bool isdepend) //isdepend=true 表示是独立发布的， isdepen=false 表示是是发布上级调用的
        {
           
            SiteGroupCms.Entity.Catalogtree catalogtree = new SiteGroupCms.Dal.CatalogDal().GetClassTree(site.ID.ToString(), catalogid, true);
            if (catalogtree.HasChild)
            {
                for (int i = 0; i < catalogtree.SubChild.Count; i++)
                {
                    publishcatalog(catalogtree.SubChild[i].Id,false);
                }
            }
           
            DataTable dt = new SiteGroupCms.Dal.ArticleDal().getDT("catalogid=" + catalogid + " and ispassed=1");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    MakeView(dt.Rows[i]["id"].ToString());
            } 
            MakeList(catalogid, 0);
        }
        public void publishsite()
        {
            Makeindex(0);
            new SiteGroupCms.Dal.LogDal().SaveLog(5);
            _response += "{\"IsError\":false,\"Message\":\"发布成功\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        public void publishallcontent()
        {
            SiteGroupCms.Dal.ArticleDal articledal = new SiteGroupCms.Dal.ArticleDal();
            DataTable dt = articledal.getDT("ispassed=1 and ispublish=0 and isdel!=1");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CreateContentFile(dt.Rows[i]["id"].ToString());
                    MakeList(dt.Rows[i]["catalogid"].ToString(), 0);
                }
            }
            Makeindex(0);
            new SiteGroupCms.Dal.LogDal().SaveLog(5);
            _response += "{\"IsError\":false,\"Message\":\"发布成功\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        public void viewsite()
        {
            Makeindex(1);
            _response += "{\"IsError\":false,\"Message\":\"发布成功\",\"art\":\"" + site.Location + "\"}";
            Response.Write(_response);

        }
        public void viewcatalog()
        {
            MakeList(catalogid, 1);
            _response += "{\"IsError\":false,\"Message\":\"发布成功\",\"art\":\"" + site.Location + "\"}";
            Response.Write(_response);

        }
        public void allsite()
        {
            DataTable dt = new SiteGroupCms.Dal.CatalogDal().GetDT("siteid=" + site.ID,"sort asc");
            if (dt == null || dt.Rows.Count == 0) return;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                publishcatalog(dt.Rows[i]["id"].ToString(),false);
            }
            Makeindex(0);
            _response += "{\"IsError\":false,\"Message\":\"发布成功\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }

        /// <summary>
        /// 生成内容页
        /// </summary>
        private void MakeView(string ids)
        {
            string[] idss = ids.Split(',');
            for (int i = 0; i < idss.Length; i++)
            {
                CreateContentFile(idss[i]);
               
            }
        }

        private void MakeList(string catalogid, int istemp)
        {
            CreateCatalogFile(catalogid, true, istemp);
            
        }

        private void Makeindex(int istemp)
        {
            CreateIndexFile(istemp);
           
        }

    }
}
