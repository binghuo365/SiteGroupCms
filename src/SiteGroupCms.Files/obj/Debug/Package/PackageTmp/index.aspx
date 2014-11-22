<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SiteGroupCms.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>站群内容管理系统</title>
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <script src="lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <link href="lib/css/common.css" rel="stylesheet" type="text/css" />
    <link href="lib/css/index.css" rel="stylesheet" type="text/css" />
    <script src="lib/js/LG.js" type="text/javascript"></script>
    <script type="text/javascript">
      //几个布局的对象
        var layout, tab, accordion,tree;
        //tabid计数器，保证tabid不会重复
        var tabidcounter = 0;
        //窗口改变时的处理函数
        function f_heightChanged(options) {
            if (tab)
                tab.addHeight(options.diff);
            if (accordion && options.middleHeight - 24 > 0)
                accordion.setHeight(options.middleHeight - 24);
        }
        //增加tab项的函数
        function f_addTab(tabid, text, url) {
            if (!tab) return;
            if (!tabid)
            {
                tabidcounter++;
                tabid = "tabid" + tabidcounter; 
            }
            tab.addTabItem({ tabid: tabid, text: text, url: url });
        }

            $(function ()
            {
                //布局
                $("#mainbody").ligerLayout({ leftWidth: 190, height: '100%',heightDiff:-50,space:4, onHeightChanged: f_heightChanged });
                var height = $(".l-layout-center").height();
                //Tab
                $("#framecenter").ligerTab({ height: height });
                //面板
                $("#mainmenu").ligerAccordion({ height: height - 24, speed: null });
                //绑定菜单事件
                $("ul.menulist li").hover(function ()
                {
                   $(this).addClass("over");
                }, function ()
                {
                    $(this).removeClass("over");
                });
                $("ul.menulist li").click(function ()
                {
                   var tabid = $(this).attr("menuno");
                  f_addTab(tabid,$(this).attr("title"),$(this).attr("url"));
                });
                //绑定树菜单
                $("#tree1").ligerTree({
                    url: 'ajaxhandler/loadcatalogtree.aspx',
                    checkbox: false,
                    slide: false,
                     childIcon:'folder',
                    nodeWidth: 100,
                    attribute: ['nodename', 'url'],
                    onSelect: function (node)
                    {
                        if (!node.data.url) return;
                        var tabid = $(node.target).attr("tabid");
                        if (!tabid)
                        {
                            tabid = new Date().getTime();
                            $(node.target).attr("tabid", tabid);
                        } 
                        f_addTab(tabid, node.data.text, node.data.url);
                    }
                });

                tab = $("#framecenter").ligerGetTabManager();
                accordion = $("#mainmenu").ligerGetAccordionManager();
                tree = $("#tree1").ligerGetTreeManager();
                $("#pageloading").hide();
            });
            //右键菜单
         var menu;
         var ajaxdata;
         var actionNodeID;//所选节点的text
         var actionNodeid;//所选节点的id
         var selectnode;//所选节点
         
          function itemclick(item, i)
        {
        if(actionNodeID=="共享文档库")
        {
           alert("共享库不支持");
           return;
        }
           switch (item.text)
           {
           case "采集文章":
             f_addTab(actionNodeID+"-采集文章",actionNodeID+"-采集文章","articleEidt.aspx?method=add&catalogid="+actionNodeid);
             break;
            case "预览栏目":
             alert("预览文档"+actionNodeID + " | " + item.text);
             break;
             case "栏目属性":
            f_addTab(actionNodeID+"-栏目属性",actionNodeID+"-栏目属性","catalogedit.aspx?method=update&id="+actionNodeid);
             break;
             case "发布栏目":
              jQuery.ligerDialog.confirm('确定发布吗?', function (confirm) {
                      if (confirm)
                         publishcatalog(actionNodeid);
                  });
           //  alert("发布栏目"+actionNodeID + " | " + item.text);
            
             break;
             case "新增栏目":
              f_addTab("新增栏目","新增栏目","catalogedit.aspx?method=add&fatherid="+actionNodeid);
             break;
             case "删除栏目":
              jQuery.ligerDialog.confirm('确定彻底删除吗?', function (confirm) {
                      if (confirm)
                          f_delete(actionNodeid);
                  });
                  break;
           }
           
        }
        $(function ()
        {
            menu = $.ligerMenu({ top: 100, left: 100, width: 120, items:
            [
            { text: '采集文章', click: itemclick,id:'1'},
            { text: '预览栏目', click: itemclick,id:'100'},      
            { text: '发布栏目', click: itemclick ,id:'4'},
            { text: '栏目属性', click: itemclick ,id:'5'},
            { text: '新增栏目', click: itemclick ,id:'6'},
            { text: '删除栏目', click: itemclick ,id:'7'}
            ]
            });
            $("#tree1").ligerTree({ onContextmenu: function (node, e)
            { 
                actionNodeID = node.data.text;
                actionNodeid=node.data.id;
                selectnode=node;
                menu.show({ top: e.pageY, left: e.pageX });
                return false;
            }
            });
            
              $.ajax({   
                    data: null,
                    type: 'post', 
                    dataType: 'html',
                    url: 'ajaxhandler/loadright.aspx',
                    success: function (result)
                    {
                        if (!result)
                        {
                            return;
                        } 
                        else
                        {
                       if(result.toString().indexOf("1")<0)
                            menu.removeItem("1");
                            if(result.toString().indexOf("4")<0)
                               menu.removeItem("4");
                            if(result.toString().indexOf("5")<0)
                               {
                                menu.removeItem("5");
                                menu.removeItem("6");
                                menu.removeItem("7");
                               }
                        }
                    }
                    });//success end
        });// ajax end
           
     function chkLogout(){
	$.ajax({
		type:		"post",
		dataType:	"json",
		url:		"ajaxhandler/complex.aspx",
		data: [{name: 'oper', value: 'loginout'}],
        error:		function(XmlHttpRequest,textStatus, errorThrown) { alert(XmlHttpRequest.responseText);},
		success:	function(d){
			if(eval(d).result=="1")
				top.location.href = 'login.htm';
		}
	});
}      



  function f_delete(a) {
          if (a.toString().indexOf('site')<0) {//删除的不是站点
        $.ajax({
		type:		"get",
		dataType:	"json",
		url:		"ajaxhandler/catalogeditdo.aspx?method=delete&id="+a,
        error:		function(XmlHttpRequest,textStatus, errorThrown) { LG.showError(XmlHttpRequest.responseText);},
		success:	function(d){
			if(d.IsError==false)//删除成功
			{
				 LG.showSuccess('删除成功');
				 //更新树形控件  移除掉所选的节点
				 tree.remove(selectnode.target);
			}
		else
				LG.showError(d.Message);
		}//end success
	});//end ajax
          }//end if
          else {
             LG.showError("不可删除");
          }
      }
     
  
  $(function()
  {
    $('#changesites').change(function(){
 $.ajax({
		type:		"post",
		dataType:	"json",
		url:		"ajaxhandler/complex.aspx",
        data: [{name: 'oper', value: 'changesite'},
        {name: 'siteid', value: $('#changesites option:selected').attr("value")}
        ],
        error:		function(XmlHttpRequest,textStatus, errorThrown) { alert(XmlHttpRequest.responseText);},
		success:	function(d){
				top.location.href = 'default.aspx';
		}
	});
                }); 
  
  });   
    


