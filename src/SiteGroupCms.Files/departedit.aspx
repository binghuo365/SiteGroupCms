<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="departedit.aspx.cs" Inherits="SiteGroupCms.departedit" %>

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
    <style>
        #temcontent{height:500px; margin-bottom:50px;}
    </style>
</head>
<body>

<form id="mainform" method="post">

</form>
     
<script>

     //当前ID
        var did = getQueryStringByName("id");
        //是否新增状态
        var isAddNew = getQueryStringByName("method")=="add";
        //是否编辑状态
        var isEdit = getQueryStringByName("method")=="update";


 //覆盖本页面grid的loading效果
        LG.overrideGridLoading(); 
//表单底部按钮 
        LG.setFormDefaultBtn(f_cancel,f_save);
 //创建表单结构
  var groupicon="../lib/icons/32X32/communication.gif";
        var mainform = $("#mainform");  
        mainform.ligerForm({ 
         fields : [
         {name:"deptid",type:"hidden"},
       {display:"部门名称",name:"name",newline:true,labelWidth:100,width:200,space:30,type:"text",validate:{required:true,maxlength:60},groupicon:groupicon,group:"部门信息"},  
       {display:"部门简介",name:"description",newline:true,labelWidth:100,width:200,space:30,type:"textarea",groupicon:groupicon}          
 ],
		 toJSON:JSON2.stringify
        });
        
         var actionRoot = "ajaxhandler/departeditdo.aspx";
        if(isEdit){ //b编辑
           // $("#artid").attr("readonly", "readonly").removeAttr("validate");
            mainform.attr("action", actionRoot + "?method=update"); 
        }

        if (isAddNew) {//新增
            mainform.attr("action", actionRoot + "?method=add");
        }
        else { //编辑
            LG.loadForm(mainform, { type: 'ajaxdepart', method: 'GetDepart', data: { ID: did} },f_loaded);
        }  

            //验证
            jQuery.metadata.setType("attr", "validate"); 
            LG.validate(mainform);
        
		function f_loaded()
        {
           /// if(!isView) return; 
            //查看状态，控制不能编辑
           // alert($("#content1").attr("value"));
           //editor.util.getData('content1')="fdfd";
            //editor.val("fdf");
           // alert(editor);
           // editor.sync();
            //$("input,select,textarea",mainform).attr("readonly", "readonly");
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
                    var win = parent||window;
                    win.LG.closeAndReloadParent(null, "deptlist");
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
