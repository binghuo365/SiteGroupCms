<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="welcome.aspx.cs" Inherits="SiteGroupCms.welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>欢迎使用 ligerUI 权限管理系统</title> 
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />

    <script src="lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>  
    <link href="lib/css/common.css" rel="stylesheet" type="text/css" />  
    <link href="lib/css/welcome.css" rel="stylesheet" type="text/css" />

     <script src="lib/jquery-validation/jquery.validate.min.js" type="text/javascript"></script> 
    <script src="lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="lib/jquery.form.js" type="text/javascript"></script>

    <script src="lib/js/common.js" type="text/javascript"></script>
    <script src="lib/js/LG.js" type="text/javascript"></script>
    <script src="lib/js/addfavorite.js" type="text/javascript"></script> 
</head>
<body style="padding:10px; overflow:auto; text-align:center;background:#FFFFFF;"> 
        <div class="navbar">
        <div class="navbar-l"></div>
        <div class="navbar-r"></div>
        <div class="navbar-icon"><img src="lib/icons/32X32/hire_me.gif" /></div>
        <div class="navbar-inner"> 
        <b><span id="labelusername"></span><span>，</span>
        <span id="labelwelcome"></span><span>欢迎使用SiteGroupCms站群内容管理系统</span></b>
        </div>
        </div>

        <div class="withicon">
            <div class="icon"><img src="lib/images/index/time.gif" /></div>
            <span id="labelLastLoginTime"></span>
        </div>


        <div class="navline">
        </div>
        
        <div class="links"> 
        </div>

        <div class="l-clear"></div>

        <div class="button" onclick="LG.addfavorite(loadMyFavorite)">
            <div class="button-l"> </div>
            <div class="button-r"> </div>
            <div class="button-icon"> <img src="lib/ligerUI/skins/icons/add.gif" /> </div> 
            <span>增加快捷方式</span>  
        </div>

          


        <div class="navbar"><div class="navbar-l"></div><div class="navbar-r"></div>
        <div class="navbar-icon"><img src="lib/icons/32X32/collaboration.gif" /></div>
        <div class="navbar-inner"> 
        <b>文档统计</b> 
        </div>
        </div>
         <p style="margin:10px;">
         
         所有文档共<span runat="server" id="span0" style="color:Red; font-size:16px"></span>篇 </br>
         采集未审核<span runat="server" id="Span1" style="color:Red; font-size:16px"></span>篇</br>
         审核未发布<span runat="server" id="Span2" style="color:Red; font-size:16px"></span>篇 </br>
         回收站<span runat="server" id="Span3" style="color:Red; font-size:16px"></span>篇 </br>
         共享文章<span runat="server" id="Span4" style="color:Red; font-size:16px"></span>篇
         </p>
      
        <div class="navline">
        </div>

         
         <div class="navbar"><div class="navbar-l"></div><div class="navbar-r"></div>
        <div class="navbar-icon"><img src="lib/icons/32X32/communication.gif" /></div>
        <div class="navbar-inner"> 
        <b>系统使用说明</b> 
        </div>
        </div>
  <p style="margin:10px;">
      版本信息:verson sitegroupcms 1.0 稳定版;
  </p>
      
        <div class="navline">
        </div>


        
           
           <script type="text/javascript">
               $("div.link").live("mouseover", function ()
               {
                   $(this).addClass("linkover");
                    
               }).live("mouseout", function ()
               {
                   $(this).removeClass("linkover");
                    

               }).live('click', function (e)
               { 
                   var text = $("a", this).html();
                   var url = $(this).attr("url");
                   parent.f_addTab(null, text, url);
               });

               $("div.link .close").live("mouseover", function ()
               {
                   $(this).addClass("closeover");
               }).live("mouseout", function ()
               {
                   $(this).removeClass("closeover");
               }).live('click', function ()
               {
                   var id = $(this).parent().attr("id");

                   LG.ajax({
                       loading: '正在删除用户收藏中...',
                       type: 'AjaxSystem',
                       method: 'RemoveMyFavorite',
                       data: { ID: id },
                       success: function ()
                       {
                           loadMyFavorite();
                       },
                       error: function (message)
                       {
                           LG.showError(message);
                       }
                   });

                   return false;
               });


               var links = $(".links");

               

               function loadMyFavorite()
               {
                   LG.ajax({
                       loading: '正在加载用户收藏中...',
                       type: 'AjaxSystem',
                       method: 'GetMyFavorite',
                       success: function (Favorite)
                       {
                           links.html("");
                           $(Favorite).each(function (i, data)
                           {
                               var item = $('<div class="link"><img src="" /><a href="javascript:void(0)"></a><div class="close"></div></div>');
                               $("img", item).attr("src", data.Icon);
                               $("a", item).html(data.FavoriteTitle);
                               item.attr("id", data.FavoriteID);
                               //item.attr("title", data.FavoriteContent || data.FavoriteTitle);
                               item.attr("url", data.Url);
                               links.append(item);
                           });
                       },
                       error: function (message)
                       {
                           LG.showError(message);
                       }
                   }); 
               }


               function loadInfo()
               {
                   LG.ajax({
                       type: 'AjaxMemberManage',
                       method: 'GetCurrentUser',
                       success: function (user)
                       {
                           $("#labelusername").html(user.title);
                            $("#labelLastLoginTime").html("您上次的登录时间是：" +user.logintime);
                       },
                       error: function ()
                       {
                           LG.tip('用户信息加载失败');
                       }
                   });


                   var now = new Date(), hour = now.getHours();
                   if (hour > 4 && hour < 6) { $("#labelwelcome").html("凌晨好！") }
                   else if (hour < 9) { $("#labelwelcome").html("早上好！") }
                   else if (hour < 12) { $("#labelwelcome").html("上午好！") }
                   else if (hour < 14) { $("#labelwelcome").html("中午好！") }
                   else if (hour < 17) { $("#labelwelcome").html("下午好！") }
                   else if (hour < 19) { $("#labelwelcome").html("傍晚好！") }
                   else if (hour < 22) { $("#labelwelcome").html("晚上好！") }
                   else { $("#labelwelcome").html("夜深了，注意休息！") }
               }

               loadInfo();
               loadMyFavorite();
           </script>  
</body>
</html>
