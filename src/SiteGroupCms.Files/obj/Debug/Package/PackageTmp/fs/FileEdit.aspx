<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="FileEdit" Codebehind="FileEdit.aspx.cs" %>
<%@ Register Assembly="chaokers.cn.FSManager.Controls" Namespace="FSManager.Controls" TagPrefix="fs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>文件编辑</title>
<link href="App_Themes/Default/css/style.css" type="text/css" rel="stylesheet" />
<link href="App_Themes/Default/css/navbar.css" type="text/css" rel="stylesheet" />
</head>
<body>
<form id="Form1" method="post" runat="server">
<div id="wrapper">

    <fs:NavBar ID="NavBar1" runat="server" ConfigXmlPath="ShellConfig/Navbar.xml" ModuleName="File" />
    
    <fs:CurrLocation ID="CurrLocation1" runat="server" InnerText="" Height="17px">
    </fs:CurrLocation>
    
    <br />        
    
    <div id="container" style="padding-left:10px;text-align:left;">
    
    <!--main.主表.begin-->
    
    <table style="width:100%;" align="center" cellpadding="0" cellspacing="0" class="table-border-1px" >
    <tr>
    <td style="background-color:#e7f5ff;font-weight:bold;padding-left:5px;height:30px;">    
        <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
        <td align="left">
            读取编码：
	        <asp:DropDownList ID="ddlFileEncodingList" Runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFileEncodingList_OnSelectedIndexChanged"></asp:DropDownList>
	        （注：如果文件的读取出现乱码，可以尝试更改编码类型）
        </td>
        <td align="right">
            <asp:CheckBox ID="chkAutoWrap" Runat="server" Text="自动换行" AutoPostBack="True" OnCheckedChanged="chkAutoWrap_OnCheckedChanged" />                    
            &nbsp;
        </td>
        </tr>
        </table>     
    </td>
    </tr>
    <tr>
    <td valign="top">
        
        <table width="100%" cellspacing="0" cellpadding="10" align="center" bgcolor="#3269A7" border="0">      
        <tr bgcolor="#ffffff">
	        <td>
	        <asp:TextBox ID="txtFileContent" Runat="server"
		         TextMode="MultiLine"
		         Width="100%"
		         Height="310"
		         Wrap="False" />		
	        </td>
        </tr>
        <tr bgcolor="#ffffff">
	        <td height="15" align="left">
	        写入编码：
	        <asp:DropDownList ID="ddlWriteFileEncodingList" Runat="server" ></asp:DropDownList>	
	        
	        <asp:Button ID="btnSave" Runat="server" OnClick="btnSave_Click" CssClass="btn" Text=" 保 存 " />&nbsp;
            
            <input name="Submit" type="button" class="btn" value=" 返 回 " onClick="window.location='Route.aspx?SubPath=<% = Server.UrlPathEncode(base.subPath) %>'">		
            
            <!--提示是否确定保存.BEGIN-->
            <asp:Panel ID="panelConfirm" runat="server" Visible="false">
	       
	        <br />
	        
	        <asp:Label ID="lblConfirm" runat="server" Font-Bold="true" Text="提示：该文件的读取编码与写入编码不一致，保存后将有可能会出现乱码，是否确定保存？" /> 	        
	        	        
	        <asp:Button id="btnConfirm" runat="server" Text="确定保存" CssClass="btn" OnClick="btnConfirm_Click" />
	        
	        <asp:Button ID="btnCancel" runat="server" Text=" 取 消 " CssClass="btn"  OnClick="btnCancel_Click" />
	        
	        </asp:Panel>
	        <!--提示是否确定保存.END-->
	        
	        </td>
        </tr>
        <tr bgcolor="#ffffff" style="display:none;">
	        <td align="center" colSpan="2">
	            <div style="width:80%;text-align:left;">	            
        	    
	            文件另存为：		
		        <asp:TextBox ID="txtSaveAs" Runat="server" CssClass="txt" />
		        <asp:Button ID="btnSaveAs" Runat="server" OnClick="btnSaveAs_Click" CssClass="btn" Text=" 确 定 " />(请填写全名，如：abc.txt)
        		
		        <br /><br />
		        <input name="Submit" type="button" class="btn" value=" 返 回 " onClick="window.location='Route.aspx?SubPath=<% = base.subPath %>'">		
		        <asp:Label ID="lblResult" Runat="server"></asp:Label>
		        </div>
	        </td>
        </tr>
        </table>
        
        <br />
        
        
    </td>
    </tr>      
    </table>    
    
    <br />  
    
    </div><!--"container".End-->    

</div>
</form>

</body>
</html>
