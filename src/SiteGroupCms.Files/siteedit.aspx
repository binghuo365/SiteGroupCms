<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="siteedit.aspx.cs" Inherits="SiteGroupCms.siteedit" %>
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
   
   
 <div style="height:70px;">
 </div>    
<script>

     //当前ID
       var id = getQueryStringByName("id")==""?"0":getQueryStringByName("id");
        //是否新增状态
        var isAddNew = getQueryStringByName("method")=="add";
        //是否编辑状态
        var isEdit = getQueryStringByName("method")=="update";
        //添加的栏目
        var issuper=getQueryStringByName("issuper")=="super";

 //覆盖本页面grid的loading效果
        LG.overrideGridLoading(); 
        
//表单底部按钮 
        LG.setFormDefaultBtn(f_cancel,f_save);
 //创建表单结构
  var groupicon="../lib/icons/32X32/communication.gif";
        var mainform = $("#mainform");  
        mainform.ligerForm({ 
         fields : [
         {name:"siteid",type:"hidden"},
         {display:"站点名称",name:"title",newline:true,labelWidth:100,width:200,space:30,type:"text",validate:{required:true,maxlength:60}, group:"基本信息",groupicon:"../lib/icons/32X32/communication.gif"},
          {display:"前台显示名",name:"webtitle",newline:false,labelWidth:100,width:200,space:30,type:"text",validate:{required:true,maxlength:60}, groupicon:groupicon},
           {display:"位置目录",name:"location",newline:true,labelWidth:100,width:200,space:30,validate:{required:true,maxlength:60}, type:"text",groupicon:groupicon},  
         {display:"域名",name:"domain",newline:false,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon},    
          {display:"是否启用",name:"isworkwy",newline:true,labelWidth:100,width:200,space:30,validate:{required:true,maxlength:60},type:"select",comboboxName:"iswork",
          options:{ data :[{id:1,text:'启用'},{id:0,text:'不启用'}],validate:{required:true,maxlength:60}, checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
           {display:"关键字",name:"keywords",newline:false,labelWidth:100,width:200,space:30,type:"text", groupicon:groupicon},
          {display:"简介",name:"description",newline:true,labelWidth:100,width:535,space:30,type:"textarea", groupicon:groupicon},
         
          {display:"首页模板",name:"sy",newline:true,labelWidth:100,width:200,space:30,group:"模板信息",groupicon:groupicon,validate:{required:false,maxlength:60},type:"select",comboboxName:"indextemplate",
          options:{ url :'ajaxhandler/select.aspx?view=templatelist&type=index',checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
           {display:"栏目页模板",name:"lm",newline:false,labelWidth:100,width:200,space:30,groupicon:groupicon,validate:{required:false,maxlength:60},type:"select",comboboxName:"listtemplate",
          options:{ url :'ajaxhandler/select.aspx?view=templatelist&type=list',checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
           {display:"内容页模板",name:"nr",newline:true,labelWidth:100,width:200,space:30,groupicon:groupicon,validate:{required:false,maxlength:60},type:"select",comboboxName:"contenttemplate",
          options:{ url :'ajaxhandler/select.aspx?view=templatelist&type=content',checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
         
       {display:"FTP服务器",name:"ftpserver",newline:true,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon,group:"FTP信息"},  
              {display:"FTP端口",name:"ftpport",newline:false,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon},  
               {display:"FTP登录名",name:"ftpuser",newline:true,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon},  
                {display:"FTP密码",name:"ftppwd",newline:false,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon}, 
                 {display:"远程目录",name:"ftpdir",newline:true,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon}  ,
          {display:"邮箱服务器",name:"emailserver",newline:true,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon,group:"邮箱信息"},  
           {display:"邮箱用户名",name:"emailuser",newline:false,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon},  
            {display:"邮箱密码",name:"emailpwd",newline:true,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon}
             
 ],
		 toJSON:JSON2.stringify
        });
        
         var actionRoot = "ajaxhandler/siteeditdo.aspx";
        if(isEdit&&issuper){ //不是超级编辑 显示部分功能
           // $("#artid").attr("readonly", "readonly").removeAttr("validate");
            mainform.attr("action", actionRoot + "?method=update&id="+id); 
        }
        else if(isEdit&&!issuper) //显示所有功能
           {
            mainform.attr("action", actionRoot + "?method=update&id="+id); 
           }
        if (isAddNew) {//新增
            mainform.attr("action", actionRoot + "?method=add");
            if(catalogid.toString().indexOf("site")<0)//站点id
            $("#catalogid").val(catalogid);
        }
        else { //编辑
            LG.loadForm(mainform, { type: 'ajaxpersoninfo', method: 'GetSiteinfo', data: { ID: id} },f_loaded);
        }  
        
        
            //验证
            jQuery.metadata.setType("attr", "validate"); 
            LG.validate(mainform);
         
		function f_loaded()
        {
           /// if(!isView) return; 
            //$("input,select,textarea",mainform).attr("readonly", "readonly");
        }
        function f_save()
        {
         jQuery.metadata.setType("attr", "validate"); 
            LG.validate(mainform);
            
            LG.submitForm(mainform, function (data) {
                var win = parent || window;
                if (data.IsError) {  
                    LG.showError('错误:' + data.Message);
                   
                }
                else { 
                    LG.showSuccess('保存成功', function () { 
                           var win = parent||window;
                    win.LG.closeAndReloadParent(null, "sitelist");
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