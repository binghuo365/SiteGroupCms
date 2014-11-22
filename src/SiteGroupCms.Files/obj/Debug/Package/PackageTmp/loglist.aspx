﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loglist.aspx.cs" Inherits="SiteGroupCms.loglist" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="../lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <script src="../lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>   
    <link href="../lib/css/common.css" rel="stylesheet" type="text/css" />  
    <script src="../lib/js/common.js" type="text/javascript"></script>   
    <script src="../lib/js/LG.js" type="text/javascript"></script>
    <script src="../lib/js/ligerui.expand.js" type="text/javascript"></script> 
    <script src="../lib/json2.js" type="text/javascript"></script>

</head>
<body>
       <ipnut type="hidden" id="MenuNo" value="loglist" />
  <div>
  <form id="search">

  </form>
  </div>
  <div id="maingrid"></div> 
  
  <script type="text/javascript">

      //相对路径
      var rootPath = "../";
      //列表结构
      var grid = $("#maingrid").ligerGrid({ 
          columns: [
          {display:"序号",name:"id",width:"5%"}
          ,{display:"操作类型",name:"type",width:"20%", align:"left"}
          ,{display:"操作人",name:"douser",width:"10%"}
          ,{display:"操作ip",name:"doip",width:"10%"}
          ,{display:"操作时间",name:"dotime",type:"string",width:"15%"}     
          ], 
          
          dataAction: 'server', 
          pageSize: 20, 
          toolbar: {},
          selectRowButtonOnly:true,
          onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow,
          url: rootPath + 'ajaxhandler/loadloglist.aspx', sortName: 'id', 
          width: '100%', height: '100%',heightDiff:-10, checkbox: true
      });

      //加载toolbar
      LG.loadToolbar(grid, toolbarBtnItemClick);

      //工具条事件
      function toolbarBtnItemClick(item) {
          switch (item.id) {

              case "qingkong"://清空日志
                  jQuery.ligerDialog.confirm('确定清空吗?', function (confirm) {
                      if (confirm)
                          f_qikong();
                  });
                  break;
              case "delete":
                  jQuery.ligerDialog.confirm('确定删除吗?', function (confirm) {
                      if (confirm)
                          f_delete();
                  });
                  break;
          }
      }
      function f_reload() {
          grid.loadData();
          checkedCustomer=[];
      }
      
      
       function f_onCheckAllRow(checked)
        {
            for (var rowid in this.records)
            {
                if(checked)
                    addCheckedCustomer(this.records[rowid]['id']);
                else
                    removeCheckedCustomer(this.records[rowid]['id']);
            }
        }

        /*
        该例子实现 表单分页多选
        即利用onCheckRow将选中的行记忆下来，并利用isChecked将记忆下来的行初始化选中
        */
        var checkedCustomer = [];
        function findCheckedCustomer(CustomerID)
        {
            for(var i =0;i<checkedCustomer.length;i++)
            {
                if(checkedCustomer[i] == CustomerID) return i;
            }
            return -1;
        }
        function addCheckedCustomer(CustomerID)
        {
            if(findCheckedCustomer(CustomerID) == -1)
                checkedCustomer.push(CustomerID);
        }
        function removeCheckedCustomer(CustomerID)
        {
            var i = findCheckedCustomer(CustomerID);
            if(i==-1) return;
            checkedCustomer.splice(i,1);
        }
        function f_isChecked(rowdata)
        {
            if (findCheckedCustomer(rowdata.id) == -1)
                return false;
            return true;
        }
        function f_onCheckRow(checked, data)
        {
            if (checked) addCheckedCustomer(data.id);
            else removeCheckedCustomer(data.id);
        }

      
      
      
      function f_delete() {
          if (checkedCustomer.length>0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'deletelog',
                  loading: '正在删除中...',
                  data: { ID: checkedCustomer.join(',')},
                  success: function () {
                      LG.showSuccess('删除成功');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('请选择行!');
          }
      }
      function f_qikong() {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'qingkonglog',
                  loading: '正在清空中...',
                  data: { ID: checkedCustomer.join(',')},
                  success: function () {
                      LG.showSuccess('清空成功');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
      } 
      
  </script>
</body>
</html>
