using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    public partial class guestdo : SiteGroupCms.Ui.AdminCenter
    {

        SiteGroupCms.Entity.Guest guest = new SiteGroupCms.Entity.Guest();
        SiteGroupCms.Dal.GuestDal guestobj = new SiteGroupCms.Dal.GuestDal();

        string username = string.Empty;
        DateTime addtime;
        string title = string.Empty;
        string content = string.Empty;
        DateTime retime;
        string recontent = string.Empty;
        string _response = "";
        string method = "";
        string audit = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                method = Request.QueryString["method"];
                switch (method)
                {
                    case "update":
                        updateguest();
                        break;
                }
                Response.Write(_response);
            }

        }
        public void updateguest()
        {

            SiteGroupCms.Entity.Guest guest = new SiteGroupCms.Entity.Guest();
            SiteGroupCms.Dal.GuestDal guestdal = new Dal.GuestDal();
            SiteGroupCms.Entity.Repost repost = new Entity.Repost();
            SiteGroupCms.Dal.RepostDal repostdal = new Dal.RepostDal();
            guest.Id = Str2Int(Request.Form["guestid"]);
            guest.Username = Request.Form["username"];
            guest.Addtime = Validator.StrToDate(Request.Form["addtime"], DateTime.Now);
            guest.Title = Request.Form["title"];
            guest.Content = Request.Form["content"];
            guest.Audit = Request.Form["audit"] == "true" ? 1 : 0;
            int res1 = guestdal.updateguest(guest);
            int res2 = 0;
            repost.Followid = Str2Int(Request.Form["guestid"]);
            List<SiteGroupCms.Entity.Repost> lists = repostdal.GetList(repost.Followid);
            if (lists == null)//添加
            {
                repost.Content = Request.Form["recontent"];
                repost.Addtime = Validator.StrToDate(Request.Form["retime"], DateTime.Now);
                res2 = repostdal.Insertentity(repost);
            }
            else //修改或者删除
            {
                repost.Content = Request.Form["recontent"];
                if (repost.Content != "") //修改
                {
                    repost.Addtime = Validator.StrToDate(Request.Form["retime"], DateTime.Now);
                    res2 = repostdal.Updateentity(repost);
                    new SiteGroupCms.Dal.LogDal().SaveLog(34);
                }
                else
                {
                    res2 = repostdal.deletebyfollowid(repost);
                }
            }
            if ((res1 + res2) > 0)
            {
                _response = "{\"IsError\":false,\"Message\":\"保存成功\",\"Data\":0}";
                new SiteGroupCms.Dal.LogDal().SaveLog(22);
            }
            else
                _response = "{\"IsError\":true,\"Message\":\"保存失败\",\"Data\":0}";

        }
    }
}