using System;
using System.Web;
using SiteGroupCms.Utils;


namespace SiteGroupCms.ajaxhandler
{
 /// <summary>
 /// 加载文章列表的控制层
 /// </summary>
  
    public partial class loadarticlelist :SiteGroupCms.Ui.AdminCenter
    {
        string type = String.Empty;
        int currentpage = 1;
        int pagesize = 1;
        //排序方式
        string orderstr = " ispassed asc,ispublish asc, catalogid asc, sort desc,addtime desc ";
        string catalogid = "2";
        string where = "1=1";
        string paras = "";//用于分辨加载ipass和ipublish
        SiteGroupCms.Dal.ArticleDal artobj = new SiteGroupCms.Dal.ArticleDal();
        SiteGroupCms.Entity.Article art = new SiteGroupCms.Entity.Article();
        string _response = String.Empty;
        SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if(!IsPostBack)
            {
             Admin_Load("1", "json");
              _admin=(SiteGroupCms.Entity.Admin)Session["admin"];
                if(_admin.Catalogid=="")
              where += " and siteid=" + _admin.CurrentSite;
                else
           //   where += " and siteid=" + _admin.CurrentSite + " and (catalogid in (" + _admin.Catalogid.Remove(_admin.Catalogid.Length - 1, 1) + ") or catalogid in ( select id from yy_cataloginfo where fatherid in(" + _admin.Catalogid.Remove(_admin.Catalogid.Length - 1, 1) + ")) ";
                    where += " and siteid=" + _admin.CurrentSite;
            }
            if (Request.QueryString["type"] != null && Request.QueryString["type"] != "")
             type = Request.QueryString["type"];
            if (Request.Form["page"] != null && Request.Form["page"] != "")
            currentpage=Validator.StrToInt(Request.Form["page"],1);
            if (Request.Form["pagesize"] != null && Request.Form["pagesize"] != "")
            pagesize = Validator.StrToInt(Request.Form["pagesize"],1);
            if(Request.QueryString["catalogid"]!=null&&Request.QueryString["catalogid"]!="")
                catalogid=Request.QueryString["catalogid"];
            if (Request.Form["where"] != null)
                where = Request.Form["where"];
            if (Request.Form["paras"]!=null)
                paras=Request.Form["paras"];
             switch (type)
             { 
                 case "notdel"://加载全部文章
                     artobj.GetListJSON(currentpage, pagesize,where+ " and isdel=0", ref _response,orderstr);
                     break;
                 case "catalog"://加载某栏目的文章
                     artobj.GetListJSON(currentpage, pagesize, where + " and catalogid=" + catalogid + " and isdel=0", ref _response, orderstr);
                     break;
                 case "del"://加载回收站的文章列表
                     artobj.GetListJSON(currentpage, pagesize, where + " and isdel=1", ref _response, orderstr);
                     break;
                 case "mypass"://加载审核通过的文章列表
                     if (paras== "ipass")//加载我通过的文章列表
                         artobj.GetListJSON(currentpage, pagesize, "(" + where + ") and  passuserid=" + _admin.Id + " and isdel=0", ref _response, orderstr);
                     else  //加载未通过和我审核的文章列表
                         artobj.GetListJSON(currentpage, pagesize, "(" + where + ") and (ispassed=0 or passuserid=" + _admin.Id + ") and isdel=0", ref _response, orderstr);
                     break;
                 case "share"://加载共享的文章列表
                     artobj.GetListJSON(currentpage, pagesize, where + " and isdel=0 and isshare=1", ref _response, orderstr);
                     break;
                 case "publish"://加载已经发布的文章列表
                     artobj.GetListJSON(currentpage, pagesize, where + " and isdel=0 and ispublish=0", ref _response, orderstr);
                     break;
             }
            Response.Write(_response);
        }
    }
}
