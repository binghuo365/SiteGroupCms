<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="member.aspx.cs" Inherits="SiteGroupCms.member" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Hello! 云作坊</title>
<meta name="keywords" content="长沙理工大学技术开发团队，长沙理工大学云作坊" />
<meta name="description" content="云作坊是归属于长沙理工大学现代技术教育中心的一个学生技术组织。先后开发了大量的web系统。" />
<link rel="stylesheet" type="text/css" href="guestimages/base.css" />
<script type="text/javascript" src="guestimages/jquery-1.5.min.js"></script>
<script>
    $(function () {
        var aImg = document.getElementById("nav").getElementsByTagName("img");
        for (i = 0; i < aImg.length; i += 2) {
            aImg[i].onmouseover = function () {
                this.src = this.src.replace("out", "hover");
            }

            aImg[i].onmouseout = function () {
                this.src = this.src.replace("hover", "out");
            }
        }
    })
</script>
<style type="text/css">
div#topdis {
	background:url(guestimages/team.jpg) no-repeat;
	width: 950px;
	height: 134px;
	margin-top:-2px;
}
.captionline {
	width: 950px;
	height: 30px;
	border-bottom: 1px #bfbfbf solid;
}
.captionline img {
	margin-top:8px;
}
.captionline .ch_zn {
	margin-left:10px;
	font-size:16px;
	font-family:"微软雅黑";
}
.captionline .en {
	margin-left:10px;
	color:#20B2AA;
	font-size:12px;
	font-family:"微软雅黑";
}
.photo {
	width:120px;
	height:120px;
	overflow:hidden;
}
.photo img {
	border: 1px solid #bfbfbf;
	background-color: #f1f1f1;
	padding: 5px;
}
.figure p {
	font: 12px Helvetica, Arial, sans-serif, 宋体;
	text-align: center;
	margin-top: 10px;
}
.figure {
	float: left;
	width: 100px;
	height: 130px;
	margin: 15px 20px 20px 10px;
}
.gallery {
	width: 950px;
}
*html .figure {
	display: inline;
}
.clear {
	clear: both;
}
#center {
	margin: 0 auto;
	clear: both;
}
#cc
{
	width:960px;
	height:auto;
	margin:0 auto;
}

</style>
</head>
<body>
<!--header-->
<div class="header">
	<div class="headCenter">
    	<!--logo-->
    	<div class="logo">					
            <a href="#">
                <img src="guestimages/logo.gif" />
            </a>
            <div>Yes, We Can!!!</div>
        </div>
        <!--end of logo-->
    <div class="searchBox">关注我们:<a href="http://weibo.com/yunstudio2012"><img src="guestimages/weibo.png" width="28" height="28" /></a>
    </div>
	   
    </div>
</div>
<div id="menu">
 <ul id="nav"><!--nav-->
    <li><a href="/sites/Demo/pub/"><img src="guestimages/out_menu_01.gif" width="95" height="35" /></a></li>
    <li><img src="guestimages/out_menu_02.gif" width="5" height="35" /></li>
    <li><a href="/sites/Demo/pub/tdwh/"><img src="guestimages/out_menu_03.gif" width="129" height="35" /></a></li>
    <li><img src="guestimages/out_menu_02.gif" width="5" height="35" /></li>
    <li><a href="/member.aspx"><img src="guestimages/out_menu_05.gif" width="132" height="35" /></a></li>
    <li><img src="guestimages/out_menu_02.gif" width="5" height="35" /></li>
    <li><a href="/sites/Demo/pub/zpzs/"><img src="guestimages/out_menu_07.gif" width="128" height="35" /></a></li>
    <li><img src="guestimages/out_menu_02.gif" width="5" height="35" /></li>
    <li><a href="/sites/Demo/pub/gjyc/"><img src="guestimages/out_menu_09.gif" width="127" height="35" /></a></li>
    <li><img src="guestimages/out_menu_02.gif" width="5" height="35" /></li>
    <li><a href="/guest.aspx"><img src="guestimages/out_menu_11.gif" width="102" height="35" /></a></li>
   </ul>
   </div>
 <!--end of header--> 

  <div id="center"> 
    <div id="cc">
      <div id="topdis"> </div>
