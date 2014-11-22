<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userlist.aspx.cs" Inherits="SiteGroupCms.userlist" %>

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
    <script src="../lib/js/ligerui.expand.js" type="text/javascript"></script> 
    <script src="../lib/json2.js" type="text/javascript"></script>
    <style>
    #searchtruename{width:150px;border:1px solid #ccc;}
    #searchdeptid{width:150px;border:1px solid #ccc;}
    
    </style>
</head>
<body>
       <ipnut type="hidden" id="MenuNo" value="userlist" />
  <div id="mainsearch" style=" width:100%">
    <div class="searchtitle">
        <span>搜索</span><img src="lib/icons/32X32/searchtool.gif" />
        <div class="togglebtn"></div> 
    </div>
    <div class="navline" style="margin-bottom:4px; margin-top:4px;"></div>
    <div class="searchbox">
<form id="formsearch" class="l-form">
<table>
<tr>
<td width=10></td>
<td>真实名字</td>
<td width=10></td>
<td><input type="text" id="searchtruename"  ></td>
<td width=10></td>
<td>部门</td>
<td width=10></td>
<td><select  id="searchdeptid" >

</select></td>


<td width=10></td>
<td><div id="searchbutton"></div></td>
</tr>
</table>
        </form>

    </div>
  </div>
  <div id="maingrid"></div> 
  

  
  
  <script type="text/javascript">
      var type = "";
      var catalogid = ""
      type = getQueryStringByName("type");
      catalogid = getQueryStringByName("catalogid");
      //相对路径
      var rootPath = "../";
      //列表结构
      var grid = $("#maingrid").ligerGrid({
          columns: [{ display: "序号", name: "id", width: "5%" },
          { display: "登录名", name: "username", width: "10%", align: "left" },
          { display: "真实名", name: "truename", width: "10%" },
          { display: "部门", name: "depttitle", type: "string", width: "15%" },
          { display: "职位", name: "job", width: "8%" },
           { display: "权重", name: "sort", width: "7%" },
          { display: "状态", name: "state", width: "10%" },
          { display: "角色", name: "role", width: "10%" },
          { display: "添加时间", name: "addtime", width: "12%" },
          { display: "最近登录", name: "logintime", width: "12%" }
          ],
          dataAction: 'server',
          pageSize: 20,
          selectRowButtonOnly: true,
          toolbar: {},
          selectRowButtonOnly: true,
          onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow,
          url: rootPath + 'ajaxhandler/loaduserlist.aspx', sortName: 'id',
          width: '100%', height: '100%', heightDiff: -10, checkbox: true
      });

      //增加搜索按钮,并创建事件  
      LG.appendSearchButtons("#formsearch", grid);

      //加载toolbar
      LG.loadToolbar(grid, toolbarBtnItemClick);

      //工具条事件
      function toolbarBtnItemClick(item) {
          switch (item.id) {
              case "add":
                  top.f_addTab(null, '添加用户', 'personinfo.aspx?method=add&issuper=super');
                  break;
              case "view":
                  var selected = grid.getSelected();
                  if (!selected) { LG.tip('请选择行!'); return }
                  top.f_addTab(null, '查看用户信息', 'personinfo.aspx?method=view&id=' + selected.id);
                  break;
              case "modify":
                  var selected = grid.getSelected();
                  if (!selected) { LG.tip('请选择行!'); return }
                  top.f_addTab(null, '修改用户信息', 'personinfo.aspx?method=update&issuper=super&id=' + selected.id);
                  break;
              case "lock":
                  jQuery.ligerDialog.confirm('确定锁定吗?', function (confirm) {
                      if (confirm)
                          f_lock();
                  });
                  break;
              case "unlock":
                  jQuery.ligerDialog.confirm('确定解锁吗?', function (confirm) {
                      if (confirm)
                          f_unlock();
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
          checkedCustomer = [];
      }

      //加载部门列表
      $.ajax({
          type: "post",
          dataType: "json",
          url: "ajaxhandler/select.aspx?view=deptlist",
          success: function (d) {
              $("#searchdeptid").append("<option value='0'>点击选择</option>");
              for (i = 0; i < d.length; i++) {
                  $("#searchdeptid").append("<option value='" + d[i].id + "'>" + d[i].text + "</option>");
              }

          }
      });

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
              $.ajax({
                  type: "post",
                  dataType: "json",
                  url: "ajaxhandler/personinfodo.aspx?method=delete",
                  data: { ids: checkedCustomer.join(',') },
                  success: function (d) {
                      if (d.IsError == false) {
                          LG.showSuccess('删除成功');
                          f_reload();
                      }
                      else {
                          LG.showError(d.Message);
                      }
                  }
              });
          }
          else {
              LG.tip('请选择行!');
          }
      }


      function f_lock() {
          if (checkedCustomer.length > 0) {
              $.ajax({
                  type: "post",
                  dataType: "json",
                  url: "ajaxhandler/personinfodo.aspx?method=lock",
                  data: { ids: checkedCustomer.join(',') },
                  success: function (d) {
                      if (d.IsError == false) {
                          LG.showSuccess('锁定成功');
                          f_reload();
                      }
                      else {
                          LG.showError(d.Message);
                      }
                  }
              });
          }
          else {
              LG.tip('请选择行!');
          }
      }


      function f_unlock() {
          if (checkedCustomer.length > 0) {
              $.ajax({
                  type: "post",
                  dataType: "json",
                  url: "ajaxhandler/personinfodo.aspx?method=unlock",
                  data: { ids: checkedCustomer.join(',') },
                  success: function (d) {
                      if (d.IsError == false) {
                          LG.showSuccess('解锁成功');
                          f_reload();
                      }
                      else {
                          LG.showError(d.Message);
                      }
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
