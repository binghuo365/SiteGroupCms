<%@ Page Language="C#" AutoEventWireup="true" Inherits="Upload" Codebehind="Upload.aspx.cs" %>
<%@ Register Assembly="chaokers.cn.FSManager.Controls" Namespace="FSManager.Controls" TagPrefix="fs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<asp:Literal ID="title" runat="server">文件管理/上传文件</asp:Literal>
<link href="App_Themes/Default/css/style.css" type="text/css" rel="stylesheet" />
<link href="App_Themes/Default/css/navbar.css" type="text/css" rel="stylesheet" />
<style type="text/css">
.note1
{
    width:100%;
    border-bottom:solid 1px #cccccc;    
    background-color:#f5f5f5;
}
.note1 p
{
    padding:6px;
    padding-left:10px;
    color:#174B73;
    margin:0px;
}
</style>
<script language="javascript">
function CreateObjects()
{
	str='';
	if(!document.Form1.upcount.value)
		document.Form1.upcount.value=1;
    var fileCnt = parseInt(document.Form1.upcount.value);	
    if (fileCnt > 100)
    {
        alert("每次最多只能上传100个文件！");
        return;
    }
		
 	for(i=1;i<=document.Form1.upcount.value;i++)
	    str+='<input type="file" name="file1" id="file1" class="input_txt" size="68" maxlength="100"><br>';
	document.all.upid.innerHTML=str+'<br>';
}

function CheckForm()
{	
	var objFile = document.Form1.upFileObj.value;		
	if(objFile.length == 0)	
	{
		alert('请指定要上传的文件！');		
		return false;
	}	
	
	openProgress();
}
</script>
</head>
<body>
<form id="Form1" method="post" runat="server" enctype="multipart/form-data">
<div id="content">

    <fs:NavBar ID="NavBar1" runat="server" ConfigXmlPath="ShellConfig/Navbar.xml" ModuleName="File" />    	
	
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
	
	<fs:CurrLocation ID="CurrLocation1" runat="server" InnerText="文件管理/上传文件" Height="17px">
    </fs:CurrLocation>
	
	<!--容器-->
	<fs:Container ID="Container1" runat="server">
	

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
	
    
    <div id="divShowUpload" runat="server">
    <!--main.主表.begin-->
    <fs:PagePart ID="pagePart1" runat="server" HeaderText="（普通上传）" BodyHorizontalAlign="left">
        
        <div class="note1"><p>说明：1）每次可上传的文件总大小为4M；2）每次可上传多个文件；</p></div>
        
        <div style="padding-left:10px;">
        <table class="editTable" width="100%" cellpadding="3" cellspacing="0" border="0">  
        <tr><td colspan="4" style="height:10px;"></td></tr>
        <tr>        
        <td>
            需上传的文件个数 : <input name="upcount" type="text" class="input_txt" value="1" size="7">
	        <input name="Button" type="button" class="btn" onClick="CreateObjects();" value=" 设 定 ">
        </td>
        </tr>   
        <tr>        
        <td>     
            <div id="upid">
            <input type="file" name="file1" id="file1"  class="input_txt" size="68" maxlength="100">
            </div>
        </td>        
        </tr>
        <tr>        
        <td align="left">
            
            <br />
		    <asp:Button ID="btnUpload" Runat="server" OnClick="btnUpload_Click" CssClass="btn" Text=" 上 传 " />
		    <asp:Label ID="lblResult" runat="server"></asp:Label>&nbsp;</span>		    
		    <input type="button" value=" 返 回 " class="btn" onClick="window.location='Route.aspx?SubPath=<% = Server.UrlEncode(base.subPath) %>';" />
	        &nbsp;&nbsp; 
        </td>
        </tr>           
        </table>
        </div>
        <br />
        
    </fs:PagePart>
  
    <br />
    
    <fs:PagePart ID="pagePart2" runat="server" HeaderText="上传方案2（网页版大文件上传）" BodyHorizontalAlign="left" Visible="false">    
        
        <div class="note1"><p>说明：1）上传文件大小可达50M；2）每次可上传一个文件；3）需要您开通网页版大文件上传功能；</p></div>
        
        <div style="padding:20px;">
        <asp:Literal ID="literalForBigFileUploadByWeb" runat="server"></asp:Literal>
        </div>                
        
    </fs:PagePart>
        
          
    </div> 
    
    
    
    </fs:Container>
    
    <br /><br /><br /><br /><br /><br /> 
	
</div>
</form>
</body>
</html>
