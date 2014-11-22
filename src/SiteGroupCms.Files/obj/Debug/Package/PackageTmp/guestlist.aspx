<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guestlist.aspx.cs" Inherits="SiteGroupCms.guestlist" %>
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
       <ipnut type="hidden" id="MenuNo" value="guestlist" />
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
          { display: "序号", name: "id", width: "5%" }
          , { display: "留言标题", name: "title", width: "18%", align: "left" }
          , { display: "留言人", name: "username", width: "10%", align: "left" }
          , { display: "留言时间", name: "addtime", width: "10%", align: "left" }
          , { display: "回复", name: "repost", width: "30%", align: "left" }
          , { display: "回复时间", name: "retime", width: "10%", align: "left" }
          , { display: "审核", name: "audit", width: "3%", align: "left" }
          , { display: '操作', isAllowHide: false, width: "7%",
              render: function (row) {
                  var html = '<a  href="javascript:f_view(' + row.id + ')">查看</a>&nbsp;';
                  html += '<a href="javascript:f_modify(' + row.id + ')">回复</a>&nbsp;';
                  return html;
              }
          }
          ],
          selectRowButtonOnly: true,
          dataAction: 'server',
          pageSize: 20,
          toolbar: {},
          onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow,
          url: rootPath + 'ajaxhandler/loadguestlist.aspx', sortName: 'id',
          width: '100%', height: '100%', heightDiff: -10, checkbox: true
      });

      //加载toolbar
      LG.loadToolbar(grid, toolbarBtnItemClick);

      //工具条事件
      function toolbarBtnItemClick(item) {
          switch (item.id) {
              case "view": //查看留言
                  if (checkedCustomer.length == 0) { LG.tip('请选择行!'); return; }
                  if (checkedCustomer.length > 1) { LG.tip('你选择了多行!'); return; }
                  f_view("0");
                  break;
              case "modify": //修改留言
                  if (checkedCustomer.length == 0) { LG.tip('请选择行!'); return; }
                  if (checkedCustomer.length > 1) { LG.tip('你选择了多行!'); return; }
                  f_modify("0");
                  break;
              case "delete": //删除留言
                  jQuery.ligerDialog.confirm('确定删除吗?', function (confirm) {
                      if (confirm)
                          f_delete();
                  });
                  break;
              case "pass": //通过留言
                  jQuery.ligerDialog.confirm('确定通过吗?', function (confirm) {
                      if (confirm)
                          f_pass();
                  });
                  break;
              case "reject": //否定留言
                  jQuery.ligerDialog.confirm('确定否定吗?', function (confirm) {
                      if (confirm)
                          f_reject();
                  });
                  break;
          }
      }
      function f_reload() {
          grid.loadData();
          checkedCustomer = [];
      }


      function f_onCheckAllRow(checked) {
          for (var rowid in this.records) {
              if (checked)
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
      function findCheckedCustomer(CustomerID) {
          for (var i = 0; i < checkedCustomer.length; i++) {
              if (checkedCustomer[i] == CustomerID) return i;
          }
          return -1;
      }
      function addCheckedCustomer(CustomerID) {
          if (findCheckedCustomer(CustomerID) == -1)
              checkedCustomer.push(CustomerID);
      }
      function removeCheckedCustomer(CustomerID) {
          var i = findCheckedCustomer(CustomerID);
          if (i == -1) return;
          checkedCustomer.splice(i, 1);
      }
      function f_isChecked(rowdata) {
          if (findCheckedCustomer(rowdata.id) == -1)
              return false;
          return true;
      }
      function f_onCheckRow(checked, data) {
          if (checked) addCheckedCustomer(data.id);
          else removeCheckedCustomer(data.id);
      }




      function f_delete() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'deldepart',
                  loading: '正在删除中...',
                  data: { ids: checkedCustomer.join(',') },
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

      function f_view(id) {
          if (id != "0")
              checkedCustomer[0] = id;
          top.f_addTab(null, '查看留言', 'guestedit.aspx?method=view&id=' + checkedCustomer[0]);
      }

      function f_modify(id) {
          if (id != "0")
              checkedCustomer[0] = id;
          top.f_addTab(null, '回复留言', 'guestedit.aspx?method=update&id=' + checkedCustomer[0]);
      }

      function f_delete() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'delguest',
                  loading: '操作中...',
                  data: { ids: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('留言删除成功');
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

      //通过
      function f_pass() {

          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'passguest',
                  loading: '操作中...',
                  data: { ids: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('审核成功');
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

      //否定
      function f_reject() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'rejectguest',
                  loading: '操作中...',
                  data: { ids: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('否定成功');
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
      
  </script>
</body>
</html>