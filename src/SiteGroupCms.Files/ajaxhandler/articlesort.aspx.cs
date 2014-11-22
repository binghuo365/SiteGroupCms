using System;
using System.Data;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 文章类排序的控制层
    /// </summary>
    public partial class articlesort : SiteGroupCms.Ui.AdminCenter
    {
        string type = "";
        string id = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                type = Request.Form["upordown"];
                id=Request.Form["ids"];
                if (id != "0")
                {
                    if (type == "1")//向上移动
                        new SiteGroupCms.Dal.ArticleDal().addsort(id);
                    else if (type == "0")//向下移动
                        new SiteGroupCms.Dal.ArticleDal().missort(id);
                    else  //置顶
                      new  SiteGroupCms.Dal.ArticleDal().topsort(id);
                    string  _response = "{\"IsError\":false,\"Message\":\"移动成功\",\"Data\":0}";
                    Response.Write(_response);
                }
            }
        }
    }
}
