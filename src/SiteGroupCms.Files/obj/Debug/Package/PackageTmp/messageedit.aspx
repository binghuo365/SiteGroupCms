<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="messageedit.aspx.cs" Inherits="SiteGroupCms.messageedit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
     <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <script src="../lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>  
    <link href="../lib/css/common.css" rel="stylesheet" type="text/css" />  
    <script src="../lib/js/common.js" type="text/javascript"></script>    
    <script src="../lib/js/LG.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.validate.min.js" type="text/javascript"></script> 
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../lib/js/validator.js" type="text/javascript"></script>
    <script src="../lib/js/ligerui.expand.js" type="text/javascript"></script> 
</head>
<body>

<form id="mainform" method="post">
</form>
     
<script>
        var groupicon="../lib/icons/32X32/communication.gif";
         var  fields=[
         {display:"公告标题",name:"title",newline:true,labelWidth:100,width:300,space:30,type:"text",validate:{required:true,maxlength:60}, group:"基本信息",groupicon:groupicon},  
         {display:"公告内容",name:"content",newline:true,labelWidth:100,width:300,space:30,type:"textarea",validate:{required:true,maxlength:1000}, groupicon:groupicon},
         
 ];
 //覆盖本页面grid的loading效果
        LG.overrideGridLoading(); 
//表单底部按钮 
        LG.setFormDefaultBtn(f_cancel,f_save);
 //创建表单结构
 
        var mainform = $("#mainform");  
        mainform.ligerForm({ 
         fields : fields,
		 toJSON:JSON2.stringify
        });
        
         var actionRoot = "ajaxhandler/messageeditdo.aspx";
            mainform.attr("action", actionRoot + "?method=add");
            
            //验证
            jQuery.metadata.setType("attr", "validate"); 
            LG.validate(mainform);   
		function f_loaded()
        {

        }
        function f_save()
        {

            LG.submitForm(mainform, function (data) {
                var win = parent || window;
                if (data.IsError) {  
                    LG.showError('错误:' + data.Message);
                   
                }
                else { 
                    LG.showSuccess('保存成功', function () { 
                     var win = parent || window;
                    win.LG.closeAndReloadParent(null, "messagelist");
                    });
                }
            });
        }
        function f_cancel()
        {
            var win = parent||window;
             win.LG.closeCurrentTab(null);
          
        }

</script>
</body>
</html>

