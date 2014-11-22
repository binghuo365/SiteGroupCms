<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SiteGroupCms._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>SiteGroupCms管理系统</title>
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <script src="lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <link href="lib/css/index.css" rel="stylesheet" type="text/css" />
    <link href="lib/css/menu.css" rel="stylesheet"type="text/css" />
    <script src="lib/js/LG.js" type="text/javascript"></script>
    <link href="lib/css/common.css" rel="stylesheet" type="text/css" />  

    <script type="text/javascript">
        //几个布局的对象
        var layout, tab, accordion, tree;
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
            if (!tabid) {
                tabidcounter++;
                tabid = "tabid" + tabidcounter;
            }

            if (tab.isTabItemExist(tabid)) {
                tab.removeTabItem(tabid);
                tab.addTabItem({ tabid: tabid, text: text, url: url });
            }
            else
                tab.addTabItem({ tabid: tabid, text: text, url: url });


        }

        $(function () {
            //布局
            $("#mainbody").ligerLayout({ leftWidth: 190, height: '100%', heightDiff: -20, space: 4, onHeightChanged: f_heightChanged });
            var height = $(".l-layout-center").height();
            //Tab
            $("#framecenter").ligerTab({ height: height });
            //面板
            $("#mainmenu").ligerAccordion({ height: height - 24, speed: null });
            //绑定菜单事件
            $("ul.menulist li").hover(function () {
                $(this).addClass("over");
            }, function () {
                $(this).removeClass("over");
            });
            $("#jsddm li ul li").click(function () {
                var tabid = $(this).attr("menuno");
                f_addTab(tabid, $(this).attr("title"), $(this).attr("url"));
            });
            //绑定树菜单
            $("#tree1").ligerTree({
                url: 'ajaxhandler/loadcatalogtree.aspx',
                checkbox: false,
                slide: false,
                childIcon: 'folder',
                nodeWidth: 100,
                attribute: ['nodename', 'url'],
                onSelect: function (node) {

                    f_addTab("栏目", node.data.text, node.data.url);
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
        var actionNodeID; //所选节点的text
        var actionNodeid; //所选节点的id
        var selectnode; //所选节点

        function itemclick(item, i) {
            if (actionNodeID == "共享文档库") {
                alert("共享库不支持");
                return;
            }
            switch (item.text) {
                case "采集文章":
                    f_addTab(actionNodeID + "-采集文章", actionNodeID + "-采集文章", "articleEidt.aspx?method=add&catalogid=" + actionNodeid);
                    break;
                case "预览栏目":
                    viewcatalog(actionNodeid);
                    break;
                case "栏目属性":
                    if (actionNodeid.toString().indexOf("site") >= 0)
                        return;
                    f_addTab(actionNodeID + "-栏目属性", actionNodeID + "-栏目属性", "catalogedit.aspx?method=update&id=" + actionNodeid);
                    break;
                case "发布栏目":
                    jQuery.ligerDialog.confirm('确定发布吗?', function (confirm) {
                        if (confirm)
                            publishcatalog(actionNodeid);
                    });
                    //  alert("发布栏目"+actionNodeID + " | " + item.text);

                    break;
                case "新增栏目":
                    f_addTab("新增栏目", "新增栏目", "catalogedit.aspx?method=add&fatherid=" + actionNodeid);
                    break;
                case "删除栏目":
                    jQuery.ligerDialog.confirm('确定彻底删除吗?', function (confirm) {
                        if (confirm)
                            f_delete(actionNodeid);
                    });
                    break;
                case "全站更新":
                    jQuery.ligerDialog.confirm('确定更新全站吗?这将花费较长时间', function (confirm) {
                        if (confirm)
                            publishall();
                    });
                    break;
            }

        }
        $(function () {
            menu = $.ligerMenu({ top: 100, left: 100, width: 120, items:
            [
            { text: '采集文章', click: itemclick, id: '1' },
            { text: '预览栏目', click: itemclick, id: '100' },
            { text: '发布栏目', click: itemclick, id: '4' },
            { text: '栏目属性', click: itemclick, id: '5' },
            { text: '新增栏目', click: itemclick, id: '6' },
            { text: '全站更新', click: itemclick, id: '8' },
            { text: '删除栏目', click: itemclick, id: '7' }
            ]
            });
            $("#tree1").ligerTree({ onContextmenu: function (node, e) {
                actionNodeID = node.data.text;
                actionNodeid = node.data.id;
                selectnode = node;
                menu.show({ top: e.pageY, left: e.pageX });
                return false;
            }
            });

            $.ajax({
                data: null,
                type: 'post',
                dataType: 'html',
                url: 'ajaxhandler/loadright.aspx',
                success: function (result) {
                    if (!result) {
                        return;
                    }
                    else {
                        if (result.toString().indexOf("1") < 0)
                            menu.removeItem("1");
                        if (result.toString().indexOf("4") < 0) {
                            menu.removeItem("4");
                            menu.removeItem("8");
                        }
                        if (result.toString().indexOf("5") < 0) {
                            menu.removeItem("5");
                            menu.removeItem("6");
                            menu.removeItem("7");
                        }
                    }
                }
            }); //success end
        }); // ajax end

        function chkLogout() {
            $.ajax({
                type: "post",
                dataType: "json",
                url: "ajaxhandler/complex.aspx",
                data: [{ name: 'oper', value: 'loginout'}],
                error: function (XmlHttpRequest, textStatus, errorThrown) { alert(XmlHttpRequest.responseText); },
                success: function (d) {
                    if (eval(d).result == "1")
                        top.location.href = 'login.htm';
                }
            });
        }



        function f_delete(a) {
            if (a.toString().indexOf('site') < 0) {//删除的不是站点
                $.ajax({
                    type: "get",
                    dataType: "json",
                    url: "ajaxhandler/catalogeditdo.aspx?method=delete&id=" + a,
                    error: function (XmlHttpRequest, textStatus, errorThrown) { LG.showError(XmlHttpRequest.responseText); },
                    success: function (d) {
                        if (d.IsError == false)//删除成功
                        {
                            LG.showSuccess('删除成功');
                            //更新树形控件  移除掉所选的节点
                            tree.remove(selectnode.target);
                        }
                        else
                            LG.showError(d.Message);
                    } //end success
                }); //end ajax
            } //end if
            else {
                LG.showError("不可删除");
            }
        }


        $(function () {
            $('#changesites').change(function () {
                $.ajax({
                    type: "post",
                    dataType: "json",
                    url: "ajaxhandler/complex.aspx",
                    data: [{ name: 'oper', value: 'changesite' },
        { name: 'siteid', value: $('#changesites option:selected').attr("value") }
        ],
                    error: function (XmlHttpRequest, textStatus, errorThrown) { alert(XmlHttpRequest.responseText); },
                    success: function (d) {
                        top.location.href = 'default.aspx';
                    }
                });
            });

        });



        function publishcatalog(a) {
            var type = "";
            if (a.toString().indexOf("site") >= 0)//是站点
                type = "site";
            else
                type = "catalog";

            LG.showLoading("发布中...文章较多时,需要一点时间");
            $.ajax({
                type: "post",
                dataType: "json",
                url: "ajaxhandler/publishajax.aspx?type=" + type,
                data: { catalogid: a },
                success: function (d) {
                    if (!d.IsError) {
                        LG.showSuccess('发布文章成功');
                        LG.hideLoading();
                    }
                },
                error: function (message) {
                    LG.showError(message);

                }
            });



        }


        function publishall() {

            LG.showLoading("发布中...文章较多时,需要一点时间");
            //  $("#pb1").fadeIn();

            $.ajax({
                type: "post",
                dataType: "json",
                url: "ajaxhandler/publishajax.aspx?type=allsite",
                data: { catalogid: 0 },
                success: function (d) {
                    if (!d.IsError) {
                        LG.showSuccess('更新成功');
                        LG.hideLoading();
                    }
                },
                error: function (message) {
                    LG.showError(message);

                }

            });

            /* var i = setInterval(function () {
            $.ajax({
            type: "post",
            dataType: "json",
            data: [
            { name: 'oper', value: 'getprocess' }
            ],
            url: "ajaxhandler/complex.aspx",
            success: function (data) {
            var percentage = Math.floor(100 * parseInt(data.bytes_uploaded) / parseInt(data.bytes_total));
            $("#pb1").progressBar(percentage);
            if(data.isend=="True")
            clearInterval(i);
            }
            });
            }, 1500);
            */




        }



        function viewcatalog(a) {

            var type = "";
            if (a.toString().indexOf("site") >= 0)//是站点
                type = "viewsite";
            else
                type = "viewcatalog";

            LG.showLoading("操作中...");
            $.ajax({
                type: "post",
                dataType: "json",
                url: "ajaxhandler/publishajax.aspx?type=" + type,
                data: { catalogid: a },
                success: function (d) {
                    if (!d.IsError) {
                        LG.hideLoading();
                        window.open("sites/" + d.art + "/pub/temp.htm");
                    }
                },
                error: function (message) {
                    LG.showError(message);
                }
            });

        }

            
    </script>
    
    
<script type="text/javascript">
    //下拉菜单的js
    var timeout = 0;
    var closetimer = 0;
    var ddmenuitem = 0;

    function jsddm_open() {
        jsddm_canceltimer();
        jsddm_close();
        ddmenuitem = $(this).find('ul').eq(0).css('visibility', 'visible');
    }

    function jsddm_close()
    { if (ddmenuitem) ddmenuitem.css('visibility', 'hidden'); }

    function jsddm_timer()
    { closetimer = window.setTimeout(jsddm_close, timeout); }

    function jsddm_canceltimer() {
        if (closetimer) {
            window.clearTimeout(closetimer);
            closetimer = null;
        } 
    }

    $(document).ready(function () {
        $('#jsddm > li').bind('mouseover', jsddm_open);
        $('#jsddm > li').bind('mouseout', jsddm_timer);
    });

    document.onclick = jsddm_close;
  </script>
  

  



</head>
<body>
    <form id="form1" runat="server">
    <div id="pageloading">
    </div>
    <div id="topmenu" class="l-topmenu">
        <div class="l-topmenu-logo">
            SiteGroupCms管理后台
            </div>
            
           <div class="l-topmenu-menu">
           <ul id="jsddm">
	<li><a href="javascript:void(0)">内容管理</a>
		<ul>
			 <li url="articlelist.aspx?type=notdel"  title="检索中心" menuno="articlelist">
              <a href="javascript:void()">检索中心</a>
                </li>
			 <li url="articlelist.aspx?type=del&MenuNo=huishouzhan"  title="回收站" id="q3" runat="server" visible="false">
			 <a href="javascript:void()">回收站</a>
			 </li>
			 <li url="articlelist.aspx?type=mypass&MenuNo=shenh"  title="我的审核" id="q2" runat="server" visible="false">
			  <a href="javascript:void()">我的审核</a>
			 </li>
			 <li url="articlelist.aspx?type=publish&MenuNo=publish"  title="发布管理" id="q4" runat="server" visible="false">
			 <a href="javascript:void()">发布管理</a>
			 </li>
			 <!-- <li url="http://www.sina.com/e"  title="录入统计">
			  <a href="javascript:void()">录入统计</a>
			  </li>
			  -->
		</ul>
	</li>
	<li id="q6" runat="server" visible="false"><a href="javascript:void(0)">模板管理</a>
		<ul>
		  <li url="templatelist.aspx"  title="模板列表" menuno="templatelist">
		  <a href="javascript:void()">模板列表</a>
		  </li>
			  <li url="templateEdit.aspx?method=add"  title="添加模板">
		  <a href="javascript:void()">添加模板</a>
		  </li>
		</ul>
	</li>
	<li><a href="javascript:void(0)">用户中心</a>
		<ul>
		
		<li url="messagelist.aspx"  title="我的消息">
		<a href="javascript:void()">我的消息</a>
		</li>
		<li url="personinfo.aspx?method=update"  title="我的信息">
		<a href="javascript:void()">我的信息</a>
		</li>
		<li url="userlist.aspx"  title="用户管理" id="q7" runat="server" menuno="userlist" visible="false">
		<a href="javascript:void()">用户管理</a>
		</li>
		<li url="rolelist.aspx"  title="角色管理" id="q71"  runat="server" menuno="rolelist" visible="false">
		<a href="javascript:void()">角色管理</a>
		</li>
		</ul>
	</li>
    <li id="q10" runat="server" visible="false">
    <a href="javascript:void(0)">留言管理</a>
     <ul>
     <li url="guestlist.aspx"  title="留言列表" menuno="guestlist"><a href="javascript:void()">留言列表</a></li>
   </ul>
    </li>
	<li id="q8" runat="server" visible="false"><a href="javascript:void(0)">站务管理</a>
		<ul>
            <li url="loglist.aspx"  title="日志管理">
            <a href="javascript:void()">日志管理</a>
            </li>  
		<li url="messagelist.aspx?MenuNo=messagelistsuper"  title="公告管理" menuno="messagelist">
		<a href="javascript:void()">公告管理</a>
		</li>
		<li url="siteedit.aspx?method=update"  title="站点配置">
		<a href="javascript:void()">站点配置</a>
		</li>
		<li url="departlist.aspx"  title="部门管理" menuno="deptlist"><a href="javascript:void()">部门管理</a></li>
		   <li url="fs/default.aspx"  title="文件管理">
		   <a href="javascript:void()">文件管理</a>
		   </li>
		</ul>
	</li>
	<!--
<li id="q9" runat="server" visible="false"><a href="javascript:void(0)">站群管理</a>
		<ul>
			  <li url="sitelist.aspx"  title="站点列表" menuno="sitelist"><a href="javascript:void()">站点列表</a></li>
			  <li url="ftpedit.aspx"  title="ftp配置"><a href="javascript:void()">FTP配置</a></li>
			 <li url="sitegrouptj.aspx"  title="站群统计"><a href="javascript:void()">站群统计</a></li>
			<li url="ArticleList.aspx?type=share&MenuNo=share"  title="共享文件管理"><a href="javascript:void()">共享管理</a></li>
			
		</ul>
	</li>
	-->
</ul>
           </div> 
            
            
  <div class="l-topmenu-welcome">
   <a href="<%=weburl %>" target="_blank">浏览前台</a>
 <a href="javascript:chkLogout();" class="l-link2">退出</a>
        </div>
    </div>
    <div id="mainbody" style="width: 99.2%; margin: 0 auto; margin-top: 4px;">
        <div position="left" title="频道列表" id="mainmenu">
                <ul id="tree1" style="margin-top: 3px;">
                </ul>
        </div>
        

        
        <div position="center" id="framecenter">
            <div tabid="home" title="我的主页" style="height: 300px">
                <iframe frameborder="0" name="home" id="home" src="welcome.aspx"></iframe>
            </div>
        </div>
    </div>
    <div style="height:20px; line-height: 20px; text-align: center;">
      <ul>
     <!-- <li>
     	<div id="container">
		<span class="progressBar" id="pb1">0%</span>
		<a  onclick="beginUpload();">20</a> 
	<iframe style="display: none;" name="progressFrame"></iframe>
    </div>
    </li>-->
      <li> 长沙理工大学 </li>
     
      </ul>
       
    </div>

    <div style="display: none">
    </div>
    </form>
</body>
</html>
