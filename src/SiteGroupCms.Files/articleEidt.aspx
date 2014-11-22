<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="articleEidt.aspx.cs" validateRequest="false" Inherits="SiteGroupCms.articleEidt" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
     <link href="../lib/css/articleedit.css" rel="stylesheet" type="text/css" />
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

    
    <link rel="stylesheet" href="kindeditor/themes/default/default.css" />
	<link rel="stylesheet" href="kindeditor/plugins/code/prettify.css" />
	<script charset="utf-8" src="kindeditor/kindeditor.js"></script>
	<script charset="utf-8" src="kindeditor/lang/zh_CN.js"></script>
	<script charset="utf-8" src="kindeditor/plugins/code/prettify.js"></script>
	<script>

	    var editor;
	    KindEditor.ready(function (K) {
	        editor = K.create('#content1', {
	            cssPath: 'kindeditor/plugins/code/prettify.css',
	            uploadJson: 'kindeditor/upload_json.ashx',
	            fileManagerJson: 'kindeditor/file_manager_json.ashx',
	            allowFileManager: true,
	            afterBlur: function () {//失去焦点就更新
	                this.sync();
	            }
	        });

	        prettyPrint();



	        K('#addpic').click(function () {
	            var imgliststr = $("#imglist").attr("value");
	            var index = imgliststr.split(",").length;
	            editor.loadPlugin('image', function () {
	                editor.plugin.imageDialog({
	                    //imageUrl : K('#url1').val(),
	                    clickFn: function (url, title, width, height, border, align) {
	                        var div = K('#imglists');
	                        div.append('<div id=\"' + index + '\" style=\"float:left;width:60px;\"><div><img src="' + url + '" title="' + title + '" /></div><div><a href=\"javascript:delimg(\'' + url + '\',' + index + ')\">删除</a></div></div>');
	                        $("#imglist").attr("value", $("#imglist").attr("value") + url + ",");
	                        $("#imgtitlelist").attr("value", $("#imgtitlelist").attr("value") + url + ",");
	                        editor.hideDialog();
	                    }
	                });
	            });
	        });



	        //添加附件集	
	        K('#addatts').click(function () {

	            var attliststr = $("#attslist").attr("value");
	            var index = attliststr.split(",").length;
	            editor.loadPlugin('insertfile', function () {
	                editor.plugin.fileDialog({
	                    //fileUrl : K('#url').val(),
	                    clickFn: function (url, title) {
	                        var li = K('#ulattslist');
	                        if (title != "") {
	                            li.append('<li id=\"' + index + '\">' + title + '&nbsp;&nbsp;&nbsp;<a href="javascript:del(\'' + url + '\',' + index + ')">删除</a></li>');
	                            $("#attslist").attr("value", $("#attslist").attr("value") + url + ",");
	                            $("#attstitlelist").attr("value", $("#attstitlelist").attr("value") + title + ",");
	                        }
	                        else {
	                            li.append('<li id=\"' + index + '\">' + url + '&nbsp;&nbsp;&nbsp;<a href="javascript:del(\'' + url + '\',' + index + ')">删除</a></li>');
	                            $("#attslist").attr("value", $("#attslist").attr("value") + url + ",");
	                            $("#attstitlelist").attr("value", $("#attstitlelist").attr("value") + url + ",");
	                        }
	                        editor.hideDialog();
	                    }
	                });
	            });
	        });
	        //拾色器
	        var colorpicker;
	        K('#color').bind('click', function (e) {
	            e.stopPropagation();
	            if (colorpicker) {
	                colorpicker.remove();
	                colorpicker = null;
	                return;
	            }
	            var colorpickerPos = K('#color').pos();
	            colorpicker = K.colorpicker({
	                x: colorpickerPos.x,
	                y: colorpickerPos.y + K('#color').height(),
	                z: 19811214,
	                selectedColor: 'default',
	                noColor: '无颜色',
	                click: function (color) {
	                    K('#color').val(color);
	                    colorpicker.remove();
	                    colorpicker = null;
	                }
	            });
	        });
	        K(document).click(function () {
	            if (colorpicker) {
	                colorpicker.remove();
	                colorpicker = null;
	            }
	        });

	    });
		
	</script>

 <SCRIPT type=text/javascript>
 //tab标签变换js
         function selectTag(showContent, selfObj) {
             // 标签
             var tag = document.getElementById("tags").getElementsByTagName("li");
             var taglength = tag.length;
             for (i = 0; i < taglength; i++) {
                 tag[i].className = "";
             }
             selfObj.parentNode.className = "selectTag";
             // 标签内容
             for (i = 0; j = document.getElementById("tagContent" + i); i++) {
                 j.style.display = "none";
             }
             document.getElementById(showContent).style.display = "block";
         }
