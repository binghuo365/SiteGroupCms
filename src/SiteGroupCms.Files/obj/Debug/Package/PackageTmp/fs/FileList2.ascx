<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileList2.ascx.cs" Inherits="MSManager.Web.FileList2" %>
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
<script language="javascript" src="Js/SetButtonBehavior.js" type="text/javascript"></script>
<style type="text/css">
#tbMenu td{ width:60px; }
</style>
<asp:Literal ID="copyright" runat="server"></asp:Literal>
</head>

<body style="font-family:宋体;margin:0px;" onLoad="InitHtml();" onselect="document.selection.empty();" onselectstart="return true">

<script type="text/javaScript">
var subPath = "<% = base.subPath %>";
var processPage = "<% = base.pageName %>";
var userID = "<% = base.userID %>";
var editableFileTypeStr = "<% = FSManager.Components.Format.GetEditableFileTypeArrayStr()%>";
var _fsListColNum = "<% = base.myDataList.RepeatColumns%>";

<%
    string lastLevelFolder = String.Empty;
    if(base.subPath == String.Empty || base.subPath == "/")
    {
	    lastLevelFolder = "/";
    }
    else
    {
	    lastLevelFolder = "/.../" + base.subPath.Remove(0,base.subPath.LastIndexOf("/") + 1);
    }
%>
var _lastLevelFolder = "<% = lastLevelFolder %>";

var inputBoxForNewFSName = "<% = txtFSName.ClientID %>";
var txtZipFileName = "<% = txtZipFileName.ClientID %>"; 

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
	keyboardEventHandlerWithCtrlKey(event.keyCode);
}
</script> 

<form id="aspnetform" method="post" runat="server" style="margin:0px;padding:0px;">

