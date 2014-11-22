using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{

    public class CatalogDal : Common
    {

        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="_wherestr">条件</param>
        /// <returns></returns>
        public bool Exists(string _wherestr)
        {
            int _ext = 0;
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = _wherestr;
                if (_doh.Exist("yy_cataloginfo"))
                    _ext = 1;
                return (_ext == 1);
            }

        }
        /// <summary>
        /// 判断重复性(标题是否存在)
        /// </summary>
        /// <param name="_title">需要检索的标题</param>
        /// <param name="_id">除外的ID</param>
        /// <param name="_wherestr">其他条件</param>
        /// <returns></returns>
        public bool ExistTitle(string _title, string _id, string _wherestr)
        {
            int _ext = 0;
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "title=@title and id<>" + _id;
                if (_wherestr != "") _doh.ConditionExpress += " and " + _wherestr;
                _doh.AddConditionParameter("@title", _title);
                if (_doh.Exist("yy_cataloginfo"))
                    _ext = 1;
                return (_ext == 1);
            }

        }
        /// <summary>
        /// 得到列表JSON数据
        /// </summary>
        /// <param name="_thispage">当前页码</param>
        /// <param name="_pagesize">每页记录条数</param>
        /// <param name="_wherestr">搜索条件</param>
        /// <param name="_jsonstr">返回值</param>
        public void GetListJSON(int _thispage, int _pagesize, string _wherestr, ref string _jsonstr)
        {
            /* using (DbOperHandler _doh = new Common().Doh())
             {
                 _doh.Reset();
                 _doh.ConditionExpress = _wherestr;
                 string sqlStr = "";
                 int _countnum = _doh.Count("yy_cataloginfo");
                 sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("[ID],[Title],[Source]", "jcms_normal_class", "Id", _pagesize, _thispage, "desc", _wherestr);
                 _doh.Reset();
                 _doh.SqlCmd = sqlStr;
                 DataTable dt = _doh.GetDataTable();
                 _jsonstr = "{result :\"1\"," +
                     "returnval :\"操作成功\"," +
                     "pagebar :\"" + SiteGroupCms.Utils.PageBar.GetPageBar(3, "js", 2, _countnum, _pagesize, _thispage, "javascript:ajaxList(<#page#>);") + "\"," +
                     SiteGroupCms.Utils.dtHelp.DT2JSON(dt, (_pagesize * (_thispage - 1))) +
                     "}";
                 dt.Clear();
                 dt.Dispose();
             }*/
        }
        /// <summary>
        /// 删除一条数据  先判断是否拥有子栏目，在判断是否含有文档，否则删除失败
        /// </summary>
        public int DeleteByID(string _id) //1,成功，2，含有子栏目，3，含有文章
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_cataloginfo] WHERE [fatherid]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                    return 2;
                _doh.Reset();
                _doh.SqlCmd = "select * from [yy_articleinfo] where [catalogid]=" + _id;
                dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                    return 3;

                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                int _del = _doh.Delete("yy_cataloginfo");
                return 1;
            }
        }
        /// <summary>
        /// 获得栏目的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        public SiteGroupCms.Entity.Catalog GetEntity(string _id)
        {
            return GetEntity(_id, "");
        }
        /// <summary>
        /// 获得栏目的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_wherestr">搜索条件</param>
        public SiteGroupCms.Entity.Catalog GetEntity(string _id, string _wherestr)
        {
            SiteGroupCms.Entity.Catalog catalog = new SiteGroupCms.Entity.Catalog();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_cataloginfo] WHERE [Id]=" + _id;
                if (_wherestr != "") _doh.SqlCmd += " AND " + _wherestr;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    catalog.ID = Str2Int(_id);
                    catalog.Title = dt.Rows[0]["title"].ToString();
                    catalog.Father = Str2Int(dt.Rows[0]["fatherid"].ToString());
                    catalog.Linkurl = dt.Rows[0]["linkurl"].ToString();
                    catalog.Type = dt.Rows[0]["type"].ToString();
                    catalog.Picurl = dt.Rows[0]["picurl"].ToString();
                    catalog.Description = dt.Rows[0]["description"].ToString();
                    catalog.Meta_description = dt.Rows[0]["meta_description"].ToString();
                    catalog.Meta_Keywords = dt.Rows[0]["meta_keywords"].ToString();
                    catalog.IsShare = Str2Int(dt.Rows[0]["isshare"].ToString());
                    catalog.Dirname = dt.Rows[0]["dirname"].ToString();
                    catalog.Siteid = Str2Int(dt.Rows[0]["siteid"].ToString());
                    catalog.Listtemplate = Str2Int(dt.Rows[0]["listtemplate"].ToString());
                    catalog.ContentTemplate = Str2Int(dt.Rows[0]["contenttemplate"].ToString());
                    catalog.Contentfileex = dt.Rows[0]["contentfileex"].ToString();


                }
            }
            return catalog;
        }

        public DataTable GetDT(string _wherestr, string _order)
        {
            SiteGroupCms.Entity.Catalog catalog = new SiteGroupCms.Entity.Catalog();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_cataloginfo] WHERE " + _wherestr + " order by " + _order;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }

        }
        /// <summary>
        /// 绑定数据到实体
        /// </summary>
        /// <param name="_id"></param>
        public void BindData2Entity(string _id, SiteGroupCms.Entity.Catalog catalog)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_cataloginfo] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    catalog.ID = Str2Int(_id);
                    catalog.Title = dt.Rows[0]["title"].ToString();
                    catalog.Father = Str2Int(dt.Rows[0]["fatherid"].ToString());
                    catalog.Linkurl = dt.Rows[0]["linkurl"].ToString();
                    catalog.Type = dt.Rows[0]["type"].ToString();
                    catalog.Picurl = dt.Rows[0]["picurl"].ToString();
                    catalog.Description = dt.Rows[0]["description"].ToString();
                    catalog.Meta_description = dt.Rows[0]["meta_description"].ToString();
                    catalog.Meta_Keywords = dt.Rows[0]["meta_keywords"].ToString();
                    catalog.IsShare = Str2Int(dt.Rows[0]["isshare"].ToString());
                    catalog.Listtemplate = Str2Int(dt.Rows[0]["listtemplate"].ToString());
                    catalog.Contentfileex = dt.Rows[0]["contentfileex"].ToString();
                    catalog.Dirname = dt.Rows[0]["dirname"].ToString();
                    catalog.Siteid = Str2Int(dt.Rows[0]["siteid"].ToString());
                    catalog.ContentTemplate = Str2Int(dt.Rows[0]["contenttemplate"].ToString());
                }
            }
        }

        /// <summary>
        /// 获得指定栏目内容页数
        /// </summary>
        /// <param name="_siteid">站点ID</param>
        /// <param name="_classid">栏目ID</param>
        /// <param name="_includechild">是否包含子类内容</param>
        /// <returns></returns>
        public int GetContetPageCount(string _siteid, string _catalogid, bool _includechild)
        {
            return 1;
            /*  using (DbOperHandler _doh = new Common().Doh())
              {

                  _doh.SqlCmd = "SELECT [PageSize],[Code] FROM [jcms_normal_class] WHERE [ChannelId]=" + _channelid + " AND [Id]=" + _classid;
                  int _pagesize = SiteGroupCms.Utils.Validator.StrToInt(_doh.GetDataTable().Rows[0]["PageSize"].ToString(), 0);
                  string _classcode = _doh.GetDataTable().Rows[0]["Code"].ToString();
                  if (_pagesize == 0) _pagesize = 20;
                  string _pstr = string.Empty;
                  if (!_includechild)
                      _pstr = " [ClassID]=" + _classid + " AND [IsPass]=1 AND [ChannelId]=" + _channelid;
                  else
                      _pstr = " [ClassID] in (Select id FROM [jcms_normal_class] WHERE [Code] LIKE '" + _classcode + "%') AND [IsPass]=1 AND [ChannelId]=" + _channelid;
                  _doh.Reset();
                  _doh.ConditionExpress = _pstr;
                  int _totalcount = _doh.Count("jcms_module_" + _channeltype);
                  return SiteGroupCms.Utils.Int.PageCount(_totalcount, _pagesize);
              }*/
        }
        /// <summary>
        /// 获得栏目名称
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public string GetClassName(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=" + _id;
                string _classname = _doh.GetField("yy_catalog", "Title").ToString();
                return _classname;
            }
        }
        /// <summary>
        /// 链接到栏目页
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_ishtml"></param>
        /// <param name="_channelid"></param>
        /// <param name="_classid"></param>
        /// <returns></returns>
        public string GetClassLink(int _page, bool _ishtml, string _channelid, string _classid, bool _truefile)
        {
            /*
            SiteGroupCms.Entity.Normal_Class _Class = new SiteGroupCms.DAL.Normal_ClassDAL().GetEntity(_classid);
            if (!_Class.IsOut)
            {
                SiteGroupCms.Entity.Normal_Channel _Channel = new SiteGroupCms.DAL.Normal_ChannelDAL().GetEntity(_channelid);
                string TempUrl = SiteGroupCms.Common.PageFormat.Class(_ishtml, site.Dir, site.UrlReWriter, _page);
                if ((_Channel.SubDomain.Length > 0) && (!_truefile))
                    TempUrl = TempUrl.Replace("<#SiteDir#><#ChannelDir#>", _Channel.SubDomain);
                TempUrl = TempUrl.Replace("<#SiteDir#>", site.Dir);
                TempUrl = TempUrl.Replace("<#SiteStaticExt#>", site.StaticExt);
                TempUrl = TempUrl.Replace("<#ChannelId#>", _channelid);
                TempUrl = TempUrl.Replace("<#ChannelDir#>", _Channel.Dir.ToLower());
                TempUrl = TempUrl.Replace("<#ChannelType#>", _Channel.Type.ToLower());
                TempUrl = TempUrl.Replace("<#ClassFilePath#>", _Class.FilePath.ToLower());
                TempUrl = TempUrl.Replace("<#id#>", _classid);
                if (_page > 0) TempUrl = TempUrl.Replace("<#page#>", _page.ToString());
                return TempUrl;
            }
            else
                return _Class.FirstPage;*/
            return "";
        }
        /// <summary>
        /// 判断是否有下属栏目
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool HasChild(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "fatherid=" + _id;
                bool _haschild = (_doh.Exist("yy_cataloginfo"));
                return _haschild;
            }
        }
        /// <summary>
        /// 获得某个频道的栏目树
        /// </summary>
        /// <param name="_siteid"></param>
        /// <param name="_parentid"></param>
        /// <param name="_includechild"></param>
        /// <returns></returns>
        public SiteGroupCms.Entity.Catalogtree GetClassTree(string _siteid, string _classid, bool _includechild)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                return getTree(_siteid, _classid, _includechild);
            }
        }
        private SiteGroupCms.Entity.Catalogtree getTree(string _siteid, string _classid, bool _includechild)
        {
            SiteGroupCms.Entity.Catalogtree _tree = new SiteGroupCms.Entity.Catalogtree();
            SiteGroupCms.Entity.Site _site = new SiteGroupCms.Dal.SiteDal().GetEntity(Str2Int(_siteid));
            if (_site == null)
                return null;
            if (_classid == "0")//表示从根节点开始
            {
                _tree.Id = _site.ID.ToString();
                _tree.Name = _site.Title;
            }
            else
            {
                SiteGroupCms.Entity.Catalog _catalog = new SiteGroupCms.Dal.CatalogDal().GetEntity(_classid);
                SiteGroupCms.Entity.Admin _admin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
                if (_admin.Catalogid.Trim() == ""  || ((_admin.Catalogid.IndexOf(_catalog.ID + ",") >= 0) && (_catalog.Father == 0)) || _catalog.Father != 0)//超级管理或者包含该栏目则显示
                {
                    _tree.Id = _catalog.ID.ToString();
                    _tree.Name = _catalog.Title;
                }
            }

            _tree.HasChild = HasChild(_classid);
            List<SiteGroupCms.Entity.Catalogtree> subtree = new List<SiteGroupCms.Entity.Catalogtree>();
            if (_includechild)
            {
                using (DbOperHandler _doh = new Common().Doh())
                {
                    _doh.Reset();
                    _doh.SqlCmd = "SELECT id FROM [yy_cataloginfo] WHERE [siteid]=" + _siteid + " AND [fatherid]=" + _classid + " order by sort desc";
                    DataTable dtClass = _doh.GetDataTable();
                    for (int i = 0; i < dtClass.Rows.Count; i++)
                    {
                        string _subclassid = dtClass.Rows[i]["Id"].ToString();
                        SiteGroupCms.Entity.Admin _admin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
                        if (_admin.Catalogid == "" || _admin.Catalogid.IndexOf(_subclassid + ",") >= 0 || _classid != "0")//超级管理或者包含该栏目则显示
                        {
                            subtree.Add(getTree(_siteid, _subclassid, _includechild));
                        }
                    }
                    dtClass.Clear();
                    dtClass.Dispose();
                }
            }
            _tree.SubChild = subtree;
            return _tree;
        }
        public string getcataloglist(string siteid)
        {
            string str = "{\"catalog\":[";
            SiteGroupCms.Entity.Catalog catalog = new SiteGroupCms.Entity.Catalog();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_cataloginfo] WHERE [siteid]=" + siteid;
                DataTable dt = _doh.GetDataTable();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SiteGroupCms.Entity.Admin _admin = ((SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"]);
                        if (_admin.Catalogid == "" || _admin.Catalogid.IndexOf(dt.Rows[i]["id"].ToString() + ",") >= 0)//超级管理或者包含该栏目则显示
                        {
                            str += "{\"id\":\"" + dt.Rows[i]["id"].ToString() + "\",\"title\":\"" + dt.Rows[i]["title"].ToString() + "\"},";
                        }
                    }


                }
            }
            return str.Remove(str.Length - 1, 1) + "]}";
        }

        public int insertcatalog(SiteGroupCms.Entity.Catalog obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.AddFieldItem("title", obj.Title);
                _doh.AddFieldItem("fatherid", obj.Father);
                _doh.AddFieldItem("description", obj.Description);
                _doh.AddFieldItem("siteid", obj.Siteid);
                _doh.AddFieldItem("dirname", obj.Dirname);
                _doh.AddFieldItem("isshare", obj.IsShare);
                _doh.AddFieldItem("meta_keywords", obj.Meta_Keywords);
                _doh.AddFieldItem("contenttemplate", obj.ContentTemplate);
                _doh.AddFieldItem("listtemplate", obj.Listtemplate);
                int _insert = _doh.Insert("yy_cataloginfo");
                return _insert;
            }
        }
        public bool update(SiteGroupCms.Entity.Catalog obj)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", obj.ID);
                _doh.AddFieldItem("title", obj.Title);
                // _doh.AddFieldItem("fatherid", obj.Father);不允许修改父级栏目
                _doh.AddFieldItem("description", obj.Description);
                //  _doh.AddFieldItem("siteid", obj.Siteid);//不允许修改站点
                //  _doh.AddFieldItem("dirname", obj.Dirname);//不允许修改栏目目录
                _doh.AddFieldItem("isshare", obj.IsShare);
                _doh.AddFieldItem("meta_keywords", obj.Meta_Keywords);
                _doh.AddFieldItem("contenttemplate", obj.ContentTemplate);
                _doh.AddFieldItem("listtemplate", obj.Listtemplate);
                int _update = _doh.Update("yy_cataloginfo");
                return (_update == 1);
            }
        }
        /// <summary>
        /// 取得栏目需要生成的静态文章的数目
        /// </summary>
        /// <param name="catalogid"></param>
        /// <returns></returns>
        public int getpublishcontnum(string catalogid)
        {
            DataTable dt = new ArticleDal().getDT("catalogid=" + catalogid + " and ispassed=1");
            return dt.Rows.Count;
        }

        /// <summary>
        /// 取出栏目中需要生成的文件的总数目
        /// 包括文字也 栏目页 子集栏目页 和首页
        /// </summary>
        /// <returns></returns>
        public int getcatalogneespublishnum(string catalogid)//catalogid=0 表示从根站点开始
        {
            int total = 1;//当前栏目页
            SiteGroupCms.Entity.Site _site = (SiteGroupCms.Entity.Site)HttpContext.Current.Session["site"];
            SiteGroupCms.Entity.Catalogtree catalogtree = getTree(_site.ID.ToString(), catalogid, true);
            //先计算所有需要发布的文档
            total += getpublishcontnum(catalogid);
            //在计算自己栏目
            for (int i = 0; i < catalogtree.SubChild.Count; i++)
            {
                total += getcatalogneespublishnum(catalogtree.SubChild[i].Id);
            }
            return total;
        }
    }
}
