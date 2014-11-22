using System;
using System.Data;
using System.Web;

namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 文章类移动控制层
    /// </summary>
    public partial class articlemovedo :SiteGroupCms.Ui.AdminCenter
    {
        string type = "";
        string copycatalogid = "";
        string ids = "";
        string _response="";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Admin_Load("1", "json");
                type = SiteGroupCms.Utils.Cookie.GetValue("type");
                copycatalogid = SiteGroupCms.Utils.Cookie.GetValue("copycatalogid");
                ids=Request.Form["ids"];
                if (type == "0")//剪贴
                {
                   if(copycatalogid!="0"&&copycatalogid.IndexOf("site")<0)
                   {
                       if (new SiteGroupCms.Dal.ArticleDal().movetoCatalog(ids, copycatalogid) > 0)
                       {
                           _response = "{\"IsError\":false,\"Message\":\"移动成功\",\"Data\":0}";
                           new SiteGroupCms.Dal.LogDal().SaveLog(29);
                       }
                       else
                           _response = "{\"IsError\":true,\"Message\":\"移动失败\",\"Data\":0}";
                   }
                       else
                       _response = "{\"IsError\":true,\"Message\":\"移动失败\",\"Data\":0}";

                }
                else if (type == "1")//复制
                {
                    if (copycatalogid != "0" && copycatalogid.IndexOf("site") < 0)
                    {
                        if (new SiteGroupCms.Dal.ArticleDal().copytocatalog(ids, copycatalogid) > 0)
                        {
                            _response = "{\"IsError\":false,\"Message\":\"移动成功\",\"Data\":0}";
                            new SiteGroupCms.Dal.LogDal().SaveLog(29);
                        }
                        else
                            _response = "{\"IsError\":true,\"Message\":\"移动失败\",\"Data\":0}";
                    }
                    else
                        _response = "{\"IsError\":true,\"Message\":\"移动失败\",\"Data\":0}";
                }
                else  //引用
                {
                    if (copycatalogid != "0" && copycatalogid.IndexOf("site") < 0)
                    {
                        if (new SiteGroupCms.Dal.ArticleDal().yiyongtocatalog(ids, copycatalogid) > 0)
                        {
                            _response = "{\"IsError\":false,\"Message\":\"引用成功\",\"Data\":0}";
                            new SiteGroupCms.Dal.LogDal().SaveLog(29);
                        }
                        else
                            _response = "{\"IsError\":true,\"Message\":\"引用失败\",\"Data\":0}";
                    }
                    else
                        _response = "{\"IsError\":true,\"Message\":\"引用失败\",\"Data\":0}";
                }


                Response.Write(_response);
            }
        }
    }
}
