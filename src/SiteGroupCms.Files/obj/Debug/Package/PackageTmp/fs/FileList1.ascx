<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileList1.ascx.cs" Inherits="MSManager.Web.FileList1" %>
<%@ Register Assembly="chaokers.cn.FSManager.Controls" Namespace="FSManager.Controls" TagPrefix="fs" %>
<%@ Import Namespace="FSManager.Components" %>
<%@ Import Namespace="FSManager.Components.Enums" %>
<%@ Import Namespace="System.Data" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<asp:Literal ID="title" runat="server">文件管理</asp:Literal>  
<link href="App_Themes/Default/css/style.css" type="text/css" rel="stylesheet" />
<link href="App_Themes/Default/css/navbar.css" type="text/css" rel="stylesheet" />
<asp:Literal ID="copyright" runat="server"></asp:Literal>
<style type="text/css">
#tbMenu td{ width:60px; }
</style>

</head>
<body style="font-family:宋体;margin:0px;" onselect="document.selection.empty();" onselectstart="return true">

<script type="text/javaScript">
var subPath = "<% = base.subPath %>";
var inputBoxForNewFSName = "<% = txtFSName.ClientID %>";

var txtFileNameClientId = "<% = txtFileName.ClientID %>"; 
var ddlFileTemplateClientID = "<% = ddlFileTemplateList.ClientID %>"; 

</script> 

<script event="onkeydown" for="document" type="text/javaScript">
if(event.ctrlKey)
{		
	keyboardEventHandler(event.keyCode);
}
else
{
	if(event.keyCode==46) 
	{
		deleteFS();
	}
}
</script> 

<form id="aspnetForm" method="post" runat="server" style="margin:0px;padding:0px;">

