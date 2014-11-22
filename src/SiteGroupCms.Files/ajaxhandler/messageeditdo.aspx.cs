using System;
using System.Web;
using System.Xml.Linq;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 我的消息添加控制层
    /// </summary>
    public partial class messageeditdo : SiteGroupCms.Ui.AdminCenter
    {

        string id = "";//接收query或者form传来的id
        string ids = "";//接收删除时传来的字符组
        string method = "";
        string title = "";
        string content = "";
        string _response = "";
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);
            }
            id = Request.QueryString["id"] == "" ? Request.Form["userid"] : Request.QueryString["id"];
            ids = Request.Form["ids"];
            method = Request.QueryString["method"];
            title=Request.Form["title"];
            content=Request.Form["content"];

            switch (method)
            {
                case "add":
                    addmessage();
                    break;
            }
            Response.Write(_response);
        }
        public void addmessage()
        {
            SiteGroupCms.Entity.Message message = new SiteGroupCms.Entity.Message();
            SiteGroupCms.Dal.MessageDal messagedal = new SiteGroupCms.Dal.MessageDal();
            if (messagedal.addmessage(title, content) >= 0)
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(13);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";
        
        }
    }
}
