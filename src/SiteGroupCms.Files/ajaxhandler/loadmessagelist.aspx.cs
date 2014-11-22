using System;
using System.Web;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 我的消息加载控制层
    /// </summary>
    public partial class loadmessagelist :SiteGroupCms.Ui.AdminCenter
    {
        string type = string.Empty;
        int currentpage = 1;
        int pagesize = 1;
        string sortname = "id";
        string sortorder = "desc";
        string where = "";
        string nid = "";
        string _response = String.Empty;
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        SiteGroupCms.Entity.Message Message = new SiteGroupCms.Entity.Message();
        SiteGroupCms.Dal.MessageDal Messagedal=new SiteGroupCms.Dal.MessageDal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
            }
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
                type = Request.QueryString["type"];
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                nid = Request.QueryString["id"];
            if (Request.Form["sortname"] != null && Request.Form["sortname"] != "state")
                sortname = Request.Form["sortname"];
            if (Request.Form["sortorder"] != null && Request.Form["sortorder"] != "")
                sortorder = Request.Form["sortorder"];
            if (Request.Form["page"] != null && Request.Form["page"] != "")
                currentpage = Validator.StrToInt(Request.Form["page"], 1);
            if (Request.Form["pagesize"] != null && Request.Form["pagesize"] != "")
                pagesize = Validator.StrToInt(Request.Form["pagesize"], 1);
            where = Request.Form["where"];
            switch (type)
            { 
                case "list":
                    Messagedal.GetListJSON(currentpage, pagesize, "userid="+_admin.Id, ref _response, sortname, sortorder);
                    Response.Write(_response);
                    break;
                case "detail":
                   Message= Messagedal.GetEntity(nid);
                   Messagedal.updateread(nid,1);
                   _response += "{";
                   _response += "\"Total\":1";
                   _response += ",\"Rows\":[{";
                   _response += "\"content\":\"" + Message.Content;
                    _response += "\"}]}";
                    Response.Write(_response);
                    break;
            }

        }
    }
}