<div id="content" style="margin:0px;padding-top:0px;">
    
    <fs:FSOprSupport_1 runat="server" />
    <fs:FSOprSupport_1_ForKeyboardEvents runat="server" />
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <fs:NavBar ID="NavBar1" runat="server" ConfigXmlPath="ShellConfig/Navbar.xml" ModuleName="File" />
    
    <fs:CurrLocation ID="CurrLocation1" runat="server">
        <div class="currLocation-TbContainer">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
        <td>当前位置：文件管理/<%=base.currPosition%></td>
        <td align="right">
        
        <% if (base.browser.Browser == "IE") { %>
        
        <img src="App_Themes/Default/Images/icon_tubiao.gif" border="0" align="absmiddle" /> <a href="Switcher.aspx?Style=2&SubPath=<% = Server.UrlEncode(base.subPath) %>">切换到图标方式浏览</a>
                
        <% } %>
        
        </td>
        </tr>
        </table> 
        </div>
    </fs:CurrLocation>

    <div class="note"><p>注：1）支持右键快捷菜单和键盘事件（仅在IE下支持）；2）根目录为站点下的templates/atts;3）模板文件位于templates/</p></div>

    <%
        if (base.SpaceStaticError)
        {
            Response.Write("<div style='margin-top:0px;margin-bottom:5px;padding:5px;font-size:14px;color:red;text-align:left;border:solid 1px #cccccc;border-left:0px;border-right:0px;border-top:0px;background-color:#fffff0;line-height:150%;'>提示：空间信息统计失败，系统暂时禁止使用上传、删除、复制、解压、压缩、新建文件、新建文件夹操作；</div>");
        }
        
        if (base.FSObjIsTooMuch)
        {
            Response.Write("<div style='margin-top:0px;margin-bottom:5px;padding:5px;font-size:14px;color:red;text-align:left;border:solid 1px #cccccc;border-left:0px;border-right:0px;border-top:0px;background-color:#fffff0;line-height:150%;'>尊敬的用户，由于您的空间内的文件（夹）数量过多（免费版最多支持" + FSManager.Components.Config.SpaceConfig.GetMaxFSObjCountForFileManageOnline() + "个文件/夹），系统已经无法准确统计出您空间的使用情况，您将不能再通过文件在线管理功能进行上传、复制、解压、压缩操作，如果你需要对文件进行这些操作，请先删除一些文件然后刷新空间或者使用FTP进行管理。</div>"); 
        }        
    %>

    <!--Current location.Start-->
    <!--<div class="toptitle" style="margin-top:0px; height:15px">&nbsp;当前位置：<%=base.currPosition%> </div>-->
    <!--Current location.End-->

    <!--Action menu.Start-->
    <table height="56" border="0" id="tbMenu">
    <tr>
    <td>
      <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="UpBtn" runat="server" ToolTip="向上一级" OnClick="Act" CommandName="ToPrevDirectory" ImageUrl="App_Themes/Default/Images/fsAction/up.gif" /></asp:ImageButton>
      </div>
    </td>
    <td>
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><a href="." onfocus="blur();" ><IMG alt="根目录" src="App_Themes/Default/Images/fsAction/home.gif" border="0" /></a></div>
    </td>
    <!--
    <td>
      <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="RefreshBtn" runat="server" ToolTip="刷新本页" OnClick="Act" CommandName="Refresh" ImageUrl="App_Themes/Default/Images/fsAction/fs_refresh.gif" /></asp:ImageButton>
      </div>
    </td>
    -->
    <td id="tdnewfolder">
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><a href="javascript:popPanel(document.getElementById('tdnewfolder'), 'tbNewDir', 6, 58);" onfocus="blur();" ><IMG alt="新文件夹" src="App_Themes/Default/Images/fsAction/new_folder.gif" border="0" /></a></div>
    </td>
    <td id="tdnewfile">
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><a href="javascript:popPanel(document.getElementById('tdnewfile'), 'tbNewFile', 6, 58);" onfocus="blur();" ><IMG alt="新建文本文件" src="App_Themes/Default/Images/fsAction/new_file.gif" border="0" /></a></div>
    </td>
    <td id="tddel">
      <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="del" runat="server" ToolTip="删除" OnClick="Act" CommandName="Delete" ImageUrl="App_Themes/Default/Images/fsAction/delete_big.gif"></asp:ImageButton>
      </div>
    </td>
    <td id="tdcut">
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="cut" runat="server" ToolTip="剪切" OnClick="Act" CommandName="Cut" ImageUrl="App_Themes/Default/Images/fsAction/cut.gif"></asp:ImageButton></div>
    </td>
    <td id="tdcopy">
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="copy" runat="server" ToolTip="复制" OnClick="Act" CommandName="Copy" ImageUrl="App_Themes/Default/Images/fsAction/copy.gif"></asp:ImageButton></div>
    </td>
    <td id="tdpas">
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="pas" runat="server" ToolTip="粘贴" OnClick="Act" CommandName="Paste" ImageUrl="App_Themes/Default/Images/fsAction/pas.gif"></asp:ImageButton></div>
    </td>    
    <td id="tdzip">
        <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><a href="javascript:zip1();" onfocus="blur();" ><IMG alt="压缩" src="App_Themes/Default/Images/fsAction/zip.gif" border="0" /></a></div>	    
    </td>
    <td id="tdunzip">
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><a href="javascript:unzip1();" onfocus="blur();" ><IMG alt="解压" src="App_Themes/Default/Images/fsAction/unzip.png" border="0" /></a></div>
    </td>
    <td id="tdupload">
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="upload" runat="server" ToolTip="上传文件" OnClick="Act" CommandName="Upload" ImageUrl="App_Themes/Default/Images/fsAction/upload.gif"></asp:ImageButton></div>
    </td>	
    </tr>
    <tr>
    <td width="61">
	    <div align="center">向上一级</div>
    </td>
    <td>
	    <div align="center">根目录</div>
    </td>
    <!--
    <td>
	    <div align="center">刷新本页</div>
    </td>
    -->
    <td id="tdnewfolder1">
	    <div align="center">新文件夹</div>
    </td>
    <td id="tdnewfile1">
	    <div align="center">新建文件</div>
    </td>	
    <td id="tddel1">
	    <div align="center">删除</div>
    </td>
    <td id="tdcut1">
	    <div align="center">剪切</div>
    </td>
    <td id="tdcopy1">
	    <div align="center">复制</div>
    </td>
    <td id="tdpas1">
	    <div align="center">粘贴</div>
    </td>
    <td>
	    <div align="center">压缩</div>
    </td>
    <td>
	    <div align="center">解压</div>
    </td>
    <td id="tdupload1">
	    <div align="center">上传文件</div>
    </td>
    </tr>
    </table>
    <!--Action menu.End-->

    <table cellpadding="0" cellspacing="0" width="100%" border="0"  style="margin-left:0px;">
    <tr>
    <td valign="top">

        <!--FsList.Start-->
        <asp:Repeater ID="myRepeater" EnableViewState="False" Runat="server">	
        <HeaderTemplate>
        <table width="100%" cellpadding="1" cellspacing="0" style="border-top:#ccc 1px solid">
        <tr class="table_tr bgcolor bold">
        <td align="center" nowrap="nowrap" width="40" height="30"><input type="checkbox" name="chkAll" onClick="javascript:SelectAll();" /></td>
        <td align="center" nowrap="nowrap" width="30"> 类型 </td>
        <td> 名称 </td>
        <td align="right" nowrap="nowrap" width="80"> 大小 </td>
        <td align="center" nowrap="nowrap" width="20">&nbsp;</td>
        <td align="center" nowrap="nowrap" width="150">最后更新时间</td>
        <td align="center" nowrap="nowrap" width="60">重命名</td>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr id="tr_<%# Container.ItemIndex%>" class="table_tr" onmouseover="setMouseoverState(<%# Container.ItemIndex%>);" onmouseout="setMouseoutState(<%# Container.ItemIndex%>)" ondblclick="ondblclickOnTr(<%# Container.ItemIndex%>);">
        <td align="center" height="28" id='tdForRename_<%# Container.ItemIndex%>'>
	        <input type="checkbox" name="chkBox" onClick="UnSelectAll(<%# (Container.ItemIndex)%>);" value="<%# Container.ItemIndex%>|<%# ((DataRowView)Container.DataItem)["FsType"]%>|<%# ((DataRowView)Container.DataItem)["FsSize"]%>">
	        <input type="hidden" name="fsName<%# Container.ItemIndex%>" value="<%# ((DataRowView)Container.DataItem)["FsName"]%>">
        </td>
        <td align="center">
	        <img src="App_Themes/Default/Images/filetype/<%# Format.GetFsTypeIcon(((DataRowView)Container.DataItem)["FsType"].ToString())%>.gif" border="0">
        </td>
        <td align="left">            
	        <%# base.ShowFsName(base.serverDomainToVisitUserSpace, base.userID, ((DataRowView)Container.DataItem)["FsLink"].ToString(), ((DataRowView)Container.DataItem)["FsType"].ToString(), ((DataRowView)Container.DataItem)["FsName"].ToString(), ((DataRowView)Container.DataItem)["FsLink"].ToString())%>
	        <%# Format.ShowDownLink(((DataRowView)Container.DataItem)["FsType"].ToString(), ((DataRowView)Container.DataItem)["FsLink"].ToString())%>
        </td>
        <td align="right">
	        &nbsp;<%# Format.FormatFileLength(((DataRowView)Container.DataItem)["FsType"].ToString(),((DataRowView)Container.DataItem)["FsSize"].ToString())%>
        </td>
        <td>&nbsp;</td>
        <td align="left">
	        <%# ((DataRowView)Container.DataItem)["LastUpdate"]%>
        </td>
        <td align="center" >            
	        <a href="javascript:renameFS(document.getElementById('tdForRename_<%# Container.ItemIndex%>'),'<%# Eval("FSName")%>','<%# ((DataRowView)Container.DataItem)["FsType"]%>','<%# ((DataRowView)Container.DataItem)["FsLink"].ToString().Replace("\\","/")%>');" onFocus="blur();" ><img src="App_Themes/Default/Images/fsAction/rename.gif" border="0" alt="重命名"></a>
        </td>
        </tr>
        </ItemTemplate>        
        <FooterTemplate>
        </table>

        </FooterTemplate>
        </asp:Repeater>
        <asp:Label ID="lblHidden" Runat="server" EnableViewState="False"></asp:Label>
        <input type="hidden" name="chkNum" value="<%=chkNum%>" />
        <!--FsList.End-->


        <table width="100%" border="0" id="tbFileFooter" runat="server">
        <tr class="table_tr ">
	        <td height="28">&nbsp;<asp:Label id="lblInfo" runat="server" Width="432" Height="18"></asp:Label></td>
        </tr>
        </table>
        

    </td>
    <td width="10"></td>
    <!--关闭右侧栏.BEGIN-->
    <td valign="top" style="width:1px;padding-right:5px;">
        <div id="closeSpaceInfoPanel" style="padding:0px;margin:0px;width:180px;display:<% =base.closeSpacePanelDisplayPropertyValue %>;" >
                
        <iframe height="360" id="mainFrame" src="SpaceInfo.aspx?SubPath=<% = base.subPath %>" scrolling="no" frameborder="0" width="100%"><noframes>您的浏览器不支持框架，请使用较新版本的浏览器！</noframes></iframe>
        
        <br />

        <table style="width:99%;margin-top:1px;" align="center" cellpadding="0" cellspacing="0" border="0" >
        <tr>
        <td align="left" style="line-height:200%;">
            <div id="div_closeSpacePanel">            
            <span style="font-size:14px;"><a href="javascript:quickCloseSpaceInfoPanel();">[快速关闭右侧栏→]</a></span>



            </div>           
        </td>
        </tr> 
        </table>        
        
        </div>
    </td>
    <!--关闭右侧栏.END-->
    <!--打开右侧栏.BEGIN-->
    <td style="width:1px;font-size:14px;text-align:center;vertical-align:top;">
        
        <div id="openSpaceInfoPanel" style="padding:0px;margin:0px;width:30px;display:<% =base.openSpacePanelDisplayPropertyValue %>;background-color:#f5f5f5;border:solid 1px #cccccc;border-right:0px;">
        
        <br />
        
        <a href="javascript:quickOpenSpaceInfoPanel();">
        快<br />
        速<br />
        打<br />
        开<br />
        右<br />
        侧<br />
        栏<br />
        ←
        </a>
        
        </div>
    </td>
    <!--打开右侧栏.END-->
    </tr>    
    </table>
    
    <!--创建新文件.Begin-->
    <div id="tbNewFile" style="width:300px;display:none;border:solid 1px #9DB3C5;background-color:red;" >
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
    <tr>
    <td style="background-image:url(App_Themes/Default/Images/bg1.jpg);">
        <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td style="font-weight:bold;padding-left:10px;height:30px;text-align:left;"><img src="App_Themes/Default/Images/icon_newfile.gif" border="0" align="absmiddle" /> 新建文件</td>
        <td align="right" style="padding-right:10px;"><a href="javascript:hidePanel('tbNewFile'); "><img src="App_Themes/Default/Images/icon_close.gif" border="0" alt="关闭" /></a></td>
        </tr>
        </table>
    </td>
    </tr> 
    <tr>
    <td style="background-color:#f9f9f9;">    
        
           <asp:UpdatePanel ID="UpdatePanel1" runat="server">
           <ContentTemplate>
           <table cellpadding="3" cellspacing="0" border="0">
           <tr>
           <td width="20"></td>
           <td height="50">
           
               文件类型：
               <asp:DropDownList ID="ddlFileTemplateList" runat="server">
               <asp:ListItem Value="txt" Selected="True">文本文件</asp:ListItem>
               <asp:ListItem Value="html">html网页文件</asp:ListItem>
               <asp:ListItem Value="asp">asp网页文件</asp:ListItem>
               <asp:ListItem Value="aspx">aspx网页文件</asp:ListItem>
               <asp:ListItem Value="css">css样式表文件</asp:ListItem>
               <asp:ListItem Value="xml">xml文件</asp:ListItem>
               </asp:DropDownList>
               
               <br /><br />
           
               文 件 名：
               <asp:TextBox ID="txtFileName" runat="server"
                    Width="140px"
                    ValidationGroup="NewFile"
                    Text="新建文本文件.txt" />
                    
               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtFileName" 
                    Display="None" 
                    ErrorMessage="请输入文件名！" 
                    ValidationGroup="NewFile" />
               
               <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server"
	                EnableViewState="False" 
	                ErrorMessage="文件名中含有非法字符！"
	                Display="None"
	                ControlToValidate="txtFileName" 
	                ValidationExpression="[^\\|<>:*?/']{1,50}" 
	                ValidationGroup="NewFile" />
               
           </td>
           </tr>
           <tr>
           <td width="20"></td>
           <td >
                <asp:Button ID="btnCreateNewFile" runat="server"
                     Text=" 创建 "
                     OnClick="btnCreateNewFile_Click" 
                     CssClass="btn"
                     Width="60px"
                     ValidationGroup="NewFile"  />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                     ShowMessageBox="True" 
                     ShowSummary="False"
                     ValidationGroup="NewFile" 
                     />
                
                <input type="button" onclick="hidePanel('tbNewFile');" style="width:60px" class="btn" value=" 关闭 " />
                
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                    正在创建，请稍候...
                    </ProgressTemplate>
                </asp:UpdateProgress> 
                
                <br /><br /><br />
                 
           </td>
           </tr>
           </table>
           </ContentTemplate>
           </asp:UpdatePanel> 
        
    </td>
    </tr>   
    </table>
    </div>
    <!--创建新文件.END-->
    
    <!--创建新文件夹.Begin-->
    <div id="tbNewDir" style="width:300px;display:none;border:solid 1px #9DB3C5;" >
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
    <tr>
    <td style="background-image:url(App_Themes/Default/Images/bg1.jpg);">
        <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td style="font-weight:bold;padding-left:10px;height:30px;text-align:left;"><img src="App_Themes/Default/Images/icon_newfolder.gif" border="0" align="absmiddle" /> 新建文件夹</td>
        <td align="right" style="padding-right:10px;"><a href="javascript:hidePanel('tbNewDir'); "><img src="App_Themes/Default/Images/icon_close.gif" border="0" alt="关闭" /></a></td>
        </tr>
        </table>
    </td>
    </tr> 
    <tr>
    <td style="background-color:#f9f9f9;">    
        
           <asp:UpdatePanel ID="UpdatePanel2" runat="server">
           <ContentTemplate>
           <table cellpadding="3" cellspacing="0" border="0">
           <tr>
           <td height="30"></td>
           <td height="50">
               文件夹名：
               <asp:TextBox ID="txtNewDirName" runat="server"
                    Width="140px"
                    ValidationGroup="NewDir"
                    Text="新建文件夹" />
                    
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtNewDirName" 
                    Display="None" 
                    ErrorMessage="请输入文件夹名！" 
                    ValidationGroup="NewDir" />
               
               <asp:RegularExpressionValidator id="revUserPwd" runat="server"
	                EnableViewState="False" 
	                ErrorMessage="文件夹名中含有非法字符！"
	                Display="None"
	                ControlToValidate="txtNewDirName" 
                    ValidationExpression="[^\\|<>:*?/']{1,50}" 
	                ValidationGroup="NewDir" />
                
           </td>
           </tr>
           <tr>
           <td width="20"></td>
           <td>
                <asp:Button ID="btnCreateNewDirectory" runat="server"
                     Text=" 创建 "
                     OnClick="btnCreateNewDirectory_Click" 
                     CssClass="btn"
                     Width="60px"
                     ValidationGroup="NewDir"  />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" 
                     ShowMessageBox="True" 
                     ShowSummary="False"
                     ValidationGroup="NewDir" 
                     />
                
                <input type="button" onclick="hidePanel('tbNewDir');" style="width:60px" class="btn" value=" 关闭 " />
                     
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                    <ProgressTemplate>
                    正在创建，请稍候...
                    </ProgressTemplate>
                </asp:UpdateProgress> 
                
                <br /><br /><br />
                
           </td>
           </tr>
           </table>
           </ContentTemplate>
           </asp:UpdatePanel> 
        
    </td>
    </tr>   
    </table>
    </div>
    <!--创建新文件夹.END-->
    
    <!--重命名文件/文件夹.Begin-->
    <div id="tbRenameFS" style="width:300px;display:none;border:solid 1px #9DB3C5;">
    <table width="100%" align="center" cellpadding="0" cellspacing="0" style="">
    <tr>
    <td style="background-image:url(App_Themes/Default/Images/bg1.jpg);">
        <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td style="font-weight:bold;padding-left:10px;height:30px;text-align:left;"><img src="App_Themes/Default/Images/icon_newfile.gif" border="0" align="absmiddle" /> <span id="lblNote_RenameFS_Title" /></td>
        <td align="right" style="padding-right:10px;"><a href="javascript:hidePanel('tbRenameFS'); "><img src="App_Themes/Default/Images/icon_close.gif" border="0" alt="关闭" /></a></td>
        </tr>
        </table>
    </td>
    </tr> 
    <tr>
    <td style="background-color:#f9f9f9;">    
        
           <br />
           <div style="padding-left:30px;">原名称：<span id="lblNote_FS_OldName"></span></div>
               
           <asp:UpdatePanel ID="UpdatePanel3" runat="server">
           <ContentTemplate>
           <table cellpadding="3" cellspacing="0" border="0">
           <tr>
           <td width="20"></td>
           <td height="50">
               
               新名称：
               <asp:TextBox ID="txtFSName" runat="server"
                    Width="140px"
                    ValidationGroup="RenameFS"
                    />
                    
               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtFSName" 
                    Display="None" 
                    ErrorMessage="请输入新名称！" 
                    ValidationGroup="RenameFS" />
               
               <asp:RegularExpressionValidator id="RegularExpressionValidator2" runat="server"
	                EnableViewState="False" 
	                ErrorMessage="新名称中含有非法字符！"
	                Display="None"
	                ControlToValidate="txtFSName" 
	                ValidationExpression="[^\\|<>:*?/']{1,50}" 
	                ValidationGroup="RenameFS" />
               
               <!--通过一个隐藏表单来存储FS类型，以便于重命名-->
               <input type="hidden" name="fsTypeForRename" id="fsTypeForRename" />
               
               <input type="hidden" name="oldFSNameForRename" id="oldFSNameForRename" />

           </td>
           </tr>
           <tr>
           <td width="20"></td>
           <td >
                <asp:Button ID="btnRenameFS" runat="server"
                     Text=" 确定 "
                     OnClick="btnRenameFS_Click" 
                     CssClass="btn"
                     Width="60px"
                     ValidationGroup="RenameFS"  />
                <asp:ValidationSummary ID="ValidationSummary3" runat="server" 
                     ShowMessageBox="True" 
                     ShowSummary="False"
                     ValidationGroup="RenameFS"
                     />
                
                <input type="button" onclick="hidePanel('tbRenameFS');" style="width:60px" class="btn" value=" 取消 " />
                
                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                    正在创建，请稍候...
                    </ProgressTemplate>
                </asp:UpdateProgress> 
                
                <br /><br /><br />
                 
           </td>
           </tr>
           </table>
           </ContentTemplate>
           </asp:UpdatePanel> 
        
    </td>
    </tr>   
    </table>
    </div> 
    <!--重命名文件/文件夹.END-->
    
    <!--压缩.Begin-->
    <div id="zipPanel" style="width:300px;display:none;border:solid 1px #9DB3C5;">
    <table width="100%" align="center" cellpadding="0" cellspacing="0" style="">
    <tr>
    <td style="background-image:url(App_Themes/Default/Images/bg1.jpg);">
        <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td style="font-weight:bold;padding-left:10px;height:30px;text-align:left;"><img src="App_Themes/Default/Images/icon_zip.gif" border="0" align="absmiddle" /> 在线压缩文件/文件夹</td>
        <td align="right" style="padding-right:10px;"><a href="javascript:hidePanel('zipPanel'); "><img src="App_Themes/Default/Images/icon_close.gif" border="0" alt="关闭" /></a></td>
        </tr>
        </table>
    </td>
    </tr> 
    <tr>
    <td style="background-color:#f9f9f9;">    
                       
           <asp:UpdatePanel ID="UpdatePanel_For_Zip" runat="server">
           <ContentTemplate>
           <table cellpadding="3" cellspacing="0" border="0">
           <tr>
           <td width="20"></td>
           <td align="right">
           选择压缩格式
           </td>
           <td height="30">         
           
           <script type="text/javascript">
           function changeZipType()
           {
                var obj = document.forms[0].radio_zipType;
                var displayObj = document.getElementById("spanzipFileType");
                
                if (obj[0].checked == true)
                {
                    displayObj.innerHTML = ".zip";    
                }
                
                if (obj[1].checked == true)
                {
                    displayObj.innerHTML = ".rar";    
                }
                
                
           }
           </script>                    
           

           <input type="radio" name="radio_zipType" id="radio_zipType_zip" value="zip" onclick="changeZipType()" checked="checked" /><label for="radio_zipType_zip">zip</label>                    
           <input type="radio" name="radio_zipType" id="radio_zipType_rar" value="rar" onclick="changeZipType()" /><label for="radio_zipType_rar">rar</label>                    
           
           </td>
           </tr>
           <tr>
           <td width="20"></td>
           <td align="right">
           压缩后的文件名
           </td>
           <td height="30">                     
                <asp:TextBox ID="txtZipFileName" runat="server"
                     ValidationGroup="zip"
                     Width="80px"
                     /><span id="spanzipFileType">.zip</span>
                
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtZipFileName" 
                    Display="None" 
                    ErrorMessage="请输入压缩后的文件名！" 
                    ValidationGroup="zip" />
                                     
           </td>
           </tr>
           <tr>
           <td width="20"></td>
           <td height="30" colspan="2" align="center">
                
                <asp:Button ID="btnZip" runat="server"
                     Text=" 确定 "
                     OnClick="btnZip_Click"
                     CssClass="btn"
                     Width="60px"
                     ValidationGroup="zip"  />
                
                <input type="button" onclick="hidePanel('zipPanel');" style="width:60px" class="btn" value=" 取消 " />                                
                
                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel_For_Zip">
                    <ProgressTemplate>
                    <br />
                    正在处理，请稍候...
                    </ProgressTemplate>
                </asp:UpdateProgress> 
                
                <asp:ValidationSummary ID="ValidationSummary4" runat="server" 
                     ShowMessageBox="True" 
                     ShowSummary="False"
                     ValidationGroup="zip"
                     />   
                
                <br />  <br />                
                
           </td>
           </tr>
           </table>
           </ContentTemplate>
           </asp:UpdatePanel> 
        
    </td>
    </tr>   
    </table>
    </div> 
    <!--压缩.END-->
    
    <!--解压.Begin-->
    <div id="unzipPanel" style="width:300px;display:none;border:solid 1px #9DB3C5;">
    <table width="100%" align="center" cellpadding="0" cellspacing="0" style="">
    <tr>
    <td style="background-image:url(App_Themes/Default/Images/bg1.jpg);">
        <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td style="font-weight:bold;padding-left:10px;height:30px;text-align:left;"><img src="App_Themes/Default/Images/icon_zip.gif" border="0" align="absmiddle" /> 在线解压</td>
        <td align="right" style="padding-right:10px;"><a href="javascript:hidePanel('unzipPanel'); "><img src="App_Themes/Default/Images/icon_close.gif" border="0" alt="关闭" /></a></td>
        </tr>
        </table>
    </td>
    </tr> 
    <tr>
    <td style="background-color:#f9f9f9;">    
                       
           <asp:UpdatePanel ID="UpdatePanel_For_Unzip" runat="server">
           <ContentTemplate>
           <table cellpadding="3" cellspacing="0" border="0">
           <tr>
           <td width="20"></td>
           <td align="right">
           解压到
           </td>
           <td height="30">                     
           
           <asp:RadioButtonList ID="rbnUnzipLocation" runat="server"
                RepeatColumns="2"
                RepeatDirection="Horizontal"
                AutoPostBack="true"
                OnSelectedIndexChanged="rbnUnzipLocation_OnSelectedIndexChanged">
                <asp:ListItem Value="current" Selected="True">当前目录</asp:ListItem>
                <asp:ListItem Value="other">其它位置</asp:ListItem>
           </asp:RadioButtonList>
           
           </td>
           </tr>
           <tr id="panel_dirListForUnzip" runat="server" visible="false">
           <td width="20"></td>
           <td height="30" colspan="2" align="center">
                <asp:DropDownList ID="ddlDirListForUnzip" runat="server">
                </asp:DropDownList>
           </td>
           </tr>
           <tr>
           <td width="20"></td>
           <td height="30" colspan="2" align="center">
                
                <asp:Button ID="btnUnzip" runat="server"
                     Text=" 确定 "
                     OnClick="btnUnzip_Click"
                     CssClass="btn"
                     Width="60px"
                     ValidationGroup="unzip"  />
                
                <input type="button" onclick="hidePanel('unzipPanel');" style="width:60px" class="btn" value=" 取消 " />                                
                
                <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel_For_Unzip">
                    <ProgressTemplate>
                    <br />
                    正在处理，请稍候...
                    </ProgressTemplate>
                </asp:UpdateProgress> 
                
                <br /><br />
           </td>
           </tr>
           </table>
           </ContentTemplate>
           </asp:UpdatePanel> 
        
    </td>
    </tr>   
    </table>
    </div> 
    <!--压缩.END-->
    
    <!--右键菜单.Begin-->
    <table id="tbContextMenu" style="width:220px;display:none;" align="center" cellpadding="0" cellspacing="0" style="border:solid 1px #9DB3C5;background-color:#f6f9fd;">
    <tr>
    <td>
        <ul id="ul_contextMenu" style="display:none;">
        <li>
            <img src="App_Themes/Default/Images/fsAction/small/back.gif" border="0" align="absmiddle" /> <a href="javascript:window.history.go(-1);">返回</a>
            &nbsp;
            <img src="App_Themes/Default/Images/fsAction/small/fs_refresh.gif" border="0" align="absmiddle" /> <a href="javascript:refresh();">刷新</a>
            &nbsp;
            <img src="App_Themes/Default/Images/fsAction/small/up.gif" border="0" align="absmiddle" /> <a href="javascript:up();">向上</a>
        </li>
        <li><img src="App_Themes/Default/Images/fsAction/small/home.gif" border="0" align="absmiddle" /> <a href="javascript:backToHome();">回到根目录</a></li>
        <li><img src="App_Themes/Default/Images/fsAction/small/new_file.gif" border="0" align="absmiddle" /> <a href="javascript:newFile();">新建文件</a></li>
        <li><img src="App_Themes/Default/Images/fsAction/small/new_folder.gif" border="0" align="absmiddle" /> <a href="javascript:newDir();">新建文件夹</a></li>        
        <li><img src="App_Themes/Default/Images/fsAction/small/pas.gif" border="0" align="absmiddle" /> <a href="javascript:paste();">粘贴</a>（ctrl + v）</li>
        </ul>
        
        <ul id="ul_contextMenu2" style="display:none;">     
        <li><img src="App_Themes/Default/Images/fsAction/small/copy.gif" border="0" align="absmiddle" /> <a href="javascript:copy();">复制</a>（ctrl + c）</li>        
        <li><img src="App_Themes/Default/Images/fsAction/small/cut.gif" border="0" align="absmiddle" /> <a href="javascript:cut();">剪切</a>（ctrl + x）</li>
        <li><img src="App_Themes/Default/Images/fsAction/small/pas.gif" border="0" align="absmiddle" /> <a href="javascript:paste();">粘贴</a>（ctrl + v）</li>
        <li><img src="App_Themes/Default/Images/fsAction/small/delete.gif" border="0" align="absmiddle" /> <a href="javascript:deleteFS();">删除</a>（delete）</li>
        <li><img src="App_Themes/Default/Images/fsAction/small/zip.gif" border="0" align="absmiddle" /> <a href="javascript:zip();">压缩</a>（ctrl + z）</li>
        <li><img src="App_Themes/Default/Images/fsAction/small/unzip.png" border="0" align="absmiddle" /> <a href="javascript:unzip();">解压</a>（ctrl + u）</li>
        </ul>  
      
    </td>
    </tr> 
    </table>
    <!--右键菜单.End-->
        
</div><!--"Content".END-->

</form>
</body>
</html>