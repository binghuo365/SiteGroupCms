using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Text;
using SiteGroupCms.Utils;

namespace SiteGroupCms.Ui
{
    /// <summary>
    /// html页使用
    /// </summary>
    public class AdminCenter : BasicPage
    {
       
        protected string AdminId = "0";
        protected string AdminName = string.Empty;
        protected string AdminRights = string.Empty;
        protected bool AdminIsLogin = false;
        public SiteGroupCms.Entity.Normal_Channel MainChannel=new SiteGroupCms.Entity.Normal_Channel();
        public string ChannelId = "0";
        public string ChannelName = string.Empty;
        public string ChannelType = "system";
        public string ChannelDir = string.Empty;
        public string ChannelItemName = string.Empty;
        public string ChannelItemUnit = string.Empty;
        public int ChannelClassDepth = 0;
        public bool ChannelIsHtml = false;
        public string ChannelUploadPath = string.Empty;
        public string ChannelUploadType = string.Empty;
        public int ChannelUploadSize = 0;
        /// <summary>
        /// 列表内容通用方法,必须重写
        /// </summary>
        protected virtual void getListBox() { }
        /// <summary>
        /// 编辑内容通用方法,必须重写
        /// </summary>
        protected virtual void editBox() { }

        /// <summary>
        /// 验证权限
        /// 超级管理员永远有效
        /// </summary>
        /// <param name="s">空时只要求登录</param>
        protected bool IsPower(string s)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
                this.AdminIsLogin = false;
            else
            {
                this.AdminIsLogin = true;
                this.AdminRights = ((SiteGroupCms.Entity.Admin)Session["admin"]).Rights;
            }
            if (s == "ok") return true;
            if (s == "") return (this.AdminIsLogin);
            else
            return (this.AdminIsLogin&&this.AdminRights.Contains(s + ","));
        }
        /// <summary>
        /// 验证权限
        /// 超级管理员永远有效
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pageType">页面分为html和json</param>
        protected void chkPower(string s, string pageType)
        {
            if (!CheckFormUrl() && pageType == "json")//不可直接在url下访问
            {
                Response.End();
            }
            if (!IsPower(s))
            {
                showErrMsg("权限不足或未登录", pageType);
            }
        }


        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="pageType">页面分为html和json</param>
        protected void showErrMsg(string msg, string pageType)
        {
            if (pageType != "json")
                FinalMessage(msg, "login.htm", 0);
            else
            {
                HttpContext.Current.Response.Clear();
                if (!this.AdminIsLogin)
                    HttpContext.Current.Response.Write(JsonResult(-1, msg));
                else
                    HttpContext.Current.Response.Write(JsonResult(0, msg));
                HttpContext.Current.Response.End();
            }
        }
        /// <summary>
        /// 管理中心初始
        /// </summary>
        /// <param name="powerNum">权限,为空表示验证是否登录</param>
        /// <param name="pageType">页面分为html和json</param>
        protected void Admin_Load(string powerNum, string pageType)
        {

            chkPower(powerNum, pageType);
        }

        /// <summary>
        /// 管理中心初始,并获得频道的各项参数值
        /// </summary>
        /// <param name="powerNum">权限</param>
        /// <param name="isChannel">如果为false就表示ChannelId可以为0</param>
        protected void Admin_Load(string powerNum, string pageType, bool isChannel)
        {
           
             chkPower(powerNum, pageType);
            if (isChannel && ChannelId == "0")
            {
                showErrMsg("参数错误,请不要在外部提交数据", pageType);
                return;
            }
            if (ChannelId != "0")
            {
                SiteGroupCms.Entity.Normal_Channel _Channel = new SiteGroupCms.Dal.Normal_ChannelDAL().GetEntity(ChannelId);
                ChannelName = _Channel.Title;
                ChannelDir = _Channel.Dirname;
                ChannelType = _Channel.Type;
                ChannelIsHtml = true;
                //去掉标签后的实际路径
                MainChannel = _Channel;
            }
            
        }
       

    }
}
