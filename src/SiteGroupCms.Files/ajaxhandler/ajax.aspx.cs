using System;
using System.Data;
using System.Web;
using System.Collections.Generic;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
namespace SiteGroupCms.ajaxhandler
{
    /// <summary>
    /// 作为ajax后台处理文件
    /// 如加载按钮、文章类后台处理等
    /// </summary>
    public partial class ajax : SiteGroupCms.Ui.AdminCenter
    {
        string _response = "";
        string method="";
         string id="";
         string type = "";
         string menuNo="";
         string ids = "";
         string menuid = "";
         SiteGroupCms.Dal.ArticleDal articledal = new SiteGroupCms.Dal.ArticleDal();
         SiteGroupCms.Entity.Admin _admin = new SiteGroupCms.Entity.Admin();
        protected void Page_Load(object sender, EventArgs e)
        {

          if (!IsPostBack)
            {
                Admin_Load("", "json");
                _admin = ((SiteGroupCms.Entity.Admin)Session["admin"]);
            }
             type = Request.QueryString["type"];
             method=Request.QueryString["method"];
             menuNo=Request.Form["MenuNo"];
            menuid=Request.Form["menuid"];
             id=Request.Form["ID"];
            ids=Request.Form["ids"];
          
           switch(method)
           {
               case "GetMyButtons"://获取按钮一类请求
                   loadbutton(menuNo);
                break;
               case "GetArticle"://获取article一类请求
                   loadarticle(id);
                   break;
               case "delarticle"://删除
                   updatestate(method);
                       break;
               case "passarticle"://通过审核
                       updatestate(method);  
                       break;
               case "rejectarticle"://否定文章
                       updatestate(method);
                       break;
               case "sharearticle"://共享文章
                       updatestate(method);
                       break;
               case "huishou"://回收
                       updatestate(method);
                       break;
               case "cddelete"://彻底删除
                       updatestate(method);
                       break;
               case "deldepart"://删除部门
                       deldepart();
                       break;
               case "GetDepart"://取得部门信息
                       getdepart(id);
                       break;
               case "GetFtp"://取得FTp信息
                       getftp();
                       break;
               case "GetPersoninfo"://取得某人员的信息
                       getpersoninfo();
                       break;
               case "deletelog"://删除日志
                       deletelog();
                       break;
               case "qingkonglog"://清空日志
                       qingkonglog();
                       break;
               case "GetSiteinfo"://得到站点信息
                       getsiteinfo();
                       break;
               case "GetCataloginfo"://得到栏目的信息
                       getcataloginfo();
                       break;
               case "GetRoleinfo"://获取角色信息
                       getroleinfo();
                       break;
               case "Gettemplateinfo"://获取模板信息
                    gettemplateinfo();
                    break;
               case "delrole"://删除角色
                       delrole();
                       break;
               case "readnotice"://标记为已读
                       readnotice();
                       break;
               case "delnotice"://删除选择的消息
                       delnotice();
                       break;
               case "deltemplate"://删除模板
                       deltemplate();
                       break;
               case "getarticlelink"://查看连接
                       getarticlelink();
                       break;
               case "delsite"://删除站点
                       delsite();
                       break;
               case "GetCurrentUser"://获取当前用户
                       GetCurrentUser();
                       break;
               case "GetMyFavorite"://获取最近收藏
                       GetMyFavorite();
                       break;
               case "RemoveMyFavorite"://删除收藏
                       RemoveMyFavorite();
                       break;
               case "AddMyFavorite"://增加收藏
                       addmyfavorite();
                       break;
               case "getyiyongid"://获取文章的引用id
                       getyinyongid();
                       break;
               case "GetGuest": //获取留言列表
                       getguest();
                       break;
               case "delguest": //删除留言
                       delguest();
                       break;
               case "passguest"://通过留言
                       passguest();
                       break;
               case "rejectguest"://否定留言
                       rejectguest();
                       break;
        }

            
        }
        protected void loadbutton(string str)
        {
          
            if (str == "articlelist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                if(_admin.Rights.Contains("1"))
                _response += "{\"BtnID\":101,\"BtnName\":\"新增\",\"BtnNo\":\"add\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/add.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":104,\"BtnName\":\"查看\",\"BtnNo\":\"view\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_view_detail.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                if (_admin.Rights.Contains("1"))
                _response += "{\"BtnID\":102,\"BtnName\":\"修改\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                if (_admin.Rights.Contains("2"))
                {
                    _response += "{\"BtnID\":105,\"BtnName\":\"通过\",\"BtnNo\":\"pass\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_bronze_3.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                    _response += "{\"BtnID\":106,\"BtnName\":\"否定\",\"BtnNo\":\"reject\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                }
                if (_admin.Rights.Contains("1"))
                _response += "{\"BtnID\":107,\"BtnName\":\"转发\",\"BtnNo\":\"copy\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/arrow_branch.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                if (_admin.Rights.Contains("4"))
                _response += "{\"BtnID\":108,\"BtnName\":\"发布\",\"BtnNo\":\"publish\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/house_go.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                if (_admin.Rights.Contains("1"))
                _response += "{\"BtnID\":109,\"BtnName\":\"共享\",\"BtnNo\":\"share\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/weather_sun.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                if (_admin.Rights.Contains("3"))
                    _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                else   //如果没有删除功能，为了保证json数据格式 应移除最后一个，
                    _response = _response.Remove(_response.Length - 1);
                _response += "]}";
            }
            else if (str == "messagelist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":102,\"BtnName\":\"标记为已读\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}";
            }
            else if (str == "messagelistsuper")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":101,\"BtnName\":\"新增\",\"BtnNo\":\"add\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/add.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}";
            }
            else if (str == "userlist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":101,\"BtnName\":\"新增\",\"BtnNo\":\"add\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/add.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":104,\"BtnName\":\"查看\",\"BtnNo\":\"view\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_view_detail.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":102,\"BtnName\":\"修改\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":105,\"BtnName\":\"解锁\",\"BtnNo\":\"unlock\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_bronze_3.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":106,\"BtnName\":\"锁定\",\"BtnNo\":\"lock\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; 
            }
            else if (str == "huishouzhan")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":101,\"BtnName\":\"回收\",\"BtnNo\":\"huishou\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/arrow_refresh.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"彻底删除\",\"BtnNo\":\"cddelete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; 
            }
            else if (str == "shenh")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":105,\"BtnName\":\"通过\",\"BtnNo\":\"pass\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_bronze_3.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"我审核的\",\"BtnNo\":\"ipass\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_bronze_1.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; ;
            }
            else if (str == "publish")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":105,\"BtnName\":\"发布\",\"BtnNo\":\"publish\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/house_go.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"全部发布\",\"BtnNo\":\"allpublish\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_bronze_1.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; ;
            }
            else if (str == "templist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":101,\"BtnName\":\"新增\",\"BtnNo\":\"add\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/add.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":102,\"BtnName\":\"修改\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; 
            }
            else if (str == "loglist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":102,\"BtnName\":\"清空\",\"BtnNo\":\"qingkong\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/cross.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}";     
            }
            else if (str == "sitelist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":101,\"BtnName\":\"新增\",\"BtnNo\":\"add\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/add.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":105,\"BtnName\":\"启用\",\"BtnNo\":\"unlock\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_bronze_3.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":106,\"BtnName\":\"暂停\",\"BtnNo\":\"lock\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":102,\"BtnName\":\"修改\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; 
            }
            else if (str == "departlist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":101,\"BtnName\":\"新增\",\"BtnNo\":\"add\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/add.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":102,\"BtnName\":\"修改\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; 
            }
            else if (str == "rolelist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":101,\"BtnName\":\"新增\",\"BtnNo\":\"add\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/add.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":102,\"BtnName\":\"修改\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; 
            }
            else if (str == "guestlist")
            {
                _response += "{\"IsError\":false,\"Message\":null,\"Data\":[";
                _response += "{\"BtnID\":104,\"BtnName\":\"查看\",\"BtnNo\":\"view\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_view_detail.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":102,\"BtnName\":\"回复\",\"BtnNo\":\"modify\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/application_edit.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":105,\"BtnName\":\"通过\",\"BtnNo\":\"pass\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_bronze_3.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":106,\"BtnName\":\"否定\",\"BtnNo\":\"reject\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/award_star_delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null},";
                _response += "{\"BtnID\":103,\"BtnName\":\"删除\",\"BtnNo\":\"delete\",\"BtnClass\":null,\"BtnIcon\":\"lib/icons/silkicons/delete.png\",\"BtnScript\":null,\"MenuNo\":\"BaseManageSuppliers\",\"InitStatus\":null,\"SeqNo\":null}";
                _response += "]}"; 
            }
            Response.Write(_response);
        }
        protected void loadarticle(string id)
        {
            SiteGroupCms.Dal.ArticleDal artdal = new SiteGroupCms.Dal.ArticleDal();
            SiteGroupCms.Entity.Article art = new SiteGroupCms.Entity.Article();
            art = artdal.GetEntity(id);
            SiteGroupCms.Dal.ArticlepicDal artpicdal = new SiteGroupCms.Dal.ArticlepicDal();
          SiteGroupCms.Dal.ArticleattsDal artattsdal = new SiteGroupCms.Dal.ArticleattsDal();
            SiteGroupCms.Entity.Articlepic artpic = new SiteGroupCms.Entity.Articlepic();
           SiteGroupCms.Entity.Articleatts artatts = new SiteGroupCms.Entity.Articleatts();
            List<SiteGroupCms.Entity.Articlepic> piclist=artpicdal.getEntityList(id);
            List<SiteGroupCms.Entity.Articleatts> attslists = artattsdal.getEntityList(id);
            //获取imglist和imgtitlelist
            string imglist="";
            string imgtitlelist="";
            string attslist="";
            string attstitlelist="";
            for (int i = 0; i < piclist.Count; i++)
			{
			   imglist+=piclist[i].Url+",";
               imgtitlelist+=piclist[i].Title+",";
			}
            for (int i = 0; i < attslists.Count; i++)
			{
			    attslist+=attslists[i].Url+",";
                attstitlelist+=attslists[i].Title+",";
			}
            string _response = "";
            if (art != null)
            {
                string isppt = art.Isppt == 1 ? "true" : "false";
                string isrecommend = art.Isrecommend == 1 ? "true" : "false";
                string isroll = art.Isroll == 1 ? "true" : "false";
                string isshart = art.Isshare == 1 ? "true" : "false";
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{";
                _response += "\"artid\":" + art.Id + ",\"catalogid\":" + art.Catalogid + ",\"title\":"+ SiteGroupCms.Utils.fastJSON.JSON.WriteString(art.Title);
                _response += ",\"abstract\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(art.Abstract)+ ",\"addtime\":\"" + art.Addtime.ToString("yyyy-MM-dd") + "\",\"color\":\"" + art.Color + "\"";
                _response += ",\"isppt\":"+isppt+",\"isrecommend\":"+isrecommend+",\"isshare\":"+isshart+",\"   isroll\":"+isroll;
                _response += ",\"keywords\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(art.Keywords) + ",\"linkurl\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(art.Linkurl) + ",\"source\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(art.Source)+ ",\"subtitle\":" +SiteGroupCms.Utils.fastJSON.JSON.WriteString(art.Subtitle) ;
                _response += ",\"imglist\":\""+imglist+"\",\"imgtitlelist\":\""+imgtitlelist+"\"";
                _response += ",\"attslist\":\"" + attslist + "\",\"attstitlelist\":\"" + attstitlelist + "\",\"clickcount\":"+art.Clickcount;
                _response+="}}";
            }
            Response.Write(_response);
        }
        public void updatestate(string str)
        {
            bool sucess = false;
            for (int i = 0; i < id.Split(',').Length; i++)
            {
                switch (str)
                { 
                    case "delarticle" ://放入回收站和修改发布状态为未发布，并且重新生成频道标签和首页标签
                        if (articledal.huihouById(id.Split(',')[i], 1))
                        {
                            articledal.publish(id.Split(',')[i], 0);
                            SiteGroupCms.Entity.Article article = articledal.GetEntity(id.Split(',')[i]);
                            CreateCatalogFile(article.Catalogid.ToString(), true, 0);
                            CreateIndexFile(0);
                            SiteGroupCms.Dal.ModuleContentDAL modulecontent=new SiteGroupCms.Dal.ModuleContentDAL();
                            if (article.Yyarticleid == 0)//不是引用
                            { 
                              if(article.Linkurl=="")//不是连接类型
                                 SiteGroupCms.Utils.DirFile.DeleteFile(modulecontent.Go2View(article.Id.ToString()));
                            }
                           
                            sucess = true;
                        }
                         new SiteGroupCms.Dal.LogDal().SaveLog(7);
                        break;
                    case "passarticle":
                        if (articledal.PassById(id.Split(',')[i], 1,_admin.Id.ToString()))
                            sucess = true;
                        new SiteGroupCms.Dal.LogDal().SaveLog(4);
                        break;
                    case "rejectarticle":
                        if (articledal.PassById(id.Split(',')[i], 0,"0"))
                            sucess = true;
                        new SiteGroupCms.Dal.LogDal().SaveLog(26);
                        break;
                    case "sharearticle":
                        if (articledal.ShareById(id.Split(',')[i], 1))
                            sucess = true;
                        new SiteGroupCms.Dal.LogDal().SaveLog(27);
                        break;
                    case "huishou":
                        if (articledal.huihouById(id.Split(',')[i], 0))
                            sucess = true;
                        new SiteGroupCms.Dal.LogDal().SaveLog(6);
                        break;
                    case "cddelete":
                        if (articledal.DeleteByID(id.Split(',')[i]))
                            sucess = true;
                        new SiteGroupCms.Dal.LogDal().SaveLog(8);
                        break;
                }
               
            }
            if (sucess)//成功删除
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"art\":0}}";
            else
                _response += "{\"IsError\":true,\"Message\":\"加载失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        protected void deldepart()
        {

            if (new SiteGroupCms.Dal.DepartDal().DeleteByIDs(ids))
            {
                new SiteGroupCms.Dal.LogDal().SaveLog(25);
                _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
            }
            else
                _response += "{\"IsError\":true,\"Message\":\"删除失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        protected void getdepart(string id)
        {
            SiteGroupCms.Dal.DepartDal departdal = new SiteGroupCms.Dal.DepartDal();
            SiteGroupCms.Entity.Depart depart = departdal.GetEntity(id);
           _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{";
           _response += "\"deptid\":" + depart.Id + ",\"name\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(depart.Name) + ",\"description\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(depart.Description);
            _response += "}}";
           Response.Write(_response);
        }
        protected void getftp()
        {
            SiteGroupCms.Dal.SiteDal sitedal = new SiteGroupCms.Dal.SiteDal();
             SiteGroupCms.Entity.Site site= sitedal.GetBaseEntity();
            _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{";
            _response += "\"ftpserver\":\"" + site.FtpServer + "\",\"ftpport\":" + site.FtpPort + ",\"ftpuser\":\"" + site.FtpUser + "\"";
            _response += ",\"ftppwd\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.FtpPwd) + ",\"ftpdir\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.FtpDir) + "";
            _response += "}}";
            Response.Write(_response);
        
        }
        protected void getpersoninfo()
        {
            SiteGroupCms.Dal.AdminDal admindal = new SiteGroupCms.Dal.AdminDal();
            SiteGroupCms.Entity.Admin admin = new SiteGroupCms.Entity.Admin();
            SiteGroupCms.Dal.RoleDal roledal = new SiteGroupCms.Dal.RoleDal();

            if (id == "0")
                admin = admindal.GetEntity(_admin.Id.ToString());
            else
                admin = admindal.GetEntity(id);

            SiteGroupCms.Entity.Role role = roledal.GetEntity(admin.RoleId);
            if (admin != null)
            {
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"userid\":" + admin.Id + ",";
                _response += "\"username\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(admin.UserName) + ",\"truename\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(admin.TrueName) + ",";
                _response += "\"depttitle_val\":\"" + admin.DeptId + "\",\"sextitle_val\":\"" + admin.Sex + "\",\"job\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(admin.Job) + ",";
                _response += "\"email\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(admin.Email) + ",\"telphone\":\"" + admin.Telphone + "\",\"mobilephone\":\"" + admin.MobilePhone + "\",";
                _response += "\"sitetitle_val\":\"" + admin.SiteId + "\",\"roletitle_val\":\"" + admin.RoleId + "\",\"rights\":\"" + role.Righttitle + "\",";
                _response += "\"sort\":" + admin.Sort + ",\"imglist\":\"" + admin.Imgurl + "\",\"imgtitlelist\":\"" + admin.Imgurl + "\"";
                _response += "}}";
            }
            Response.Write(_response);
        }
        private void deletelog()
        {
            if (new SiteGroupCms.Dal.LogDal().DeleteLog(id))
                _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
            else
                _response += "{\"IsError\":true,\"Message\":\"删除失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
           
        
        }
        private void qingkonglog()
        {

            if (new SiteGroupCms.Dal.LogDal().DeleteLogs())
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"art\":0}}";
            else
                _response += "{\"IsError\":true,\"Message\":\"加载失败\",\"Data\":{\"art\":0}}";
          
            Response.Write(_response);
        }
        private void getsiteinfo()
        {
            SiteGroupCms.Dal.AdminDal admindal = new SiteGroupCms.Dal.AdminDal();
            SiteGroupCms.Entity.Admin admin = new SiteGroupCms.Entity.Admin();
            SiteGroupCms.Dal.RoleDal roledal = new SiteGroupCms.Dal.RoleDal();
            SiteGroupCms.Entity.Site site = new SiteGroupCms.Entity.Site();
            SiteGroupCms.Dal.SiteDal sitedal = new SiteGroupCms.Dal.SiteDal();
            if (id == "0")//站点id
                site = (SiteGroupCms.Entity.Site)Session["site"];
            else
                site = sitedal.GetEntity(Str2Int(id));

            SiteGroupCms.Entity.Role role = roledal.GetEntity(admin.RoleId);
            if (site != null)
            {
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"siteid\":" + site.ID + ",";
                _response += "\"title\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.Title) + ",\"webtitle\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.WebTitle) + ",\"iswork_val\":\""+site.IsWork+"\",";
                _response += "\"location\":\"" + site.Location + "\",\"domain\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.Domain) + ",\"keywords\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.Keyword) + ",";
                _response += "\"description\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.Description) + ",\"ftpserver\":\"" + site.FtpServer + "\",\"ftpport\":\"" + site.FtpPort + "\",";
                _response += "\"ftpuser\":\"" + site.FtpUser + "\",\"ftppwd\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.FtpPwd) + ",\"ftpdir\":\"" +site.FtpDir + "\",";
                _response += "\"indextemplate_val\":\"" + site.Indextemplate + "\",\"listtemplate_val\":\"" + site.Listtemplate + "\",\"contenttemplate_val\":\"" + site.Contenttemplate + "\",";
                _response += "\"emailserver\":\"" + site.EmailServer + "\",\"emailuser\":\"" + site.EmailUser + "\",\"emailpwd\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(site.EmailPwd) ;
                _response += "}}";
            }
            Response.Write(_response);
        }
        public void getcataloginfo()
        {
            SiteGroupCms.Dal.CatalogDal catalogdal = new SiteGroupCms.Dal.CatalogDal();
            SiteGroupCms.Entity.Catalog catalog=new SiteGroupCms.Entity.Catalog();
            if (id != "0")
                catalog = catalogdal.GetEntity(id);
            if (catalog != null)
            {
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"id\":" + catalog.ID + ",";
                _response += "\"title\":" +SiteGroupCms.Utils.fastJSON.JSON.WriteString(catalog.Title)  + ",\"fathercatalog_val\":\"" + catalog.Father + "\",";
                _response += "\"dirname\":" +SiteGroupCms.Utils.fastJSON.JSON.WriteString(catalog.Dirname ) + ",\"desctiption\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(catalog.Description) + ",\"isshare\":\"" + catalog.IsShare + "\",";
                _response += "\"listtemplate_val\":\"" + catalog.Listtemplate + "\",\"contenttemplate_val\":\"" +catalog.ContentTemplate + "\""; 
                _response += "}}";
            }
            Response.Write(_response);
        }
        public void getroleinfo()
        {
            SiteGroupCms.Dal.RoleDal roledal = new SiteGroupCms.Dal.RoleDal();
            SiteGroupCms.Entity.Role role = new SiteGroupCms.Entity.Role();
            SiteGroupCms.Dal.RightDal rightdal = new SiteGroupCms.Dal.RightDal();
            SiteGroupCms.Entity.Right right = new SiteGroupCms.Entity.Right();
            if (id != "0")
                role = roledal.GetEntity(Str2Int(id));
            if (role != null)
            {
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"id\":" + role.Id + ",";
                string[] strs=role.Rights.Split(',');
                for (int i = 0; i < strs.Length-1; i++)
                {
                    _response += "\"q"+strs[i]+"\":true,";
                }
                _response += "\"title\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(role.Title) + ",\"description\":" +SiteGroupCms.Utils.fastJSON.JSON.WriteString(role.Description);
                _response += "}}";
            }
            Response.Write(_response);
        }
        public void delrole()
        {
            int result=new SiteGroupCms.Dal.RoleDal().deleterole(id);
            if (result==2)
                _response += "{\"IsError\":true,\"Message\":\"删除失败，用户表已经使用了该角色\",\"Data\":{\"art\":0}}";
            else if (result == 0)
                _response += "{\"IsError\":true,\"Message\":\"删除失败\",\"Data\":{\"art\":0}}";
            else
            {
                new SiteGroupCms.Dal.LogDal().SaveLog(19);
                _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
            }
            Response.Write(_response);
        }
        public void readnotice()
        {
            if (new SiteGroupCms.Dal.MessageDal().updatereadbyids(ids))
                _response += "{\"IsError\":false,\"Message\":\"标记成功\",\"Data\":{\"art\":0}}";
            else
                _response += "{\"IsError\":true,\"Message\":\"标记失败\",\"Data\":{\"art\":0}}";          
            Response.Write(_response);
        }
        public void delnotice()
        {
            if (new SiteGroupCms.Dal.MessageDal().DeleteByIds(ids))
               _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
            else 
                _response += "{\"IsError\":true,\"Message\":\"删除失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        public void gettemplateinfo()
        {
            SiteGroupCms.Dal.Normal_TemplateDAL templatedal = new SiteGroupCms.Dal.Normal_TemplateDAL();
            SiteGroupCms.Entity.Normal_Template template = new SiteGroupCms.Entity.Normal_Template();
            if (id != "0")
                template = templatedal.GetEntity(id);
            if (template != null)
            {
                _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"id\":" + template.Id + ",";
                _response += "\"title\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(template.Title )+ ",\"type_val\":\"" + template.Type + "\",";
                _response += "\"source\":\"" + template.Source + "\",\"filename\":\"" +template.FileName+"\"" ;
                _response += "}}";
            }
            Response.Write(_response);
        }
        public void deltemplate()
        {
            SiteGroupCms.Dal.Normal_TemplateDAL templatedal = new SiteGroupCms.Dal.Normal_TemplateDAL();
             SiteGroupCms.Entity.Normal_Template template=new SiteGroupCms.Entity.Normal_Template();
             string[] idd = ids.Split(',');
             for (int i = 0; i < idd.Length; i++)
             {
                 template = templatedal.GetEntity(idd[i]);
                 if (SiteGroupCms.Utils.DirFile.FileExists(template.Source))
                     SiteGroupCms.Utils.DirFile.DeleteFile(template.Source);
             }
            if (templatedal.DeleteByIDs(ids))
            { 
                new SiteGroupCms.Dal.LogDal().SaveLog(12);
               _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
            }
            else
                _response += "{\"IsError\":true,\"Message\":\"删除失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        public void getarticlelink()
        {
            SiteGroupCms.Dal.Module_articleDAL module = new SiteGroupCms.Dal.Module_articleDAL();
            //判断是否为引用型
            SiteGroupCms.Entity.Article articleobj=new SiteGroupCms.Dal.ArticleDal().GetEntity(id);
            if (articleobj.Yyarticleid.ToString() == "" || articleobj.Yyarticleid.ToString() == "0")//不是引用型
            {
                if (articleobj.Ispublish != 1)//尚未发布
                    _response += "{\"IsError\":true,\"Message\":\"失败\",\"Data\":{\"art\":\"" + module.GetContentLink(id) + "\"}}";
                else
                    _response += "{\"IsError\":false,\"Message\":\"成功\",\"Data\":{\"art\":\"" + module.GetContentLink(id) + "\"}}";
            }
            else //是引用型
            {
                SiteGroupCms.Entity.Article yuanarticle =new SiteGroupCms.Dal.ArticleDal().GetEntity(articleobj.Yyarticleid.ToString());
                if (yuanarticle.Ispublish != 1)//尚未发布
                    _response += "{\"IsError\":true,\"Message\":\"失败\",\"Data\":{\"art\":\"" + module.GetContentLink(yuanarticle.Id.ToString()) + "\"}}";
                else
                    _response += "{\"IsError\":false,\"Message\":\"成功\",\"Data\":{\"art\":\"" + module.GetContentLink(yuanarticle.Id.ToString()) + "\"}}";
            }
            Response.Write(_response);
        }
        public void delsite()
        {
            if (new SiteGroupCms.Dal.SiteDal().deleteByid(id))
                _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
            else
                _response += "{\"IsError\":true,\"Message\":\"系统站点，不可删\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        public void GetCurrentUser()
        {
            _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{\"id\":" + _admin.Id + ",";
            _response += "\"title\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(_admin.UserName)+ ",\"logintime\":\"" + _admin.LastLoginTime + "\"";
            _response += "}}";
            Response.Write(_response);
        }
        public void GetMyFavorite()
        {
            SiteGroupCms.Dal.FavoriteListDal favdal = new SiteGroupCms.Dal.FavoriteListDal();
            SiteGroupCms.Dal.FavoriteDal fav = new SiteGroupCms.Dal.FavoriteDal();
            DataTable dt = favdal.GetDT("userid=" + _admin.Id);
            if (dt == null || dt.Rows.Count == 0)//插入一个
                favdal.addfavorite("1");
            dt = favdal.GetDT("userid="+_admin.Id);
            _response += "{\"IsError\":false,\"Message\":null,";
            _response += "\"Data\":[";
            if (dt.Rows.Count >= 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if(i<dt.Rows.Count-1)
                        _response += "{\"FavoriteID\":" + dt.Rows[i]["listid"].ToString() + ",\"FavoriteTitle\":\"" + dt.Rows[i]["title"].ToString() + "\",\"FavoriteAddTime\":\"" + dt.Rows[i]["addtime"].ToString() + "\",\"FavoriteContent\":\"" + dt.Rows[i]["content"].ToString() + "\",\"UserID\":" + dt.Rows[i]["id"].ToString() + ",\"Url\":\"" + dt.Rows[i]["url"].ToString() + "\",\"Icon\":\"" + dt.Rows[i]["icon"].ToString() + "\"},";
                    else
                        _response += "{\"FavoriteID\":" + dt.Rows[i]["listid"].ToString() + ",\"FavoriteTitle\":\"" + dt.Rows[i]["title"].ToString() + "\",\"FavoriteAddTime\":\"" + dt.Rows[i]["addtime"].ToString() + "\",\"FavoriteContent\":\"" + dt.Rows[i]["content"].ToString() + "\",\"UserID\":" + dt.Rows[i]["id"].ToString() + ",\"Url\":\"" + dt.Rows[i]["url"].ToString() + "\",\"Icon\":\"" + dt.Rows[i]["icon"].ToString() + "\"}";
                }
            }           
            _response +=  "]}";
            Response.Write(_response);
        }
        public void RemoveMyFavorite()
        {
            if (new SiteGroupCms.Dal.FavoriteListDal().GetDT("userid=" + _admin.Id).Rows.Count <= 1)
                _response += "{\"IsError\":true,\"Message\":\"至少保留一个快捷方式吧\",\"Data\":{\"art\":0}}";
            else
            {
                if (new SiteGroupCms.Dal.FavoriteListDal().delete(id))
                    _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
                else
                    _response += "{\"IsError\":true,\"Message\":\"至少保留一个快捷方式吧\",\"Data\":{\"art\":0}}";
            }
                Response.Write(_response);
        }
        public void addmyfavorite()
        {
            SiteGroupCms.Dal.FavoriteListDal favdal = new SiteGroupCms.Dal.FavoriteListDal();
            if (favdal.Exists("favoriteid=" + menuid + " and userid=" + _admin.Id))
            {
                _response += "{\"IsError\":true,\"Message\":\"已经存在此类快捷方式\",\"Data\":{\"art\":0}}";
            }
            else
            {
                if (favdal.addfavorite(menuid))
                    _response += "{\"IsError\":false,\"Message\":\"添加成功\",\"Data\":{\"art\":0}}";
                else
                    _response += "{\"IsError\":true,\"Message\":\"添加失败\",\"Data\":{\"art\":0}}";
            }
                Response.Write(_response);
        }
        public void getyinyongid()
        {
            SiteGroupCms.Entity.Article articleobj = new SiteGroupCms.Dal.ArticleDal().GetEntity(id);
            _response += "{\"IsError\":false,\"Message\":\"返回成功\",\"Data\":{\"yyarticleid\":"+articleobj.Yyarticleid+"}}";
            Response.Write(_response);
        }
        public void getguest()
        {
            SiteGroupCms.Entity.Guest guest = new SiteGroupCms.Dal.GuestDal().GetEntity(Convert.ToInt32(id));
          List <SiteGroupCms.Entity.Repost> repostlist=new SiteGroupCms.Dal.RepostDal().GetList(guest.Id);
            _response += "{\"IsError\":false,\"Message\":\"加载成功\",\"Data\":{";
            _response += "\"guestid\":" + guest.Id + ",\"username\":\""+guest.Username+"\",\"addtime\":\"" + guest.Addtime.ToString("yyyy-MM-dd") + "\",\"title\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(guest.Title);
            _response += ",\"userip\":\""+guest.Userip+"\",\"content\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(guest.Content) ;
           if(repostlist!=null)
               _response += " ,\"retime\":\"" + repostlist[0].Addtime.ToString("yyyy-MM-dd") + "\",\"recontent\":" + SiteGroupCms.Utils.fastJSON.JSON.WriteString(repostlist[0].Content);
           
            _response += "}}";
            Response.Write(_response);
        }

        public void delguest()
        {
            if (new SiteGroupCms.Dal.GuestDal().deletes(ids))
            {
                _response += "{\"IsError\":false,\"Message\":\"删除成功\",\"Data\":{\"art\":0}}";
                new SiteGroupCms.Dal.LogDal().SaveLog(35);
            }
            else
                _response += "{\"IsError\":true,\"Message\":\"删除失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
        public void passguest()
        {
            if (new SiteGroupCms.Dal.GuestDal().updatestatues(ids, 1))
            {
                _response += "{\"IsError\":false,\"Message\":\"审核成功\",\"Data\":{\"art\":0}}";
                new SiteGroupCms.Dal.LogDal().SaveLog(35);
            }
            else
                _response += "{\"IsError\":true,\"Message\":\"审核失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }

        public void rejectguest()
        {
            if (new SiteGroupCms.Dal.GuestDal().updatestatues(ids, 0))
            {
                _response += "{\"IsError\":false,\"Message\":\"否定成功\",\"Data\":{\"art\":0}}";
                new SiteGroupCms.Dal.LogDal().SaveLog(35);
            }
            else
                _response += "{\"IsError\":true,\"Message\":\"否定失败\",\"Data\":{\"art\":0}}";
            Response.Write(_response);
        }
    }
}
