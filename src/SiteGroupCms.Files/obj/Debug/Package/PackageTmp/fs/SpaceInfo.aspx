<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpaceInfo.aspx.cs" Inherits="MSManager.Web.SpaceInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<asp:Literal ID="title" runat="server">空间使用信息</asp:Literal>
<style type="text/css">
.table-border-1px
{
   border: 1px solid #cccccc;
   border-collapse:collapse;
}

.td-border-1px
{
   border: 1px solid #cccccc;
}
</style>
</head>
<body>
<table style="width:99%;margin-top:1px;" align="center" cellpadding="0" cellspacing="0" class="table-border-1px" >
<tr>
<td style="background-color:#f5f5f5;font-weight:bold;padding-left:20px;height:32px;text-align:left;">空间信息</td>
</tr> 
<tr>
<td style="background-color:#f9f9f9;" class="td-border-1px">    
    
    <div style="padding:10px;text-align:left;">
    
    <table cellpadding="2" cellspacing="0" style="border:solid 1px #cccccc;background-color:#ffffff;height:18px;display:none;" width="100%">    
    <tr>        
        <asp:Literal ID="literal1" runat="server" EnableViewState="false" Visible="false" />
    </tr>
    </table>
    <div style="line-height:170%;">
    <asp:Literal ID="literalSpaceCapacity" runat="server" EnableViewState="false" Visible="false" /> <br />
    已用：<asp:Literal ID="literalSpaceUsedInfo" runat="server" EnableViewState="false" /> <br />
   <asp:Literal ID="literalSpaceRemanent" runat="server" EnableViewState="false" Visible="false" /> <br />
    <% 
        int totalFSObjCount = 0;
        int totalFileCount = 0;

        base.GetFSObjCountInfo(ref totalFSObjCount, ref totalFileCount);

        if (base.FSObjIsTooMuch)
        {
            Response.Write("文件总数：至少" + totalFileCount.ToString() + " 个");
            Response.Write("<br />");
            Response.Write("文件夹总数：至少" + (totalFSObjCount - totalFileCount).ToString() + " 个");
        }
        else
        {
            Response.Write("文件总数：" + totalFileCount.ToString() + " 个");
            Response.Write("<br />");
            Response.Write("文件夹总数：" + (totalFSObjCount - totalFileCount).ToString() + " 个");
        }
    %>
    
    </div>
    </div>       
    
</td>
</tr>   
</table>

<br />

<table style="width:99%;" align="center" cellpadding="0" cellspacing="0" class="table-border-1px">
<form id="form1" runat="server">
<tr>
<td style="background-color:#f5f5f5;font-weight:bold;padding-left:20px;height:32px;text-align:left;">在当前目录下搜索</td>
</tr> 
<tr>
<td style="background-color:#f9f9f9;" class="td-border-1px">  
    
   <table cellpadding="3" cellspacing="0" border="0">
   <tr>
   <td height="30"><img src="App_Themes/Default/Images/search.gif" align="absmiddle" /></td>
   <td>
       <asp:TextBox ID="txtFileName" runat="server"
            Width="140px"
            />
       
       <div style="display:none;">
       <asp:TextBox ID="TextBox1" runat="server"
            Width="140px"
            />
       </div>
                           
       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
             ControlToValidate="txtFileName" 
             Display="None" 
             ErrorMessage="请输入要搜索的文件名！"
             />
   </td>
   </tr>
   <tr>
   <td height="30" colspan="2">
        <asp:Button ID="btnSearch" runat="server"
             Text=" 搜索 "
             OnClick="btnSearch_Click" 
             CssClass="btn" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
             ShowMessageBox="True" 
             ShowSummary="False"
             />
   </td>
   </tr>
   </table>
    
</td>
</tr>   
</form>
</table>

</body>
</html>