</SCRIPT>

</head>
<body>

 

<ul id=tags>
  <li class="selectTag"><A onClick="selectTag('tagContent0',this)" 
  href="javascript:void(0)">基本信息</A> </li>
  <li><A onClick="selectTag('tagContent1',this)" 
  href="javascript:void(0)">附件集</A> </li>
  <li><A onClick="selectTag('tagContent2',this)" 
  href="javascript:void(0)">图片集</A> </li></ul>
<div id=tagContent>
<div class="tagContent selectTag" id="tagContent0">
<div id="formcontent">
     <form id="mainform" method="post" >
         <textarea id="content2" runat="server" visible="true" style="display:none"> </textarea>
     </form>
    </div> 
</div>
<div class="tagContent" id="tagContent1">
     <div class="atts">
     <img src="lib/icons/function_icon_set/add_48.png" style="margin:5px 0 0 0; cursor:hand;" title="点击添加文件" id="addatts" width="20" height="20"/>
     </div> 
     <div id="attslist" style="padding-bottom:40px; height:auto">
     <ul id="ulattslist">
     </ul>
   <!--   <table id="mytable" cellspacing="0"> 
<tr> 
<th scope="col">附件名称</th> 
<th scope="col">附件地址</th> 
</tr> 
<tr> 
<td class="row">badwolf</td> 
<td class="row">100</td> 
</tr> 
</table> 
-->
     </div>
</div>
<div class="tagContent" id="tagContent2">
 <div class="pic">
     <img src="lib/icons/function_icon_set/add_48.png" style="margin:5px 0 0 0; cursor:hand;" title="点击添加图片" id="addpic" width="20" height="20"/>
     </div> 

     <div id="imglists">
     </div>
