<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="catalogedit.aspx.cs" Inherits="SiteGroupCms.catalogedit" %>

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
    
<script>

     //当前ID
        var fid = getQueryStringByName("fatherid")==""?0:getQueryStringByName("fatherid");
        var id= getQueryStringByName("id")==""?0:getQueryStringByName("id");
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
         {name:"catalogid",type:"hidden"},
         {display:"栏目名称",name:"title",newline:true,labelWidth:100,width:200,space:30,type:"text",validate:{required:true,maxlength:60}, group:"基本信息",groupicon:groupicon},
          { display: "栏目ID", name: "id", newline: false, labelWidth: 100, width: 200, space: 30, type: "text", groupicon: groupicon },    
         {display:"栏目关键字",name:"keywords",newline:true,labelWidth:100,width:200,space:30,type:"text",validate:{required:false,maxlength:60}, groupicon:groupicon},
        { display: "目录名", name: "dirname", validate:{required:true,maxlength:60}, newline: false, labelWidth: 100, width: 200, space: 30, type: "text", groupicon: groupicon },    
         {display:"父级栏目",name:"ffa",newline:true,labelWidth:100,width:200,space:30,groupicon:groupicon,type:"select",comboboxName:"fathercatalog",
          options:{ url :'ajaxhandler/select.aspx?view=cataloglist',checkbox:false,nodeWidth :220,isMultiSelect: false}},
         { display: "是否共享", name: "isshare", newline: false, labelWidth: 100, width: 200, space: 30, type: "checkbox", groupicon: groupicon },
         {display:"简介",name:"description",newline:true,labelWidth:100,width:535,space:30,type:"textarea",groupicon:groupicon},
         {display:"栏目页模板",name:"lm",newline:true,labelWidth:100,width:200,space:30,groupicon:groupicon,group:"模板信息",validate:{required:true,maxlength:60},type:"select",comboboxName:"listtemplate",
          options:{ url :'ajaxhandler/select.aspx?view=templatelist&type=list',checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
         {display:"内容页模板",name:"nr",newline:false,labelWidth:100,width:200,space:30,groupicon:groupicon,validate:{required:true,maxlength:60},type:"select",comboboxName:"contenttemplate",
          options:{ url :'ajaxhandler/select.aspx?view=templatelist&type=content',checkbox:false,nodeWidth :220,isMultiSelect: false}}
 ],
		 toJSON:JSON2.stringify
        });
        
         var actionRoot = "ajaxhandler/catalogeditdo.aspx";
        if(isEdit){
            $("#dirname").attr("readonly", "readonly");
            $("#id").attr("readonly", "readonly");
            mainform.attr("action", actionRoot + "?method=update&id="+id); 
        }
        if (isAddNew) {//新增
            mainform.attr("action", actionRoot + "?method=add");
            $("#id").attr("value", "自动生成");
            $("#id").attr("readonly", "readonly");
            if(catalogid.toString().indexOf("site")>=0)//是跟站点
            $("#fathercatalog_val").val(0);
            else
            $("#fathercatalog_val").val(fid);
        }
        else { //编辑
            LG.loadForm(mainform, { type: 'ajaxpersoninfo', method: 'GetCataloginfo', data: { ID: id} },f_loaded);
        }  
        
        
            //验证
            jQuery.metadata.setType("attr", "validate"); 
            LG.validate(mainform);
         
		function f_loaded()
        {
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
                          //大规模刷新
                          top.location.href="default.aspx";
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
</form>
</body>
</html>

