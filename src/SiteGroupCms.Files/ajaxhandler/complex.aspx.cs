using System;
using System.Web;
using SiteGroupCms.Utils;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 登陆页面对应的控制层
    /// 包括登陆验证、权限验证等
    /// </summary>
    public partial class complex : SiteGroupCms.Ui.AdminCenter
    {
        private string _operType = string.Empty;
        private string _response = string.Empty;
        private string _siteid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            this._operType = f("oper");
           this._siteid = f("siteid");
            switch (this._operType)
            {
                
                case "login":
                    Login();
                    break;
                case "logout":
                    Logout();
                    break;
                case "chkadminpower":
                    ChkAdminPower();
                    break;
                case "ajaxChinese2Pinyin":
                    ajaxChinese2Pinyin();
                    break;
                case "changesite":
                    changesite();
                    break;
                default:
                    DefaultResponse();
                    break;
            }
            Response.Write(this._response);
        }
        private void DefaultResponse()
        {
            Admin_Load("", "json");
            this._response = JsonResult(1, "成功登录");
        }
        private void Login()
        {

            string _name = f("name");
            string _yanzheng = f("yanzhengma");
            if (_yanzheng != Session["ValidateCode"].ToString())
            {
                this._response = JsonResult(0, "验证码错误");
                return;
            }
            string _pass = SiteGroupCms.Utils.Strings.Left(f("pass"), 16);
            string _loginInfo = new SiteGroupCms.Dal.AdminDal().ChkAdminLogin(_name, _pass);
            if (_loginInfo == "ok")
            {
                this._response = JsonResult(1, "登陆成功");
                new SiteGroupCms.Dal.LogDal().SaveLog(2);//写入日志
            }
            else
            {
                this._response = JsonResult(0, _loginInfo);
                new SiteGroupCms.Dal.LogDal().SaveLog(0,9);//写入日志
            }
        }
        private void Logout()
        {
            new SiteGroupCms.Dal.AdminDal().ChkAdminLogout();
            //文件系统推出  
            if (Session["UserSpaceCapacity"]!=null)
              Session["UserSpaceCapacity"] = null;
            //其次，调用文件管理模块提供的API接口，用以清空本次登录所保存的用户空间信息；
            FSManager.Components.API.Logout();
            this._response = JsonResult(1, "成功退出");
        }
        /// <summary>
        /// 检查权限
        /// </summary>
        private void ChkAdminPower()
        {
            Admin_Load(q("power"), "json");
            this._response = JsonResult(1, "身份合法");
        }
        /// <summary>
        /// 将中文装换成拼音
        /// </summary>
        private void ajaxChinese2Pinyin()
        {
            Admin_Load("", "json");
            int t = Str2Int(f("t"), 0);
            if (t == 1)
                this._response = JsonResult(1, SiteGroupCms.Utils.ChineseSpell.MakeSpellCode(f("chinese"), "", SpellOptions.TranslateUnknowWordToInterrogation));
            else
                this._response = JsonResult(1, SiteGroupCms.Utils.ChineseSpell.MakeSpellCode(f("chinese"), "", SpellOptions.FirstLetterOnly));
        }
        /// <summary>
        ///转换当前站点
        /// </summary>
        private void changesite()  
        {

            SiteGroupCms.Entity.Admin _admin = (SiteGroupCms.Entity.Admin)Session["admin"];
            _admin.CurrentSite =Convert.ToInt32(_siteid);
            Session["admin"] = _admin;
            this._response = JsonResult(1, "切换成功");
        }
       
    }
}