<div id="content" style="margin:0px;padding-top:0px;">

    <fs:FSOprSupport_2 ID="FSOprSupport_21" runat="server" />
    <fs:FSOprSupport_2_ForKeyboardEvents ID="FSOprSupport_2_ForKeyboardEvents1" runat="server" />
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <fs:NavBar ID="NavBar1" runat="server" ConfigXmlPath="ShellConfig/Navbar.xml" ModuleName="File" />
    
    <fs:CurrLocation ID="CurrLocation1" runat="server">
        <div class="currLocation-TbContainer">
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
        <td>当前位置：文件管理/<%=base.currPosition%></td>
        <td align="right"><img src="App_Themes/Default/Images/icon_liebiao.gif" border="0" align="absmiddle" /> <a href="Switcher.aspx?Style=1&SubPath=<% = Server.UrlEncode(base.subPath) %>">切换到列表方式浏览</a></td>
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

    <!--Action menu.Start-->
    <table height="56" border="0" id="tbMenu">
    <tr>
    <td>
      <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><asp:ImageButton id="UpBtn" runat="server" ToolTip="向上一级" OnClick="Act" CommandName="ToPrevDirectory" ImageUrl="App_Themes/Default/Images/fsAction/up.gif" /></asp:ImageButton>
      </div>
    </td>
    <td>
	    <div align="center" class="defaultBtn" onmouseup="button_up(this);" onmousedown="button_down(this);" onmouseover="button_over(this);" onmouseout="button_out(this);"><a href="<% = base.pageName %>" onfocus="blur();" ><IMG alt="根目录" src="App_Themes/Default/Images/fsAction/home.gif" border="0" /></a></div>
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
    
    <table cellpadding="0" cellspacing="0" width="100%"><tr><td height="1" bgcolor="#98b1c8"></td></tr></table>
    
    <br />
    
    <table cellpadding="0" cellspacing="0" width="100%" border="0"  style="margin-left:0px;">
    <tr>
    <td valign="top">
    
        <!--MainBlock.Start-->
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
        <!--MainBlock.Left.Start-->
        <td width="2"></td>
        <td style="display:none;" width="165" valign="top" align="center" onClick="javascript:UnSelectAll();" bgcolor="#f8f8f8">
        	
	        <table width="98%" cellpadding="2" border="0">	
	        <tr>
	        <td align="left" style="WORD-BREAK: break-all;">
		        <span id="spanFsName"></span>
		        <span id="spanFsType"></span>
		        <span id="spanFsSize"></span>
		        <span id="spanLastUpdate"></span>
	        </td>
	        </tr>
	        <tr height="10px;"><td></td></tr>
	        <tr bgcolor="#cccccc"><td></td></tr>
	        <tr height="10px;"><td></td></tr>
	        <tr>
	        <td align=left>
	            <asp:Label id="lblInfo" runat="server"></asp:Label> <br />
		        <span id="spanSelectedObjCount"></span>
	        </td>
	        </tr>
	        </table>
	        <br>
	        <table width="98%" cellpadding="3" border="0" bgcolor="#f0f0f0" cellspacing="1" style="color:#bbbbbb;">	
	        <tr bgcolor="#ffffff">
	        <td colspan="2"><b>文件操作说明:</b></td>	
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td align="left" colspan="2">双击下载文件(打开文件夹)</td>
	        </tr>		
	        <tr bgcolor="#ffffff">
	        <td align="left" colspan="2">右击可重命名、编辑文件</td>
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td align="left" colspan="2">可鼠标+Ctrl(Shift)键多选</td>
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td align="left" colspan="2">可用光标键选择单个文件</td>
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td align="left" colspan="2">可用Ctrl+光标键多选文件</td>
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td align="left" colspan="2">在左侧空白处单击取消选择</td>
	        </tr>								
	        <tr bgcolor="#ffffff">
	        <td colspan="2"><b>快捷键说明:</b></td>	
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td width="30%" align="left">全 选</td><td>Ctrl + A</td>
	        </tr>	
	        <tr bgcolor="#ffffff">
	        <td align="left">复 制</td><td>Ctrl + C</td>
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td align="left">剪 切</td><td>Ctrl + X</td>
	        </tr>	
	        <tr bgcolor="#ffffff">
	        <td align="left">粘 贴</td><td>Ctrl + V</td>
	        </tr>		
	        <tr bgcolor="#ffffff">
	        <td align="left">删 除</td><td>Delete</td>
	        </tr>	
	        <tr bgcolor="#ffffff">
	        <td align="left">压 缩</td><td>Ctrl + Z</td>
	        </tr>
	        <tr bgcolor="#ffffff">
	        <td colspan="2">注:只有当本页获得焦点后以上操作才有效(在本页任意位置单击即可获得焦点).</td>	
	        </tr>
	        </table>
        </td>
        <!--MainBlock.Left.End-->

        <!--MainBlock.Left.FsList.Start-->
        <td valign="top">
        
            <asp:DataList ID="myDataList" Runat="server"
	             EnableViewState="False" 
	             RepeatColumns="5"
	             RepeatDirection=Horizontal
	             RepeatLayout=Table	 
	             ItemStyle-HorizontalAlign=Center
	             ItemStyle-VerticalAlign=Top
	             Width="100%"
	             >
            <ItemTemplate>
            <div align="center" style="margin:0px 0px 5px 0px;" onmouseout="HideContextMenu('menuTB');">
                <div id="div_<%# Container.ItemIndex%>" onDblClick="OpenObject('<%# ((DataRowView)Container.DataItem)["FsName"].ToString()%>','<%# ((DataRowView)Container.DataItem)["FsType"].ToString()%>','<%# Server.UrlEncode(((DataRowView)Container.DataItem)["FsLink"].ToString().Replace("\\","/").ToString())%>','<%# ((DataRowView)Container.DataItem)["FsSize"].ToString()%>');" onMouseDown="KeyUp(this,<%# Container.ItemIndex.ToString()%>,'<%# ((DataRowView)Container.DataItem)["FsName"].ToString()%>','<%# ((DataRowView)Container.DataItem)["FsType"].ToString()%>','<%# ((DataRowView)Container.DataItem)["FsLink"].ToString().Replace("\\","/")%>','<%# Format.FormatFileLength(((DataRowView)Container.DataItem)["FsType"].ToString(),((DataRowView)Container.DataItem)["FsSize"].ToString())%>','<%# ((DataRowView)Container.DataItem)["LastUpdate"].ToString()%>');" style="width:80px;height:60px;border:#eeeeee 1px solid;VERTICAL-ALIGN: middle;text-align:center;PADDING-TOP:20px;PADDING-LEFT: 0px; PADDING-BOTTOM: 0px; MARGIN: 0px;background-repeat:no-repeat;background-position:center center;background-image:url('App_Themes/Default/Images/filetype2/<%# Format.GetFsTypeIcon_2(((DataRowView)Container.DataItem)["FsType"].ToString())%>.gif');"></div>
                <div style="margin-top:3px;width:90px;WORD-BREAK: break-all;">
	                <input type="checkbox" style="display:none;" name="chkBox" value="<%# Container.ItemIndex%>|<%# ((DataRowView)Container.DataItem)["FsType"]%>|<%# ((DataRowView)Container.DataItem)["FsSize"]%>">
	                <input type="hidden" name="fsName<%# Container.ItemIndex%>" value="<%# ((DataRowView)Container.DataItem)["FsName"]%>">	    
	                <%# base.ShowFsName(base.serverDomainToVisitUserSpace, base.userID, ((DataRowView)Container.DataItem)["FsLink"].ToString(), ((DataRowView)Container.DataItem)["FsType"].ToString(), ((DataRowView)Container.DataItem)["FsName"].ToString(), ((DataRowView)Container.DataItem)["FsLink"].ToString())%>
                </div>
            </div>	
            </ItemTemplate>
            </asp:DataList>
            <asp:Label ID="lblHidden" Runat="server" EnableViewState="False"></asp:Label>
            <input type="hidden" name="chkNum" value="<%=chkNum%>" />
            <a name="bottom"></a>
            
            
            <table width="100%" border="0" id="tbFileFooter" runat="server">
            <tr class="table_tr ">
	            <td height="28">&nbsp;<asp:Label id="lblInfo2" runat="server" Width="432" Height="18"></asp:Label></td>
            </tr>
            </table>

        </td>
        <!--MainBlock.Left.FsList.End-->
        </tr>

        </table>
        <!--MainBlock.End-->
        
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
            <span style="font-size:14px;"><a href="javascript:quickCloseSpaceInfoPanel();">[快速关闭右侧栏→]</a></span>&nbsp;
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
    

    <!--右键菜单.Start-->     
    <div id="menuTB" style="display:none;position:absolute;" onMouseOver="this.style.display='block'" onClick="this.style.display='none'" onMouseOut="this.style.display='none'" style="width:380px;display:none;border:solid 1px #9DB3C5;" >
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
    <tr>
    <td style="background-image:url(App_Themes/Default/Images/bg1.jpg);">
        <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td style="font-weight:bold;padding-left:10px;height:30px;text-align:left;WORD-BREAK: break-all;"><span id="spanIconForOpr"></span>操作</td>
        <td align="right" style="padding-right:10px;"><a href="javascript:hidePanel('tbNewDir'); "><img src="App_Themes/Default/Images/icon_close.gif" border="0" alt="关闭" /></a></td>
        </tr>
        </table>
    </td>
    </tr> 
    <tr>
    <td style="background-color:#f9f9f9;">          
        <div style="padding:10px;">

        <div><span id="tdFsName"></span></div>
        
        <br />
        
        <span id="trFsSize"></span>
        <span id="tdFsSize"></span>
        <span id="tdUpdate"></span>&nbsp;  
        
        <br /><br />
        
        <span id="spanRename"></span>&nbsp;
        <span id="spanZip"></span>
        <span id="spanUnZip"></span>
        <span id="spanCopy"></span>&nbsp;
        <span id="spanCut"></span>&nbsp;
        <span id="spanDelete"></span>&nbsp;
        <span id="spanEdit"></span>
        
        </div>
    </td>
    </tr>   
    </table>
    </div>
    <!--右键菜单.End-->
    
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
                     Width="80px" /><span id="spanzipFileType">.zip</span>
                
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

</div><!--"Content".END-->

</form>
</body>
</html>