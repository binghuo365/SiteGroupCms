<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Help_For_FileList2.ascx.cs" Inherits="MSManager.Web.Help.Help_For_FileList2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>使用帮助</title>
<style type="text/css">
#ul1 { margin:0px;margin-left:6px;padding:0px; list-style-type:none;}
#ul1 li{ margin:0px;padding:0px;height:25px;  font-size:14px; }
</style>
</head>
<body style="margin:0px;">
<form id="form1" runat="server" encType="multipart/form-data" method="post">

<table width="600" bgcolor="#ffffff" cellspacing="10" align="center">
<tr>
<td>

<!--空白边-->

<table height="330" cellSpacing="0" cellPadding="5" width="100%" align="center" border="0" style="background-color:#f6f9fd;border:solid 1px #98b1c8;">

<tr height="24">
<td align="left" valign="top">
	<div style="padding-left:6px;padding-top:5px;font-size:14px;font-weight:bold;">
	文件管理使用帮助/基于图标方式浏览
	
	<hr style="height:10px;color:#98b1c8;">
	</div>
	
	<ul id="ul1">
	<li>1、所有针对文件的操作须在文件被选定后才有效；</li>
	<li>2、在文件/文件夹上单击可选择该文件/文件夹；</li>
	<li>3、双击文件夹可进入下级目录；双击文件可下载；</li>
	<li>4、支持键盘上的四个方向键来选择上一个、下一个文件；</li>
	<li>5、可按下ctrl或shift键（与在windows中操作类似），快速选取多个文件；</li>
	<li>6、对于全选、复制、剪切、粘贴、删除操作支持键盘操作，对应的键盘快捷键如下：

	    <div style="padding-top:5px;margin-left:20px;line-height:150%;">
	    全选：ctrl + A；
	    复制：ctrl + c；<br />
	    剪切：ctrl + x；
	    粘贴：ctrl + v；<br />
	    压缩：ctrl + z；
	    解压：ctrl + u；<br />
	    删除：delete；   	    
	    
	    <br /><br />
	    <span style="font-weight:bold;">注：当文件以图标方式浏览时，页面功能只支持IE浏览器；</span>	    
	    </div>
	</li>
	</ul>
	
</td>
</tr>
</table>

<!--空白边-->

</td>
</tr>
</table>
<center><a href="javascript:window.close();">[关闭]</a></center>

</form>
</body>
</html>
