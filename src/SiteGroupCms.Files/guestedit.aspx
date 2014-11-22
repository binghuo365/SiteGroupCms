<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guestedit.aspx.cs" ValidateRequest="false" Inherits="SiteGroupCms.guestedit" %>

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
    var gid = getQueryStringByName("id");
    //是否新增状态
    var isAddNew = getQueryStringByName("method") == "add";
    //是否编辑状态
    var isEdit = getQueryStringByName("method") == "update";


    //覆盖本页面grid的loading效果
    LG.overrideGridLoading();
    //表单底部按钮 
    if (isEdit)
        LG.setFormDefaultBtn(f_cancel, f_save);
    else
        LG.setFormDefaultBtn(f_cancel);
    //创建表单结构
    var groupicon = "../lib/icons/32X32/communication.gif";
    var mainform = $("#mainform");
    mainform.ligerForm({
        fields: [
       { name: "guestid", type: "hidden" },
        { display: "留言人", name: "username", newline: true, labelWidth: 80, width: 200, space: 10, type: "text", groupicon: groupicon, group: "留言信息" },
       { display: "采集时间", name: "addtime", newline: true, labelWidth: 80, width: 200, space: 10, type: "date", format: "yyyy-MM-dd", groupicon: groupicon },
       { display: "留言标题", name: "title", newline: true, labelWidth: 80, width: 400, space: 10, type: "text", groupicon: groupicon },
        { display: "IP地址", name: "userip", newline: true, labelWidth: 80, width: 400, space: 10, type: "text", groupicon: groupicon },
       { display: "留言内容", name: "content", newline: true, labelWidth: 80, width: 400, space: 10, type: "textarea", groupicon: groupicon },

       { display: "通过审核", name: "audit", newline: true, labelWidth: 80, width: 400, space: 10, type: "checkbox", groupicon: groupicon },
       { display: "回复时间", name: "retime", newline: true, labelWidth: 80, width: 200, space: 10, type: "date", format: "yyyy-MM-dd", groupicon: groupicon },
       { display: "回复内容", name: "recontent", newline: true, labelWidth: 80, width: 400, space: 10, type: "textarea", groupicon: groupicon }
 ],
        toJSON: JSON2.stringify
    });

    var actionRoot = "ajaxhandler/guestdo.aspx";
    if (isEdit) { //b编辑
        // $("#artid").attr("readonly", "readonly").removeAttr("validate");
        mainform.attr("action", actionRoot + "?method=update");
    }

    if (isAddNew) {//新增
        mainform.attr("action", actionRoot + "?method=add");
    }
    else { //编辑
        LG.loadForm(mainform, { type: 'ajaxguest', method: 'GetGuest', data: { ID: gid} }, f_loaded);
    }

    //验证
    jQuery.metadata.setType("attr", "validate");
    LG.validate(mainform);

    function f_loaded() {

    }
    function f_save() {

        LG.submitForm(mainform, function (data) {
            var win = parent || window;
            if (data.IsError) {
                LG.showError('错误:' + data.Message);

            }
            else {
                LG.showSuccess('保存成功', function () {
                    var win = parent || window;
                    win.LG.closeAndReloadParent(null, "guestlist");
                });
            }
        });
    }
    function f_cancel() {
        var win = parent || window;
        win.LG.closeCurrentTab(null);

    }

</script>
</body>
</html>

