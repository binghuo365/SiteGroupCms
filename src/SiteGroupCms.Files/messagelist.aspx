<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="messagelist.aspx.cs" Inherits="SiteGroupCms.messagelist" %>

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
    <style>
    </style>
</head>
<body>
       <ipnut type="hidden" id="MenuNo" value="messagelist" />
  
  <div id="maingrid"></div> 
  <script type="text/javascript">
  var type="";
  var catalogid=""
  type=getQueryStringByName("type");
  catalogid=getQueryStringByName("catalogid");
      //相对路径
      var rootPath = "../";
      //列表结构
      var grid = $("#maingrid").ligerGrid({ 
           columns: [{display:"序号",name:"id",width:"5%"},
           {display:"标题",name:"title",width:"25%", align:"left"},
           {display:"发送人",name:"senduser",width:"10%"},
           {display:"发送时间",name:"sendtime",type:"string",width:"20%"}
          ,{display:"阅读",name:"isread",type:"string",width:"10%"}
          ], 
          dataAction: 'server', 
          pageSize: 20, 
          detail: { onShowDetail: f_showOrder },
          selectRowButtonOnly:true,
          onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow,
          url: rootPath + 'ajaxhandler/loadmessagelist.aspx?type=list', sortName: 'id', 
          toolbar: {},
          width: '100%', height: '100%',heightDiff:-10, checkbox: true
      });




      //加载toolbar
      LG.loadToolbar(grid, toolbarBtnItemClick);

      //工具条事件
      function toolbarBtnItemClick(item) {
          switch (item.id) {
 
              case "view":
                  var selected = grid.getSelected();
                  if (!selected) { LG.tip('请选择行!'); return }
                  f_showOrder(selected,detailPanel);
                  break;
              case "modify": //标记为已经读
                  jQuery.ligerDialog.confirm('确定标记为已读?', function (confirm) {
                      if (confirm)
                          f_read();
                  });
                  break;
             case "add":
                  top.f_addTab(null, '发布消息', 'messageedit.aspx?method=add');
                  break;
              case "delete": //删除信息
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

         function f_read() {
          if (checkedCustomer.length>0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'readnotice',
                  loading: '操作中...',
                  data: { ids: checkedCustomer.join(',')},
                  success: function () {
                      LG.showSuccess('标记为已读');
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
      function f_delete() {
         if (checkedCustomer.length>0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'delnotice',
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
             
             
      function f_showOrder(row, detailPanel,callback)
        {
            var grid = document.createElement('div'); 
            $(detailPanel).append(grid);
             $(grid).css('text-indent',30);
            $(grid).css('margin',10).ligerGrid({
                columns:
                            [
                            { display: '详细内容', name: 'content',width:'599',height:'400',align:'left' },
                            ], isScroll: false, showToggleColBtn: false, width: '600',usePager:false,checkbox:false             
                 , onAfterShowData: callback,frozen:false,
              
                   url:'ajaxhandler/loadmessagelist.aspx?type=detail&id='+row.id
            });  
        }
        
         function f_showOrder2(row, detailPanel,callback)
        {
            var grid = document.createElement('div'); 
            $(detailPanel).append(grid);
             $(grid).css('text-indent',30);
            $(grid).css('margin',10).ligerGrid({
                columns:
                            [
                            { display: '详细内容', name: 'content',width:'599',height:'400',align:'left' },
                            ], isScroll: false, showToggleColBtn: false, width: '600',usePager:false,checkbox:false             
                 , onAfterShowData: callback,frozen:false,
              
                   url:'ajaxhandler/loadmessagelist.aspx?type=detail&id='+row.id
            });  
        }
        
   

  </script>
  

  
  
</body>
</html>