function publishcatalog(a)
{
var type="";
if(a.toString().indexOf("site")>=0)//是站点
{
type="site";
}
else//是栏目
{
type="catalog";
}


	 LG.showLoading("发布中文章较多时需要一点时间");
      $.ajax({
		type:		"post",
		dataType:	"json",
		url:		"ajaxhandler/publishajax.aspx?type="+type,
		data:		{catalogid: a},
		success:	function(d){
		if(!d.IsError)
		{
		  LG.showSuccess('发布文章成功');
		   LG.hideLoading();
                      f_reload();	
                      }
		},
		error: function (message) {
                      LG.showError(message);
                      
                      }
         });



}





            
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="pageloading">
    </div>
    <div id="topmenu" class="l-topmenu">
        <div class="l-topmenu-logo">
            SiteGroupCms 站群内容管理系统</div>
        <div class="l-topmenu-welcome">
            <span id="changesites" runat="server" visible="false">切换到：
            <select id="currentsiteid">
              <%=sites %>
            </select>
            </span><span class="space">|</span> <a onclick="chkLogout();" class="l-link2" target="_blank">退出</a>
        </div>
    </div>
    <div id="mainbody" style="width: 99.2%; margin: 0 auto; margin-top: 4px;">
        <div position="left" title="主要菜单" id="mainmenu">
            <div title="站点频道树">
                <ul id="tree1" style="margin-top: 3px;">
                </ul>
            </div>
    
            
            <div title="内容管理">
            <ul class="menulist">
            <li url="articlelist.aspx?type=notdel"  title="检索中心" menuno="articlelist">
            <img src="/lib/icons/32X32/search.gif"/><span>检索中心</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                <li url="articlelist.aspx?type=del&MenuNo=huishouzhan"  title="回收站" id="q3" runat="server" visible="false">
            <img src="/lib/icons/32X32/basket.gif"/><span>回收站</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                <li url="articlelist.aspx?type=mypass&MenuNo=shenh"  title="我的审核">
            <img src="/lib/icons/32X32/hire_me.gif"/><span>我的审核</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                <li url="articlelist.aspx?type=publish&MenuNo=publish"  title="发布管理" id="q4" runat="server" visible="false">
            <img src="/lib/icons/32X32/print.gif"/><span>发布管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                <li url="tongji.aspx"  title="录入统计">
            <img src="/lib/icons/32X32/report.gif"/><span>录入统计</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                </ul>
            </div>
            
            <div title="模板管理" id="q6" runat="server" visible="false">
            <ul class="menulist">
            <li url="templatelist.aspx"  title="模板列表" menuno="templatelist">
            <img src="/lib/icons/32X32/product_169.gif"/><span>模板列表</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
             </li>
                </ul>
            </div>
            
            <div title="用户中心">
            <ul class="menulist">
            <li url="messagelist.aspx"  title="我的消息">
            <img src="/lib/icons/32X32/email.gif"/><span>我的消息</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
             <li url="personinfo.aspx?method=update"  title="我的信息">
            <img src="/lib/icons/32X32/member.gif"/><span>我的信息</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                
              <li url="userlist.aspx"  title="用户管理" id="q7" runat="server" menuno="userlist" visible="false">
            <img src="/lib/icons/32X32/user.gif"/><span>用户管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
            <li url="rolelist.aspx"  title="角色管理" id="q71"  runat="server" menuno="rolelist" visible="false">
            <img src="/lib/icons/32X32/link.gif"/><span>角色管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                </ul>
            </div>
            
     <div title="站务管理" id="q8" runat="server" visible="false">
           <ul class="menulist">
            <li url="loglist.aspx"  title="日志管理">
            <img src="/lib/icons/32X32/statistics.gif"/><span>  日志管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
              
                </li>
              <li url="messagelist.aspx?MenuNo=messagelistsuper"  title="公告管理" menuno="messagelist">
            <img src="/lib/icons/32X32/comment.gif"/><span> 公告管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
             <li url="siteedit.aspx?method=update"  title="站点配置">
            <img src="/lib/icons/32X32/config.gif"/><span>站点配置</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                 <li url="filemanage.aspx"  title="文件管理">
            <img src="/lib/icons/32X32/category.gif"/><span>文件管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                </ul>
            </div>
            
                    <div title="站群管理" id="q9" runat="server" visible="false">
            <ul class="menulist">
                 <li url="sitelist.aspx"  title="站点列表">
            <img src="/lib/icons/32X32/sitemap.gif"/><span>  站点列表</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
              
                </li>
            <li url="ftpedit.aspx"  title="ftp配置">
            <img src="/lib/icons/32X32/communication.gif"/><span>  FTP配置</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
              
                </li>
                     <li url="sitegrouptj.aspx"  title="站群统计">
            <img src="/lib/icons/32X32/report.gif"/><span>  站群统计</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
              
                </li>
              <li url="ArticleList.aspx?type=share&MenuNo=share"  title="共享文件管理">
            <img src="/lib/icons/32X32/premium.gif"/><span> 共享文件管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
             <li url="departlist.aspx"  title="部门管理" menuno="deptlist">
            <img src="/lib/icons/32X32/bank.gif"/><span>部门管理</span>
            <div class="menuitem-l"></div><div class="menuitem-r"></div>
                </li>
                </ul>
            </div>
            
        </div>
        

        
        <div position="center" id="framecenter">
            <div tabid="home" title="我的主页" style="height: 300px">
                <iframe frameborder="0" name="home" id="home" src="welcome.aspx"></iframe>
            </div>
        </div>
    </div>
    <div style="height: 32px; line-height: 32px; text-align: center;">
       长沙理工大学
    </div>
    <div style="display: none">
    </div>
    </form>
</body>
</html>

