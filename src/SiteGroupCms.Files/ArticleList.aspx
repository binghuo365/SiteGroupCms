<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleList.aspx.cs" Inherits="SiteGroupCms.ArticleList" %>

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
#searchtitle{width:200px; border:1px solid #ccc;}
#searchcatalogid{width:150px;border:1px solid #ccc;}
#searchstate{width:150px;border:1px solid #ccc;}

</style>
</head>
<body>
       <ipnut type="hidden" id="MenuNo" value="articlelist" />
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
<td>标题</td>
<td width=10></td>
<td><input type="text" id="searchtitle" name="searchtitle" ></td>
<td width=10></td>
<td>栏目</td>
<td width=10></td>
<td><select  id="searchcatalogid" name="searchcatalog">

</select></td>

<td width=10></td>
<td>状态</td>
<td width=10></td>
<td><select name="" id="searchstate" name="searchstate">
 <option value="0">点击选择</option>
 <option value="1">未通过审核</option>
 <option value="2">通过审核</option>
 <option value="3">共享</option>
 <option value="4">未发布</option>
 <option value="5">滚动新闻</option>
 <option value="6">推荐新闻</option>
 <option value="7">幻灯片新闻</option>
 <option value="8">外链新闻</option>

</select></td>
<td width=10></td>
<td><div id="searchbutton"></div></td>
</tr>
</table>
        </form>
    </div>
  </div>
  
  <div id="maingrid"></div> 
                  <ul id="tree2" style="margin-top: 3px;">
              </ul>
  
  <script type="text/javascript">

      $(function () {
          //绑定树菜单
          var tree = $("#tree2").ligerTree({
              url: 'ajaxhandler/loadcatalogtree.aspx',
              checkbox: false,
              slide: false,
              childIcon: 'folder',
              nodeWidth: 100,
              attribute: ['nodename', 'url']
          });
          tree = $("#tree2").ligerGetTreeManager();
          $("#pageloading").hide();
          //加载菜单栏
      });

      var type = "";
      var catalogid = "";
      type = getQueryStringByName("type");
      catalogid = getQueryStringByName("catalogid");
      //相对路径
      var rootPath = "../";
      //列表结构
      var grid = $("#maingrid").ligerGrid({
          columns: [
          { display: "标题", name: "title", width: "34%", align: "left" },
          { display: "作者", name: "author", width: "5%" },
          { display: "采集时间", name: "addtime", type: "string", width: "11%" },
          { display: "所属频道", name: "catalogid", width: "8%" },
          { display: "类型", name: "articletype", width: "5%" },
          { display: "浏览量", name: "clickcount", width: "4%" },
          { display: "属性", name: "state", width: "14%" },
          { display: '排序', isAllowHide: false, width: "4%",
              render: function (row) {
                  var html = '<a  href="javascript:f_sort(' + row.id + ',1)">上</a>&nbsp;';
                  html += '<a href="javascript:f_sort(' + row.id + ',0)">下</a>';
                  return html;
              }
          },
          { display: '操作', isAllowHide: false, width: "9%",
              render: function (row) {
                  var html = '<a  href="javascript:f_modify(' + row.id + ')">修改</a>&nbsp;';
                  html += '<a href="javascript:f_view(' + row.id + ')">查看</a>&nbsp;';
                  html += '<a href="javascript:f_sort(' + row.id + ',3)">置顶</a>';
                  return html;
              }
          }
          ],

          dataAction: 'server',
          pageSize: 20,
          toolbar: {},
          selectRowButtonOnly: true,
          onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow, allowHideColumn: false, rownumbers: true,
          url: rootPath + 'ajaxhandler/loadarticlelist.aspx?type=' + type + '&catalogid=' + catalogid,
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
                  top.f_addTab(null, '采集文章', 'articleEidt.aspx?method=add&catalogid=' + catalogid);
                  break;
              case "view":
                  if (checkedCustomer.length == 0) { LG.tip('请选择行!'); return; }
                  if (checkedCustomer.length > 1) { LG.tip('你选择了多行!'); return; }
                  f_view("0");
                  break;
              case "modify": //修改文章
                  if (checkedCustomer.length == 0) { LG.tip('请选择行!'); return; }
                  if (checkedCustomer.length > 1) { LG.tip('你选择了多行!'); return; }
                  f_modify("0");
                  break;
              case "copy":
                  if (checkedCustomer.length == 0) { LG.tip('请选择行!'); return; }
                  f_copy();
                  break;
              case "pass":
                  jQuery.ligerDialog.confirm('确定通过审核吗?', function (confirm) {
                      if (confirm)
                          f_pass();
                  });
                  break;
              case "reject":
                  jQuery.ligerDialog.confirm('确定否定吗?', function (confirm) {
                      if (confirm)
                          f_reject();
                  });
                  break;
              case "publish":
                  jQuery.ligerDialog.confirm('确定发布吗?', function (confirm) {
                      if (confirm)
                          f_publish();
                  });
                  break;
              case "share":
                  jQuery.ligerDialog.confirm('确定否定吗?', function (confirm) {
                      if (confirm)
                          f_share();
                  });
                  break;
              case "delete":
                  jQuery.ligerDialog.confirm('确定删除吗?', function (confirm) {
                      if (confirm)
                          f_delete();
                  });
                  break;
              case "cddelete":
                  jQuery.ligerDialog.confirm('确定彻底删除吗?', function (confirm) {
                      if (confirm)
                          f_cddelete();
                  });
                  break;
              case "huishou":
                  jQuery.ligerDialog.confirm('确定回收吗?', function (confirm) {
                      if (confirm)
                          f_huishou();
                  });
                  break;
              case "ipass":
                  loadipass();
                  break;
              case "allpublish":
                  jQuery.ligerDialog.confirm('确定发布吗?', function (confirm) {
                      if (confirm)
                          allpublish();
                  });
                  break;
          }
      }

      //加载栏目列表
      $.ajax({
          type: "post",
          dataType: "json",
          url: "ajaxhandler/loadcataloglist.aspx",
          data: { siteid: 1 },
          success: function (d) {
              $("#searchcatalogid").append("<option value='0'>点击选择</option>");
              for (i = 0; i < d.catalog.length; i++) {
                  $("#searchcatalogid").append("<option value='" + d.catalog[i].id + "'>" + d.catalog[i].title + "</option>");
              }

          }
      });




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
                  method: 'delarticle',
                  loading: '正在删除中...',
                  data: { ID: checkedCustomer.join(',') },
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

      function f_modify(id) {
          if (id != "0")
              checkedCustomer[0] = id;
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'getyiyongid',
                  loading: '加载中...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function (d) {
                      if (d.yyarticleid == "" || d.yyarticleid == "0")
                          top.f_addTab(null, '修改文章', 'articleEidt.aspx?method=update&id=' + checkedCustomer.join(','));
                      else
                          LG.showError('引用文章不可修改');
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

      function f_pass() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'passarticle',
                  loading: '操作中...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('全部通过审核');
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
      function f_reject() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'rejectarticle',
                  loading: '操作中...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('否定文章成功');
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
      function f_share() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'sharearticle',
                  loading: '操作中...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('共享文章成功');
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
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'getarticlelink',
                  loading: '操作中...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function (d) {
                      window.open(d.art);
                  },
                  error: function (d) {
                      LG.showError("尚未发布");
                  }
              });
          }
          else {
              LG.tip('请选择行!');
          }
      }
      function f_huishou() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'huishou',
                  loading: '操作中...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('回收文章成功');
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
      function f_cddelete() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'cddelete',
                  loading: '操作中...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('彻底删除文章成功');
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

      function f_publish() {
          if (checkedCustomer.length > 0) {
              LG.showLoading("未通过审核的文章不会发布，发布中文章较多时需要一点时间");
              $.ajax({
                  type: "post",
                  dataType: "json",
                  url: "ajaxhandler/publishajax.aspx?type=some",
                  data: { ids: checkedCustomer.join(',') },
                  success: function (d) {
                      if (!d.IsError) {
                          LG.showSuccess('发布文章成功');
                          LG.hideLoading();
                          f_reload();
                      }
                  },
                  error: function (message) {
                      LG.showError("未通过审核发布失败");
                      LG.hideLoading();

                  }
              });
          }
          else {
              LG.tip('请选择行!');
          }
      }

      function loadipass() {
          grid.set('parms', { paras: "ipass" });
          grid.loadData();
      }

      function allpublish() {
          LG.showLoading("未通过审核的文章不会发布，发布中文章较多时需要一点时间");
          $.ajax({
              type: "post",
              dataType: "json",
              url: "ajaxhandler/publishajax.aspx?type=someall",
              data: { ids: checkedCustomer.join(',') },
              success: function (d) {
                  if (!d.IsError) {
                      LG.showSuccess('发布文章成功');
                      LG.hideLoading();
                      f_reload();
                  }
              },
              error: function (message) {
                  LG.showError("未通过审核发布失败");

              }
          });
      }

      function f_copy() {

          $.ligerDialog.open({ height: 400,
              url: 'movecatalog.aspx',
              buttons: [
                { text: '确定', onclick: function (item, dialog) {
                    if (LG.cookies.get("copycatalogid") != "0" && LG.cookies.get("copycatalogid").toString().indexOf('site') < 0) //选择了栏目，且不是站点名
                    {
                        LG.showLoading("移动中");
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "ajaxhandler/articlemovedo.aspx",
                            data: { ids: checkedCustomer.join(',') },
                            success: function (d) {
                                if (!d.IsError) {
                                    LG.showSuccess('移动成功');
                                    LG.hideLoading();
                                    f_reload();
                                    dialog.close();
                                }
                                else {
                                    LG.showError(message);
                                    LG.hideLoading();
                                }
                            },
                            error: function (message) {
                                LG.showError(message);
                            }
                        });
                    }
                    else {
                        LG.tip('未选中移动栏目');
                    }
                }
                },
           { text: '取消', onclick: function (item, dialog) {
               dialog.close();
               LG.cookies.set("copycatalogid", "0", 1);
               LG.cookies.set("type", "0", 1);
           }
           }
             ], title: '移动到....', isResize: true
          });

      }

      function f_sort(id, type) {
          $.ajax({
              type: "post",
              dataType: "json",
              url: "ajaxhandler/articlesort.aspx",
              data: { ids: id, upordown: type },
              success: function (d) {
                  if (!d.IsError) {
                      f_reload();
                  }
                  else {
                      LG.showError(message);
                  }
              },
              error: function (message) {
                  LG.showError(message);
              }
          });
      }
          
  </script>
</body>
</html>