<div class="gallery">
        <div class="captionline"><img src="guestimages/waw.png"/><span class="ch_zn">团队创始人</span><span class="en">The Team founder</span></div>
       
    <asp:Repeater ID="Repeater1" runat="server">
    <ItemTemplate>
     <div class="figure">
          <div class="photo"><a href="<%#Eval("email") %>"><img src="<%#getimg(Eval("imgurl")) %>" alt="" width="90" height="90" class="userInfo"></a> </div>
          <p> <%#Eval("truename") %></p>
        </div>
    </ItemTemplate>
    </asp:Repeater>
       


        <div class="clear"> </div>
      </div>
      
            <div class="gallery">
        <div class="captionline"><img src="guestimages/waw.png"/><span class="ch_zn">团队主管</span><span class="en">The Manager</span></div>
      
         <asp:Repeater ID="Repeater2" runat="server">
    <ItemTemplate>
    <div class="figure">
          <div class="photo"><a href="<%#Eval("email") %>"><img src="<%#getimg(Eval("imgurl")) %>" alt="" width="90" height="90" class="userInfo"></a> </div>
          <p> <%#Eval("truename") %></p>
        </div>
    </ItemTemplate>
    </asp:Repeater>

        <div class="clear"> </div>
      </div>
      <div class="gallery">
        <div class="captionline"><img src="guestimages/waw.png"/><span class="ch_zn">团队商务组</span><span class="en">The Business Members</span></div>
        
         <asp:Repeater ID="Repeater3" runat="server">
    <ItemTemplate>
   <div class="figure">
          <div class="photo"><a href="<%#Eval("email") %>"><img src="<%#getimg(Eval("imgurl")) %>" alt="" width="90" height="90" class="userInfo"></a> </div>
          <p> <%#Eval("truename") %></p>
        </div>
    </ItemTemplate>
    </asp:Repeater>
        <div class="clear"> </div>
      </div>
      <div class="gallery">
        <div class="captionline"><img src="guestimages/waw.png"/><span class="ch_zn">团队美工组</span><span class="en">The Web UI Designer</span></div>
        
        <asp:Repeater ID="Repeater4" runat="server">
    <ItemTemplate>
  <div class="figure">
          <div class="photo"><a href="<%#Eval("email") %>"><img src="<%#getimg(Eval("imgurl")) %>" alt="" width="90" height="90" class="userInfo"></a> </div>
          <p> <%#Eval("truename") %></p>
        </div>
    </ItemTemplate>
    </asp:Repeater>
        <div class="clear"> </div>
      </div>
      <div class="gallery">
        <div class="captionline"><img src="guestimages/waw.png"/><span class="ch_zn">团队程序组</span><span class="en">The Software Engineer</span></div>
        
        <asp:Repeater ID="Repeater5" runat="server">
    <ItemTemplate>
   <div class="figure">
          <div class="photo"><a href="<%#Eval("email") %>"><img src="<%#getimg(Eval("imgurl")) %>" alt="" width="90" height="90" class="userInfo"></a> </div>
          <p> <%#Eval("truename") %></p>
        </div>
    </ItemTemplate>
    </asp:Repeater>
        <div class="clear"></div>
      </div>
       <div class="gallery">
        <div class="captionline"><img src="guestimages/waw.png"/><span class="ch_zn">历届成员</span><span class="en">The Old Members </span></div>
      
       <asp:Repeater ID="Repeater6" runat="server">
    <ItemTemplate>
    <div class="figure">
          <div class="photo"><a href="<%#Eval("email") %>"><img src="<%#getimg(Eval("imgurl")) %>" alt="" width="90" height="90" class="userInfo"></a> </div>
          <p> <%#Eval("truename") %></p>
        </div>        
    </ItemTemplate>
    </asp:Repeater>
    <div class="clear"> </div>
      </div>
    </div>
  </div>

   <!--底部开始-->
 <div style="clear:both;">   
     <table class="shuangxin" width="100%" border="0"><!--联系我们-->
        <tr>
          <td width="5%" height="24">&nbsp;</td>
          <td width="22%">本实验室承接各类商业项目</td>
          <td width="22%">主管邮箱：<a href="mailto:yunstudio2012@qq.com">yunstudio2012@qq.com</a></td>
          <td width="13%">&nbsp;</td>
          <td width="7%"><a href="#">网站地图</a></td>
          <td width="2%">|</td>
          <td width="7%"><a href="newslist.php">热点新闻</a></td>
          <td width="2%"><a href="#"></a>|</td>
          <td width="7%"><a href="#">寻求帮助</a></td>
          <td width="2%"><a href="#"> </a>|</td>
          <td width="11%"><a href="#">联系我们</a></td>
        </tr>
      </table>
	<div class="footer"><!--footer-->
      <table width="100%" border="0">
        <tr>
          <td width="30%" height="39">&nbsp;</td>
          <td colspan="2" align="center"> Copyright © 2010-2012 云作坊 Inc. 保留所有权利 </td>
          <td width="7%"><!--<a href="../guest2/guest.php">批评建议</a>--></td>
          <td width="2%"><!--|--></td>
          <td width="20%"><!--<a href="../guest2/guest.php">点子提供</a>--></td>
        </tr>
        <tr align="center">
          <td colspan="6">湘ICP备10021854<span style="margin-left:12px;"><script src="http://s25.cnzz.com/stat.php?id=4368956&web_id=4368956&show=pic1" language="JavaScript"></script></span></td>
        </tr>
      </table>
    </div>

</div>
<!--底部end-->


</body>
</html>
