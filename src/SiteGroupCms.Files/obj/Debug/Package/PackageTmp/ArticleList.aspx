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
        <span>����</span><img src="lib/icons/32X32/searchtool.gif" />
        <div class="togglebtn"></div> 
    </div>
    <div class="navline" style="margin-bottom:4px; margin-top:4px;"></div>
    <div class="searchbox">
<form id="formsearch" class="l-form">
       <table>
<tr>
<td width=10></td>
<td>����</td>
<td width=10></td>
<td><input type="text" id="searchtitle" name="searchtitle" ></td>
<td width=10></td>
<td>��Ŀ</td>
<td width=10></td>
<td><select  id="searchcatalogid" name="searchcatalog">

</select></td>

<td width=10></td>
<td>״̬</td>
<td width=10></td>
<td><select name="" id="searchstate" name="searchstate">
 <option value="0">���ѡ��</option>
 <option value="1">δͨ�����</option>
 <option value="2">ͨ�����</option>
 <option value="3">����</option>
 <option value="4">δ����</option>
 <option value="5">��������</option>
 <option value="6">�Ƽ�����</option>
 <option value="7">�õ�Ƭ����</option>
 <option value="8">��������</option>

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
          //�����˵�
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
          //���ز˵���
      });

      var type = "";
      var catalogid = "";
      type = getQueryStringByName("type");
      catalogid = getQueryStringByName("catalogid");
      //���·��
      var rootPath = "../";
      //�б�ṹ
      var grid = $("#maingrid").ligerGrid({
          columns: [
          { display: "����", name: "title", width: "34%", align: "left" },
          { display: "����", name: "author", width: "5%" },
          { display: "�ɼ�ʱ��", name: "addtime", type: "string", width: "11%" },
          { display: "����Ƶ��", name: "catalogid", width: "8%" },
          { display: "����", name: "articletype", width: "5%" },
          { display: "�����", name: "clickcount", width: "4%" },
          { display: "����", name: "state", width: "14%" },
          { display: '����', isAllowHide: false, width: "4%",
              render: function (row) {
                  var html = '<a  href="javascript:f_sort(' + row.id + ',1)">��</a>&nbsp;';
                  html += '<a href="javascript:f_sort(' + row.id + ',0)">��</a>';
                  return html;
              }
          },
          { display: '����', isAllowHide: false, width: "9%",
              render: function (row) {
                  var html = '<a  href="javascript:f_modify(' + row.id + ')">�޸�</a>&nbsp;';
                  html += '<a href="javascript:f_view(' + row.id + ')">�鿴</a>&nbsp;';
                  html += '<a href="javascript:f_sort(' + row.id + ',3)">�ö�</a>';
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
      //����������ť,�������¼�  
      LG.appendSearchButtons("#formsearch", grid);
      //����toolbar
      LG.loadToolbar(grid, toolbarBtnItemClick);
      //�������¼�
      function toolbarBtnItemClick(item) {
          switch (item.id) {
              case "add":
                  top.f_addTab(null, '�ɼ�����', 'articleEidt.aspx?method=add&catalogid=' + catalogid);
                  break;
              case "view":
                  if (checkedCustomer.length == 0) { LG.tip('��ѡ����!'); return; }
                  if (checkedCustomer.length > 1) { LG.tip('��ѡ���˶���!'); return; }
                  f_view("0");
                  break;
              case "modify": //�޸�����
                  if (checkedCustomer.length == 0) { LG.tip('��ѡ����!'); return; }
                  if (checkedCustomer.length > 1) { LG.tip('��ѡ���˶���!'); return; }
                  f_modify("0");
                  break;
              case "copy":
                  if (checkedCustomer.length == 0) { LG.tip('��ѡ����!'); return; }
                  f_copy();
                  break;
              case "pass":
                  jQuery.ligerDialog.confirm('ȷ��ͨ�������?', function (confirm) {
                      if (confirm)
                          f_pass();
                  });
                  break;
              case "reject":
                  jQuery.ligerDialog.confirm('ȷ������?', function (confirm) {
                      if (confirm)
                          f_reject();
                  });
                  break;
              case "publish":
                  jQuery.ligerDialog.confirm('ȷ��������?', function (confirm) {
                      if (confirm)
                          f_publish();
                  });
                  break;
              case "share":
                  jQuery.ligerDialog.confirm('ȷ������?', function (confirm) {
                      if (confirm)
                          f_share();
                  });
                  break;
              case "delete":
                  jQuery.ligerDialog.confirm('ȷ��ɾ����?', function (confirm) {
                      if (confirm)
                          f_delete();
                  });
                  break;
              case "cddelete":
                  jQuery.ligerDialog.confirm('ȷ������ɾ����?', function (confirm) {
                      if (confirm)
                          f_cddelete();
                  });
                  break;
              case "huishou":
                  jQuery.ligerDialog.confirm('ȷ��������?', function (confirm) {
                      if (confirm)
                          f_huishou();
                  });
                  break;
              case "ipass":
                  loadipass();
                  break;
              case "allpublish":
                  jQuery.ligerDialog.confirm('ȷ��������?', function (confirm) {
                      if (confirm)
                          allpublish();
                  });
                  break;
          }
      }

      //������Ŀ�б�
      $.ajax({
          type: "post",
          dataType: "json",
          url: "ajaxhandler/loadcataloglist.aspx",
          data: { siteid: 1 },
          success: function (d) {
              $("#searchcatalogid").append("<option value='0'>���ѡ��</option>");
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
      ������ʵ�� ����ҳ��ѡ
      ������onCheckRow��ѡ�е��м���������������isChecked�������������г�ʼ��ѡ��
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
                  loading: '����ɾ����...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('ɾ���ɹ�');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }

      function f_modify(id) {
          if (id != "0")
              checkedCustomer[0] = id;
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'getyiyongid',
                  loading: '������...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function (d) {
                      if (d.yyarticleid == "" || d.yyarticleid == "0")
                          top.f_addTab(null, '�޸�����', 'articleEidt.aspx?method=update&id=' + checkedCustomer.join(','));
                      else
                          LG.showError('�������²����޸�');
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }

      function f_pass() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'passarticle',
                  loading: '������...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('ȫ��ͨ�����');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }
      function f_reject() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'rejectarticle',
                  loading: '������...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('�����³ɹ�');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }
      function f_share() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'sharearticle',
                  loading: '������...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('�������³ɹ�');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }
      function f_view(id) {
          if (id != "0")
              checkedCustomer[0] = id;
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'getarticlelink',
                  loading: '������...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function (d) {
                      window.open(d.art);
                  },
                  error: function (d) {
                      LG.showError("��δ����");
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }
      function f_huishou() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'huishou',
                  loading: '������...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('�������³ɹ�');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }
      function f_cddelete() {
          if (checkedCustomer.length > 0) {
              LG.ajax({
                  type: 'AjaxBaseManage',
                  method: 'cddelete',
                  loading: '������...',
                  data: { ID: checkedCustomer.join(',') },
                  success: function () {
                      LG.showSuccess('����ɾ�����³ɹ�');
                      f_reload();
                  },
                  error: function (message) {
                      LG.showError(message);
                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }

      function f_publish() {
          if (checkedCustomer.length > 0) {
              LG.showLoading("δͨ����˵����²��ᷢ�������������½϶�ʱ��Ҫһ��ʱ��");
              $.ajax({
                  type: "post",
                  dataType: "json",
                  url: "ajaxhandler/publishajax.aspx?type=some",
                  data: { ids: checkedCustomer.join(',') },
                  success: function (d) {
                      if (!d.IsError) {
                          LG.showSuccess('�������³ɹ�');
                          LG.hideLoading();
                          f_reload();
                      }
                  },
                  error: function (message) {
                      LG.showError("δͨ����˷���ʧ��");
                      LG.hideLoading();

                  }
              });
          }
          else {
              LG.tip('��ѡ����!');
          }
      }

      function loadipass() {
          grid.set('parms', { paras: "ipass" });
          grid.loadData();
      }

      function allpublish() {
          LG.showLoading("δͨ����˵����²��ᷢ�������������½϶�ʱ��Ҫһ��ʱ��");
          $.ajax({
              type: "post",
              dataType: "json",
              url: "ajaxhandler/publishajax.aspx?type=someall",
              data: { ids: checkedCustomer.join(',') },
              success: function (d) {
                  if (!d.IsError) {
                      LG.showSuccess('�������³ɹ�');
                      LG.hideLoading();
                      f_reload();
                  }
              },
              error: function (message) {
                  LG.showError("δͨ����˷���ʧ��");

              }
          });
      }

      function f_copy() {

          $.ligerDialog.open({ height: 400,
              url: 'movecatalog.aspx',
              buttons: [
                { text: 'ȷ��', onclick: function (item, dialog) {
                    if (LG.cookies.get("copycatalogid") != "0" && LG.cookies.get("copycatalogid").toString().indexOf('site') < 0) //ѡ������Ŀ���Ҳ���վ����
                    {
                        LG.showLoading("�ƶ���");
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            url: "ajaxhandler/articlemovedo.aspx",
                            data: { ids: checkedCustomer.join(',') },
                            success: function (d) {
                                if (!d.IsError) {
                                    LG.showSuccess('�ƶ��ɹ�');
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
                        LG.tip('δѡ���ƶ���Ŀ');
                    }
                }
                },
           { text: 'ȡ��', onclick: function (item, dialog) {
               dialog.close();
               LG.cookies.set("copycatalogid", "0", 1);
               LG.cookies.set("type", "0", 1);
           }
           }
             ], title: '�ƶ���....', isResize: true
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
