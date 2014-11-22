using System;
using System.Data;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 下拉控件相关的加载控制层
    /// </summary>
    public partial class select :SiteGroupCms.Ui.AdminCenter
    {
        protected string view = "";
        protected string idfield = "";
        protected string textfield = "";
        protected string type = "";
        DataTable dt = new DataTable();
        string _response = "";
        SiteGroupCms.Dal.CatalogDal catadal = new SiteGroupCms.Dal.CatalogDal();
        SiteGroupCms.Entity.Catalog cata = new SiteGroupCms.Entity.Catalog();
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            view = Request.QueryString["view"];
            idfield=Request.QueryString["idfield"];
            textfield=Request.QueryString["textfield"];
            type=Request.QueryString["type"];
            if (!IsPostBack)
            {
                Admin_Load("", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);
            }
            loadlist();
            
        }
        protected void loadlist()
        {
            switch (view)
            { 
                case "catalogtree"://加载树形栏目目录
                    SiteGroupCms.Entity.Catalogtree tree = new SiteGroupCms.Dal.CatalogDal().GetClassTree(_admin.CurrentSite.ToString(), "0", true);
                    Response.Write("[" + SiteGroupCms.Dal.Treejson.tree2json2(tree, true) + "]");
                    break;
                case "cataloglist"://加载列表型栏目目录
                    SiteGroupCms.Dal.CatalogDal catalogda = new SiteGroupCms.Dal.CatalogDal();
                    dt = catalogda.GetDT("siteid="+_admin.CurrentSite,"sort asc");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (_admin.Catalogid == "" || _admin.Catalogid.IndexOf(dt.Rows[i]["id"].ToString() + ",") >= 0)//超级管理或者包含该栏目则显示
                        {
                            if (i < dt.Rows.Count - 1)
                                _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(dt.Rows[i]["title"].ToString()) + "},";
                            else
                                _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(dt.Rows[i]["title"].ToString()) + "}";
                        }
                    }
                    Response.Write("[" + _response + "]");
                    break;
                case "deptlist"://加载部门列表
                    SiteGroupCms.Dal.DepartDal deptdal = new SiteGroupCms.Dal.DepartDal();
                     dt = deptdal.GetDT("1=1");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if(i<dt.Rows.Count-1)
                            _response += "{\"id\":"+dt.Rows[i]["id"].ToString()+",\"text\":"+SiteGroupCms.Utils.fastJSON.JSON.WriteString(dt.Rows[i]["dept"].ToString())+"},";
                            else
                           _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(dt.Rows[i]["dept"].ToString()) + "}";
                        }
                    Response.Write("["+_response+"]");
                    break;
                case "sitelist"://加载站点列表
                    SiteGroupCms.Dal.SiteDal sitedal = new SiteGroupCms.Dal.SiteDal();
                     dt = sitedal.GetDT("1=1");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i < dt.Rows.Count - 1)
                                _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(dt.Rows[i]["title"].ToString()) + "},";
                            else
                                _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(dt.Rows[i]["title"].ToString()) + "}";
                        }
                    Response.Write("[" + _response + "]");
                    break;
                case "rolelist"://加载角色列表
                    SiteGroupCms.Dal.RoleDal roledal = new SiteGroupCms.Dal.RoleDal();
                    dt = roledal.GetDT("1=1");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (i < dt.Rows.Count - 1)
                                _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":\"" + dt.Rows[i]["role"].ToString() + "\"},";
                            else
                                _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":\"" + dt.Rows[i]["role"].ToString() + "\"}";
                        }
                    Response.Write("[" + _response + "]");
                    break;
                case "templatelist"://加载模板列表
                    loadtemplatelist(type);
                    break;
                case "favorite"://记载我的操作收藏列表
                    SiteGroupCms.Dal.FavoriteDal favdal = new SiteGroupCms.Dal.FavoriteDal();
                    dt = favdal.GetDT("1=1");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i < dt.Rows.Count - 1)
                        {
                            if(_admin.Rights.Contains(dt.Rows[i]["rightid"].ToString()))
                            _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":\"" + dt.Rows[i]["title"].ToString() + "\"},";
                        }
                           
                        else
                        {
                            if (_admin.Rights.Contains(dt.Rows[i]["rightid"].ToString()))
                          _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":\"" + dt.Rows[i]["title"].ToString() + "\"}";
                        } 
                    }
                    Response.Write("[" + _response + "]");
                    break;
            }

        }
        private void loadtemplatelist(string type)
        {
            SiteGroupCms.Dal.Normal_TemplateDAL temdal = new SiteGroupCms.Dal.Normal_TemplateDAL();

            string where = "";
            switch (type)
            { 
                case "index":
                    where = "type=1";
                    break;
                case "list":
                    where = "type=2";
                    break;
                case "content":
                    where = "type=3";
                    break;
            }
            DataTable dt = temdal.GetDT(where);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i < dt.Rows.Count - 1)
                    _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":\"" + dt.Rows[i]["title"].ToString() + "\"},";
                else
                    _response += "{\"id\":" + dt.Rows[i]["id"].ToString() + ",\"text\":\"" + dt.Rows[i]["title"].ToString() + "\"}";
            }
            Response.Write("["+_response+"]");
        }
    }
}
