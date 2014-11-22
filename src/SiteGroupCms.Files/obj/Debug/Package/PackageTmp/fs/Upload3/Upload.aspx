<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="MSManager.Web.Upload3.Upload" %>
<%@ Register Assembly="chaokers.cn.FSManager.Controls" Namespace="FSManager.Controls" TagPrefix="fs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>文件上传</title>
<link href="../App_Themes/Default/css/style.css" type="text/css" rel="stylesheet" />
<link href="../App_Themes/Default/css/navbar.css" type="text/css" rel="stylesheet" />
</head>
<body>
<form id="Form1" method="post" runat="server" enctype="multipart/form-data">
<div id="wrapper" style="text-align:left;">

    <fs:NavBar ID="NavBar1" runat="server" ConfigXmlPath="../ShellConfig/Navbar.xml" ModuleName="File" />  
    
    <%
        if (base.SpaceStaticError)
        {
            Response.Write("<div style='margin-top:0px;margin-bottom:5px;padding:5px;font-size:14px;color:red;text-align:left;border:solid 1px #cccccc;border-left:0px;border-right:0px;border-top:0px;background-color:#fffff0;line-height:150%;'>提示：空间信息统计失败，上传操作暂时无法使用；</div>");
            this.btnUpload.Enabled = false;
        }
        
        if (base.FSObjIsTooMuch)
        {
            Response.Write("<div style='padding:5px;font-size:14px;color:red;text-align:left;border:solid 1px #cccccc;border-left:0px;border-right:0px;border-top:0px;background-color:#fffff0;line-height:150%;'>尊敬的用户，由于您的空间内的文件（夹）数量过多（免费版最多支持" + FSManager.Components.Config.SpaceConfig.GetMaxFSObjCountForFileManageOnline() + "个文件/夹），WEB上传功能暂不可用，如需上传文件，请先删除一些文件然后刷新空间。</div>");
            this.btnUpload.Enabled = false;
        }
    %>
    
    <fs:CurrLocation ID="CurrLocation1" runat="server" InnerText="文件管理/网页版大上传文件" Height="17px">
    </fs:CurrLocation>

    <div id="Div1" style="width:700px;padding-left:10px;text-align:left;">
    
    
    <!--显示空间使用情况.Begin-->	
	<div style="padding:0px;text-align:left;">
	
    <p style="font-size:14px;font-weight:bold;margin-bottom:5px;margin-top:10px;">您的空间使用情况如下：</p>
    
    <table cellpadding="2" cellspacing="0" style="border:solid 1px #cccccc;background-color:#ffffff;height:18px;" width="100%">    
    <tr>        
        <asp:Literal ID="literal1" runat="server" EnableViewState="false" />
    </tr>
    </table>
    
    
    
    <div style="line-height:170%;">
    总共：<asp:Literal ID="literalSpaceCapacity" runat="server" EnableViewState="false" /> &nbsp;&nbsp;
    已用：<asp:Literal ID="literalSpaceUsedInfo" runat="server" EnableViewState="false" /> &nbsp;&nbsp;
    剩余：<asp:Literal ID="literalSpaceRemanent" runat="server" EnableViewState="false" />
    </div>
    
    <asp:Literal ID="literalNote" runat="server" EnableViewState="false" />
    
    </div>       
    <!--显示空间使用情况.END-->
    
		
	<br />
    
    
    <div id="container" style="text-align:left;margin:0px;padding:25px;background-color:#F6F9FD;border:solid 1px #98B1C8;">

        <span style="font-weight:bold;font-size:14px;">文件上传</span>
        <hr style="height:4px;color:#98B1C8;" />

        <!--上传部分.begin-->
        <div>
        
            <asp:FileUpload ID="fileUpload" runat="server" 
                 CssClass="input"
                 Height="21px" />
            <asp:Button ID="btnUpload" runat="server" 
                 Text=" 上 传 "
                 CssClass="btn"
                 OnClick="btnUpload_Click"
                 OnLoad="btnUpload_Load" />         
            
		    <input type="button" value=" 返 回 " class="btn" onClick="window.location='../Route.aspx?SubPath=<% = Server.UrlEncode(base.subPath) %>';" />
            <br />
            
            <asp:Label ID="lblUploadInfo" runat="server" style="margin-top:3px;"/>
            
            <!--显示上传成功后的重新上传的提示信息.begin-->
            <div id="divHidden" runat="server" visible="false" style="margin-top:10px;font-size:14px;">                                    
            
            或者，您也可以点击
                <asp:LinkButton ID="lbnReUpload" runat="server"
                     Text="[重新上传]"
                     ForeColor="blue"
                     OnClick="lbnReUpload_Click" />        
            按钮以重新上传文件.                
            </div>
            <!--显示上传成功后的重新上传的提示信息.end-->
            
            <div style="display:none;">
	        <asp:TextBox ID="txtUploadedFileName" runat="server"
                         CssClass="input" />
            </div>	
        
        </div>
        <!--上传部分.end-->
          
        <br />
        
        <asp:
        
        <!--说明部分.begin-->
        <span style="font-weight:bold;font-size:12px;">上传说明：</span>
        <br />
        <hr style="height:4px;color:#98B1C8;" />
        <div style="font-size:12px;">
            <ul style="list-style-type:none;margin:0px;">
            <li>1、最大可上传50M;</li>
            </ul>
        </div>
        <!--说明部分.end-->
    
    </div>
    
    
    </div><!--"Div1".END-->

</div>
</form>
</body>
</html>