</div>
</div>


     
    
     

    

   
    
     	
    <script type="text/javascript"> 
        //当前ID
        var artid = getQueryStringByName("id");
        //是否新增状态
        var isAddNew = getQueryStringByName("method")=="add";
        //是否编辑状态
        var isEdit = getQueryStringByName("method")=="update";
        //添加的栏目
        var catalogid=getQueryStringByName("catalogid");

        //覆盖本页面grid的loading效果
        LG.overrideGridLoading(); 

        //表单底部按钮 
        LG.setFormDefaultBtn(f_cancel,f_save);

  //创建表单结构
  var groupicon="../lib/icons/32X32/communication.gif";
        var mainform = $("#mainform");  
        mainform.ligerForm({ 
         fields : [
         {name:"artid",type:"hidden"},
         {display:"<span style=color:red>文章标题</span>",name:"title",newline:true,labelWidth:100,width:400,space:30,type:"text",validate:{required:true,maxlength:60}},
         {display:"颜色",name:"color",newline:false,labelWidth:50,width:70,space:30,type:"text",groupicon:groupicon},     
         {display:"flv视频地址",name:"subtitle",newline:true,labelWidth:100,width:400,space:30,type:"text",validate:{required:false,maxlength:60}, groupicon:groupicon},
         {display:"浏览量",name:"clickcount",newline:false,labelWidth:50,width:70,space:30, type:"text",validate:{digits:true},groupicon:groupicon},
         {display:"链接地址",name:"linkurl",newline:true,labelWidth:100,width:400,space:10,type:"text",groupicon:groupicon},
         {name:"cc",type:"text",label:"(不为空时，文章直接调转到此地址)",newline:false,width:1,labelwidth:250},   
 {display:"栏目",name:"catalogidwuyong",newline:true,labelWidth:100,width:200,space:30,validate:{required:true,maxlength:60},type:"select",comboboxName:"catalogtitle",options:{tree:{
url :'ajaxhandler/select.aspx?view=catalogtree',
checkbox:false,
nodeWidth :220
},valueFieldID:"catalogid",valueField:"catalogid"}}, 

         {display:"采集时间",name:"addtime",newline:false,labelWidth:100,width:200,space:30,type:"date",format:"yyyy-MM-dd", groupicon:groupicon},
         {display:"来源",name:"source",newline:true,labelWidth:100,width:200,space:30,type:"text",validate:{required:false,maxlength:100},groupicon:groupicon},
         {display:"关键字",name:"keywords",newline:false,labelWidth:100,width:200,space:30,type:"text",validate:{required:false,maxlength:20},groupicon:groupicon},
         {display:"属性",name:"state",newline:true,labelWidth:100,width:1,space:10,type:"text",validate:{required:false,maxlength:20},groupicon:groupicon },
         {display:"推荐",name:"isrecommend",newline:false,labelWidth:40,width:20,space:10,type:"checkbox",validate:{required:false,maxlength:20},groupicon:groupicon },
         {display:"幻灯片",name:"isppt",newline:false,labelWidth:50,width:20,space:10,type:"checkbox",validate:{required:false,maxlength:20},groupicon:groupicon },
         {display:"滚动",name:"isroll",newline:false,labelWidth:40,width:20,space:10,type:"checkbox",validate:{required:false,maxlength:20},groupicon:groupicon },
         {display:"共享",name:"isshare",newline:false,labelWidth:40,width:20,space:10,type:"checkbox",validate:{required:false,maxlength:20},groupicon:groupicon },
         {display:"摘要",name:"abstract",newline:true,labelWidth:100,width:532,space:30,type:"textarea",groupicon:groupicon},
         {display:"内容",name:"content1",newline:true,labelWidth:100,width:732,height:400,space:30,type:"textarea",groupicon:groupicon},
         {name:"imglist",type:"hidden"},
         {name:"imgtitlelist",type:"hidden"},
         {name:"attslist",type:"hidden"},
         {name:"attstitlelist",type:"hidden"}
 ],
		 toJSON:JSON2.stringify
        });
        

        var actionRoot = "ajaxhandler/articleEditdo.aspx";
        if(isEdit){ //编辑
          mainform.attr("action", actionRoot + "?method=update"); 
        }
        if (isAddNew) {//新增
            mainform.attr("action", actionRoot + "?method=add");
            if(catalogid.toString().indexOf("site")<0)//站点id
            $("#catalogid").val(catalogid);
        }
        else { //查看或者编辑
            LG.loadForm(mainform, { type: 'ajaxarticle', method: 'GetArticle', data: { ID: artid} },f_loaded);
        }  
           
            //设置表单的验证属性
            jQuery.metadata.setType("attr", "validate"); 
            LG.validate(mainform);
		function f_loaded()
        {
        //加载图片集和附件集
       var div = $('#imglists');
       var imgliststr=$("#imglist").attr("value");//来自表单
       var imgtitleliststr=$("#imgtitlelist").attr("value");
       var li=$('#ulattslist');
       var attsliststr=$("#attslist").attr("value");
       var attstitlteliststr=$("#attstitlelist").attr("value");
      //以下为图片集 暂时不用
     
       for(i=0;i<imgliststr.split(",").length-1;i++)
	    div.append('<div id=\"'+i+'\" style=\"float:left;width:60px;\"><div><img src="' + imgliststr.split(",")[i] + '" title="'+imgtitleliststr.split(",")[i]+'" /></div><div><a href=\"javascript:delimg(\''+imgliststr.split(",")[i]+'\','+i+')\">删除</a></div></div>');
	       //加载附件集合 
	         for(j=0;j<attsliststr.split(",").length-1;j++)
	          li.append('<li id=\"'+j+'\">'+attsliststr.split(",")[j]+'&nbsp;&nbsp;&nbsp;<a href="javascript:del(\''+attsliststr.split(",")[j]+'\','+j+')">删除</a></li>');

		
		//以下为加载文章内容
	   editor.html("");						
       editor.html($("#content2").attr("value"));          
        }
        
        
        function f_save() {
            //更新编辑器
            editor.sync();
            LG.submitForm(mainform, function (data) {
                var win = parent || window;
                if (data.IsError) {  
                    LG.showError('错误:' + data.Message);
                }
                else { 
                 var win = parent||window;
                    win.LG.showSuccess('保存成功', function () { 
                    win.LG.closeAndReloadParent(null,"栏目");
                    });
                }
            });
        }
        function f_cancel()
        {
            var win = parent||window;
             win.LG.closeCurrentTab(null);
          
        }

function del(a,self)//用于删除附近里面的东西
{
       var attsliststr=$("#attslist").attr("value");
       var str=attsliststr.split(',');
       for(i=0;i<str.length-1;i++)
       {
         if(str[i]==a)//是要删除的元素，
         {
          str.splice(i,1);
           break;
           }
       } 
       $("#attslist").attr("value",str.join(','));
       $("#"+self).remove();
         
}


function delimg(a,self)//用于删除img里面的东西
{
       var imgliststr=$("#imglist").attr("value");
       var str=imgliststr.split(',');
       for(j=0;j<str.length-1;j++)
       {
         if(str[j]==a)//是要删除的元素，
         {
          str.splice(j,1);
           break;
           }
       } 
       $("#imglist").attr("value",str.join(','));
       $("#"+self).remove();    
}

    </script>
</body>
</html>
