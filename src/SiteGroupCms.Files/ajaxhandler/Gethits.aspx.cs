using System;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 记录文章点击量
    /// </summary>
    public partial class Gethits : SiteGroupCms.Ui.BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int count = 0;
            SiteGroupCms.Dal.ArticleDal articledal = new SiteGroupCms.Dal.ArticleDal();
            int articleid = Request.QueryString["articleid"]==null ? 0 : Convert.ToInt32(Request.QueryString["articleid"].ToString());
            if (articleid != 0)
            {
                SiteGroupCms.Entity.Article articleobj = new SiteGroupCms.Dal.ArticleDal().GetEntity(articleid.ToString());
                if (articleobj.Yyarticleid != 0)//是引用类型
                {
                    articleid = articleobj.Yyarticleid;
                    articleobj=new SiteGroupCms.Dal.ArticleDal().GetEntity(articleid.ToString());
                }
                if (articleobj.Linkurl == null || articleobj.Linkurl.ToString() == "")//普通文章
                {
                    count = articledal.gethitcount(articleid);
                    Response.Write("document.write('" + count + "');");
                }
                else  //连接文章
                {
                    count  = articledal.gethitcount(articleid);
                    Response.Redirect(articleobj.Linkurl);
                }
                
            }
            else
                Response.Write("document.write('"+count+"');");
        }
    }
}
