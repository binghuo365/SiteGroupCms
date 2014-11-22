<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RefreshSpace.aspx.cs" Inherits="MSManager.Web.RefreshSpace" %>
<%@ Register Assembly="chaokers.cn.FSManager.Controls" Namespace="FSManager.Controls" TagPrefix="fs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<asp:Literal ID="title" runat="server">文件管理/刷新空间</asp:Literal>   
<link href="App_Themes/Default/css/navbar.css" type="text/css" rel="stylesheet" />
<style type="text/css">
.div1{ padding:20px;padding-left:100px;text-align:left; }
</style>
</head>
<body>
<form id="form1" runat="server">
<div id="wrapper">
    
    <fs:NavBar ID="NavBar1" runat="server" ConfigXmlPath="ShellConfig/Navbar.xml" ModuleName="File" />
    
    <fs:CurrLocation ID="CurrLocation1" runat="server" InnerText="文件管理/刷新空间" Height="17px">
    </fs:CurrLocation>

    <div id="container" style="width:700px;padding-left:10px;text-align:left;">    

    <br />
        
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>    
    
    <fs:PagePart ID="pagePart1" runat="server" HeaderText="刷新空间" BodyPadding="1px" BodyHeight="60px">        
        
        <div class="div1">
        
        <asp:Button ID="btnRefresh" runat="server"
             Text=" 刷新 "
             CssClass="btn"
             Width="80px"
             OnClick="btnRefresh_Click"
             />    
        
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
             ShowMessageBox="True" 
             ShowSummary="False" />  
        
        &nbsp;&nbsp;
        
        验证码：
        <asp:TextBox ID="txtVfyCode" runat="server" 
             Width="150px"
             CssClass="input" /> <img src="VfyCode/VfyCodeForRefreshSpace.aspx" border="0" />
        
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
             ControlToValidate="txtVfyCode" 
             Display="None" 
             ErrorMessage="请填写验证码！" />     
        
        <br /><br />
        <asp:Label ID="lblRefreshInfo" runat="server"></asp:Label>
        
        
        <asp:UpdateProgress ID="UpdateProgress2" runat="server"  AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            正在刷新，请稍后...
        </ProgressTemplate>
        </asp:UpdateProgress>  
        
        
        </div>       
        
    </fs:PagePart>   
    
    </ContentTemplate>
    </asp:UpdatePanel>  
    
    <br />
    
    <div style="padding:10px;border:solid 1px #98b1c8;background-color:#f6f9fd;">
    <h1 style="font-size:12px;margin:0px;">刷新空间有什么用？</h1>
    
    <p style="line-height:150%;">    
    1、如果您的空间内的文件（夹）数量过多，则系统会禁止通过文件在线管理功能上传文件，此时，您可以先删除一些文件，然后<br />&nbsp;&nbsp;&nbsp;在此刷新空间，如果系统检测到您空间内的文件（夹）数量小于系统限制，便自动解除对WEB上传的禁止； <br />
     
    2、频繁的文件操作可能会导致系统无法准确统计出您空间的使用情况（比如：已使用空间、文件/夹数量等信息），刷新空间可<br />&nbsp;&nbsp;&nbsp;以及时让您的空间管理面板显示准确的空间使用情况；
    </p>
    </div>   
    
    </div><!--"container".End-->
    
    <br /><br /><br /><br /><br /><br />  
  
</div>
</form>
</body>
</html>
