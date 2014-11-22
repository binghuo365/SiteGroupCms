<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="movecatalog.aspx.cs" Inherits="SiteGroupCms.movecatalog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>站群内容管理系统</title>
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <script src="lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <link href="lib/css/common.css" rel="stylesheet" type="text/css" />
    <link href="lib/css/index.css" rel="stylesheet" type="text/css" />
    <script src="../lib/js/LG.js" type="text/javascript"></script>

    

</head>
<body>
    
    <div id="pageloading">
    </div>
    <div style="padding:5px 0 0 5px;">
         移动方式:&nbsp; &nbsp;  <label>复制&nbsp;  <input type="radio" name="RadioGroup1" value="1" />
</label>&nbsp;  
       <label>剪切&nbsp;  <input type="radio" name="RadioGroup1" value="0" checked="checked"/></label> 
       <label>引用&nbsp;  <input type="radio" name="RadioGroup1" value="2"/></label> 
    </div>
                <ul id="tree1" style="margin-top: 3px;">
              </ul>
    
<script type="text/javascript">

            $(function ()
            {
            //清空所有以下的cookie
             LG.cookies.set("copycatalogid","0",1);
             LG.cookies.set("type","0",1);
                 //绑定树菜单
              var tree=  $("#tree1").ligerTree({
                    url: 'ajaxhandler/loadcatalogtree.aspx?type=notincludeshare',
                    checkbox: false,
                    slide: false,
                    childIcon:'folder',
                    nodeWidth: 100,
                      onSelect: function (node)
                    {
                       if (!node.data.id) return;
                        LG.cookies.set("copycatalogid",node.data.id,1);
                    },
                    attribute: ['nodename', 'url']
                });
                tree = $("#tree1").ligerGetTreeManager();
                $("#pageloading").hide();
                //加载菜单栏
                
                $('[name="RadioGroup1"]').click(function(){
                LG.cookies.set("type",$('[name="RadioGroup1"]:checked').attr("value"),1);
                }); 

                
             // alert($('[name="RadioGroup1"]:checked').attr("value")); 
                
                
            });

       
     

            
    </script>
</body>
</html>