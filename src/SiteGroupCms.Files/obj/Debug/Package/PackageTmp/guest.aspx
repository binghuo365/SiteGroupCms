<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guest.aspx.cs" Inherits="SiteGroupCms.guest" %>
<%@ Register Assembly="UcfarPager" Namespace="UcfarPagerControls" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link href="/sites/Demo/templates/atts/20130116/style/gwfn.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="/sites/Demo/templates/atts/20130116/js/jquery.js"></script>
<script type="text/javascript" src="/sites/Demo/templates/atts/20130116/js/gwfn.js"></script>
<title>现代教育技术中心欢迎您！</title>

<style type="text/css">
div#topdis {
	background:url("guestimages/guestbook.jpg") no-repeat;
	width:950px;
	height: 134px;
	margin-top:-2px;
}
.clear {
	clear: both;
}
#center {
	margin: 0 auto;
	clear: both;
}
.mabox {
    clear: both;
    margin: 0 auto;
    width: 1003px;
}
.place {
    background: url("guestimages/icon15.gif") no-repeat scroll 0 0 transparent;
    clear: both;
    color: #FFFFFF;
    height: 24px;
    line-height: 24px;
    margin: 8px 0 0 -13px;
    padding: 0 0 9px 18px;
    position: relative;
    width: 316px;
}
.place a {
    color: #FFFFFF;
}
.mleft {
    background: url("guestimages/bgs0.gif") repeat scroll 0 0 #FFFFFF;
    border: 1px solid #E3E3E3;
    float: left;
    margin: 0 auto;
    min-height: 500px;
    width: 950px;
}
.mbox {
    background: none repeat scroll 0 0 #FFFFFF;
    border: 1px solid #D5D5D5;
    clear: both;
    margin: 8px 6px 6px;
    min-height: 500px;/*限制最小高度*/
}
.ndetail {
    clear: both;
    padding: 10px 15px;
}
h1.title {
    clear: both;
    color: #06328D;
    font-size: 14px;
    line-height: 26px;
    text-align: center;
}
.ninfo {
    border-bottom: 1px dashed #DDDDDD;
    clear: both;
    color: #333333;
    font-family: Arial,Helvetica,sans-serif;
    padding-bottom: 15px;
    text-align: center;
}
.acontent {
    clear: both;
    color: #000000;
    font-size: 14px;
    line-height: 26px;
    padding: 10px;
}
.share{ height:30px;}
.captionline {
	width: 900px;
	height: 30px;
	border-bottom: 1px #bfbfbf solid;
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
.culture{
	padding-left:10px;
	font-size:13px;
}
.culture span
{
	color: #11858F;
    font-size: 14px;
    font-weight: bold;
}
</style>
<!--我添加的样式表-->
<style type="text/css">
.leaveWords{
	background:#FAFAFA;
	border:#999 1px solid;
	padding:20px;
	margin:25px;
}
.leaveWords ul{
	margin:0;
	padding:0 0 5px 20px;
	border-bottom:#999 1px solid;
}
.leaveWords ul li.people{
	list-style:url("guestimages/ico1.gif");
	float:left;
	width:200px;
	font-size:16px;
}
.leaveWords ul li.theme{
	list-style:url("guestimages/icon3.jpg");
	float:left;
	width:200px;
	font-size:15px;
}
.leaveWords ul li.time{
	list-style:url("guestimages/ico2.gif");
	float:left;
}
.leaveWords .words{
	margin-left:23px;
	font-size:14px;
}
.leaveWords .words p{
	margin:5px 0;
	color:#555;
}
.leaveWords .words .reply{
	border-top:#F90 2px dashed;
	padding-top:10px;
}
.leaveWords .words .reply p{
	color:#F00;
}
.changepage{
	text-align:right;
	margin:0 20px;
	padding:10px 20px;
	border:#999 1px solid;
	background:#FAFAFA;
	font-size:14px;
}
.changepage a{
	border:#CCC 1px solid;
	padding:5px 10px;
	margin-right:5px;
	color:#000;
	font-size:14px;
}
.changepage a:hover{
	background:#DDD;
}
.message{
	border:#999 1px solid;
	margin:20px;
	padding:20px;
	background:#FAFAFA;
}
.message h2{
	font-size:20px;
	font-weight:normal;
	margin:0;
	padding-bottom:10px;
	border-bottom:#999 1px solid;
}
.message h2 span{
	font-size:14px;
}
.message .text div{
	margin-top:15px;
}
.message .text div textarea{
	margin-bottom:10px;
	padding:10px;
	width:848px;
	height:120px;
	max-width:848px;
	max-height:120px;
	overflow:auto;
}
.message .text div input[type="text"]{
	width:200px;
}
.message .text input[type="submit"]{
	background:url("guestimages/sub.gif");
	color:#FFF;
	height: 26px;
	line-height: 26px;
	text-align: center;
	width: 54px;
	border:none;
	padding:0;
	margin-right:10px;
}
.message .text input[type="text"]{
	width:50px;
	height:22px;
	padding:0 0 0 3px;
	font-family:Arial, Helvetica, sans-serif;
	margin-right:10px;
}
.message .text img{
	height:24px;
	width:50px;
	margin-top:-5px;
	margin-right:10px;
}
</style>
<!--我添加的样式表结束-->
<script>
    function RefreshImage() {
        var el = document.getElementById("yanzhengma");
        el.src = el.src + '?';
    }
    function checkpost() {

        if (document.getElementById("UserName").value == "") {
            alert("用户名不能为空！");
            document.getElementById("UserName").focus();
            return false;

        }
        if (document.getElementById("Title").value == "") {
            alert("标题不可为空！");
            document.getElementById("Title").focus();
            return false;
        }
        if (document.getElementById("Content").value == "") {
            alert("内容不可为空");
            document.getElementById("Content").focus();
            return false;

        }
        if (document.getElementById("yz").value == "") {
            alert("请填写验证码！");
            document.getElementById("yz").focus();
            return false;
        }

        return true;
    }
</Script>
</head>

<body>
 <form id="form1" runat="server">
 
	<div class="logo"></div>
    <ul class="nav">
    	<li id="n1"><a class="n1"  href="/sites/demo/pub/"></a></li>
    	<li  id="n2"><a class="n2" href="/sites/Demo/pub/lmsz/2013110374.html"></a></li>
    	<li id="n3"><a class="n3"  href="/sites/demo/pub/xwytz/news"></a></li>
    	<li  id="n4"><a class="n4" href="/sites/demo/pub/Research/"></a></li>
    	<li id="n5"><a class="n5"  href="/sites/demo/pub/peixun/media"></a></li>
    	<li id="n6"><a class="n6"  href="/sites/demo/pub/jyjsss/dmt"></a></li>
    	<li  id="n7"><a class="n7" href="http://210.43.192.80"></a></li>
    	<li id="n8"><a class="n8"  href="/guest.aspx"></a></li>
    	<li  id="n9"><a class="n9" href="/sites/demo/pub/down/"></a></li>
    	<li id="n10"><a class="n10"  href="http://www.csust.edu.cn"></a></li>
    </ul><!-- nav -->
<script>
    var str = window.location;
    if (str.toString().indexOf("lmsz") >= 0)//中心简介  n2
        document.getElementById("n2").className = "current";
    else if (str.toString().indexOf("xwytz") >= 0)//通知公告  n3
        document.getElementById("n3").className = "current";
    else if (str.toString().indexOf("Research") >= 0)//教育技术研究  n4
        document.getElementById("n4").className = "current";
    else if (str.toString().indexOf("peixun") >= 0)//教育技术培训  n5
        document.getElementById("n5").className = "current";
    else if (str.toString().indexOf("jyjsfw") >= 0)//教育技术服务  n6
        document.getElementById("n6").className = "current";
    else if (str.toString().indexOf("yyjt") >= 0)//校电视台  n7
        document.getElementById("n7").className = "current";
    else if (str.toString().indexOf("zxdy") >= 0)//在线答疑  n8
        document.getElementById("n8").className = "current";
    else if (str.toString().indexOf("down") >= 0)//下载中心  n9
        document.getElementById("n9").className = "current";
    else
        document.getElementById("n1").className = "current";
</script>    
    <div class="secBox">
        <div class="sec-first t10">
	    	<img src="/sites/Demo/templates/atts/20130116/images/gwfn_sec_031.jpg" alt="" />
    	</div>  	
        <div class="sec-second">
        	<h2>在线答疑</h2>
            <div><a href='/sites/Demo/pub/'>首页</a>&raquo;<a href="/guest.aspx">在线答疑</a></div>
        </div><!-- sec-second -->        
        <div class="jpkc-third t10">

  

<div class="mleft">
  <asp:Repeater ID="Repeater1" runat="server" 
        onitemdatabound="Repeater1_ItemDataBound">
  　<ItemTemplate>
  	<div class="leaveWords">
    	<ul>
        	<li class="people"><%# Eval("username") %>
            </li>
            <li class="theme">主题:<%# Eval("title") %></li>
            <li class="time"><%# Eval("addtime") %>
            </li>
            <div style="clear:both;"></div>
        </ul>
        
        <div class="words">
         	<p><%# Eval("content") %>  </p>
            <div class="reply">
            	<strong>回复内容</strong>
                <asp:Repeater runat="server" id="Repeater2">
　<ItemTemplate>
 <p><%# Eval("content") %> </p>
</ItemTemplate>
</asp:Repeater>
            </div>
        </div>
    </div>
     </ItemTemplate>
    </asp:Repeater>
  	<div class="changepage"> 
         <cc2:UcfarPager ID="Pager2" runat="Server"  PageSize="8"  PagePara="Page"  
                           PageStyle="十页缩略">
</cc2:UcfarPager>
            
          <div style="clear:both;"></div>
    </div>
    
    <div class="message">
            <h2>填写你的留言<span> / Message</span></h2>                     
            <div class="text">
            	<div>
                	您的姓名：
                     <asp:TextBox runat="server" id="UserName"> </asp:TextBox>
                	留言主题： <asp:TextBox runat="server"  id="Title">
              </asp:TextBox></div>
               	<div>留言内容： <asp:TextBox runat="server"  id="Content" TextMode="MultiLine"></asp:TextBox></div>
                
                <asp:ImageButton 
                ID="ImageButton1" style="width:auto; height:auto; border:none;" 
                src="guestimages/sub.gif" runat="server"  OnClientClick="return checkpost()" onclick="ImageButton1_Click" />
           					
                 <asp:TextBox runat="server"  id="yz" style="width:60px">
            </asp:TextBox>
                <img src="ajaxhandler/produceyanz.aspx" id="yanzhengma"  style="margin-top:10px;" onclick="RefreshImage()" height="20px" width="50px" />
              <a href="javascript:RefreshImage()">看不清，换一张 </a>
            </div>
        </div>
            
  
  </div>
   
    <div style="clear:both;"></div>
        </div><!-- jpkc-third -->
    </div><!-- secBox -->
    <div class="footer t10">长沙理工大学版权所有 长沙理工大学现代教育技术中心 湖南省长沙市(雨花区)万家丽南路2段960号 邮编:410014 域名备案信息：湘ICP备05003881号<script src="http://s17.cnzz.com/stat.php?id=4956004&web_id=4956004&show=pic1" language="JavaScript"></script>
</div>  
    

    </form>
</body>
</html>
