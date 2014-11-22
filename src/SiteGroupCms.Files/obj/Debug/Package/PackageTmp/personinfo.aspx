<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="personinfo.aspx.cs" Inherits="SiteGroupCms.personinfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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

   

     
 <style type="text/css">
	#space {height:10px; width:800px; float:none;}
	.pic{float:none; width:800px; text-indent:5px; margin-top:5px;margin-bottom:10px;}
    .atts{float:none; width:100px;text-indent:5px; margin-top:5px;margin-bottom:10px; width:100px;  }
    #imglists{ width:800px; height:auto; float:none; margin-left:100px;}
    #imglists img{  margin-right:10px;}
    #attslist{width:auto; height:auto; float:left; margin-left:10px; margin-top:10px;}
    #ulattslist{ list-style-position:inside; height:auto;}
    #formcontent{width:100%; height:1040px;}
    a:hover{color:Red;}
    
	</style>
</head>
<body>

<form id="mainform" method="post">


</form>
<div style="clear:both;  width:800px; display:none;">
  <div class="l-group l-group-hasicon">
  &nbsp; &nbsp;
<img src="../lib/icons/32X32/communication.gif" style="height:16px; width:16px;"> 照片<span>信息</span>
</div>
 
</div>
     
<div style="clear:both;height:50px;">
</div>
      
       
<script>

     //当前ID
        var id = getQueryStringByName("id")==""?"0":getQueryStringByName("id");
        //是否新增状态
        var isAddNew = getQueryStringByName("method")=="add";
        //是否编辑状态
        var isEdit = getQueryStringByName("method")=="update";
        var isView=getQueryStringByName("method")=="view";
        //添加的栏目
        var issuper=getQueryStringByName("issuper")=="super";

 //覆盖本页面grid的loading效果
        LG.overrideGridLoading(); 
        
//表单底部按钮 
        LG.setFormDefaultBtn(f_cancel,isView ? null : f_save);
 //创建表单结构
  var groupicon="../lib/icons/32X32/communication.gif";
        var mainform = $("#mainform");  
        mainform.ligerForm({ 
         fields : [
         {name:"userid",type:"hidden"},
         {display:"登录名",name:"username",newline:true,labelWidth:100,width:200,space:30,type:"text",validate:{required:true,maxlength:30}, group:"基本信息",groupicon:"../lib/icons/32X32/communication.gif"},
         {display:"真实名字",name:"truename",newline:false,labelWidth:100,width:200,space:30,validate:{required:true,maxlength:10},type:"text",groupicon:groupicon},     
         {display:"密码",name:"password",newline:true,labelWidth:100,width:200,space:30,type:"text",groupicon:groupicon},
         {display:"部门",name:"deptwuyong",newline:false,labelWidth:100,width:200,space:30,validate:{required:true,maxlength:60},type:"select",comboboxName:"depttitle",
          options:{ url :'ajaxhandler/select.aspx?view=deptlist',checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
         {display:"性别",name:"sexwy",newline:true,labelWidth:100,width:200,space:30,validate:{required:true,maxlength:60},type:"select",comboboxName:"sextitle",
          options:{ data :[{id:1,text:'男'},{id:2,text:'女'},{id:0,text:'未知'}],checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
         {display:"职位",name:"job",newline:false,labelWidth:100,width:200,space:30,type:"text", groupicon:groupicon},
         {display:"连接地址",name:"email",newline:true,labelWidth:100,width:200,space:30,type:"text", groupicon:groupicon},
         {display:"QQ",name:"telphone",newline:false,labelWidth:100,width:200,space:30,type:"text", groupicon:groupicon},
         {display:"手机",name:"mobilephone",newline:true,labelWidth:100,width:200,space:30,type:"text", groupicon:groupicon},
           { display: "权重", name: "sort", newline: false, labelWidth: 100, width: 200, space: 30, type: "text", groupicon: groupicon },
         {display:"所属站点",name:"siteidwuy",newline:true,labelWidth:100,width:200,space:30,group:"权限信息",groupicon:"../lib/icons/32X32/communication.gif",validate:{required:true,maxlength:60},type:"select",comboboxName:"sitetitle",
          options:{ url :'ajaxhandler/select.aspx?view=sitelist',checkbox:false,nodeWidth :220,isMultiSelect: false}}, 
          {display:"角色",name:"rolewy",newline:false,labelWidth:100,width:200,space:30,validate:{required:true,maxlength:60},type:"select",comboboxName:"roletitle",
          options:{ url :'ajaxhandler/select.aspx?view=rolelist',checkbox:false,nodeWidth :220,isMultiSelect: false}},
          { display: "权限", name: "rights", newline: true, labelWidth: 100, width: 535, space: 30, type: "textarea", groupicon: groupicon },
          { name: "imglist", type: "hidden" },
         { name: "imgtitlelist", type: "hidden" }
         <%=catalogs%>
   
 ],
		 toJSON:JSON2.stringify
        });
        
         var actionRoot = "ajaxhandler/personinfodo.aspx";
        if(isEdit&&!issuper){ //不是超级编辑 显示部分功能
           $("#username").attr("readonly", "readonly");
            mainform.attr("action", actionRoot + "?method=update&id="+id); 
        }
        else if(isEdit&&issuper) //超级编辑显示所有功能
           {
            $("#username").attr("readonly", "readonly");
             mainform.attr("action", actionRoot + "?method=update&id="+id); 
           }
        if (isAddNew) {//新增
            mainform.attr("action", actionRoot + "?method=add");
        }
        else { //编辑
            LG.loadForm(mainform, { type: 'ajaxpersoninfo', method: 'GetPersoninfo', data: { ID: id} },f_loaded);
        }  

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
                    var win = parent||window;
                    win.LG.closeAndReloadParent(null, "userlist");
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
