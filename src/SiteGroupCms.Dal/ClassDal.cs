using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
    /// <summary>
    /// 栏目表信息
    /// </summary>
    public class Normal_ClassDAL : Common
    {
        public Normal_ClassDAL()
        {
            base.SetupSystemDate();
        }

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
                if (_doh.Exist("jcms_normal_class"))
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
                if (_doh.Exist("jcms_normal_class"))
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
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = _wherestr;
                string sqlStr = "";
                int _countnum = _doh.Count("jcms_normal_class");
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
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteByID(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                int _del = _doh.Delete("jcms_normal_class");
                return (_del == 1);
            }
        }
        /// <summary>
        /// 获得栏目的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        public SiteGroupCms.Entity.Normal_Class GetEntity(string _id)
        {
            return GetEntity(_id, "");
        }
        /// <summary>
        /// 获得栏目的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_wherestr">搜索条件</param>
        public SiteGroupCms.Entity.Normal_Class GetEntity(string _id, string _wherestr)
        {
            SiteGroupCms.Entity.Normal_Class _class = new SiteGroupCms.Entity.Normal_Class();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [jcms_normal_class] WHERE [Id]=" + _id;
                if (_wherestr != "") _doh.SqlCmd += " AND " + _wherestr;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    _class.Id = dt.Rows[0]["Id"].ToString();
                    _class.ChannelId = Validator.StrToInt(dt.Rows[0]["ChannelId"].ToString(), 0);
                    _class.ParentId = Validator.StrToInt(dt.Rows[0]["ParentId"].ToString(), 0);
                    _class.Title = dt.Rows[0]["Title"].ToString();
                    _class.Info = dt.Rows[0]["Info"].ToString();
                    _class.Img = dt.Rows[0]["Img"].ToString();
                    _class.FilePath = dt.Rows[0]["FilePath"].ToString();
                    _class.Code = dt.Rows[0]["Code"].ToString();
                    _class.IsPost = Validator.StrToInt(dt.Rows[0]["IsPost"].ToString(), 0) == 1;
                    _class.IsTop = Validator.StrToInt(dt.Rows[0]["IsTop"].ToString(), 0) == 1;
                    _class.TopicNum = Validator.StrToInt(dt.Rows[0]["TopicNum"].ToString(), 0);
                    _class.TemplateId = Str2Str(dt.Rows[0]["TemplateId"].ToString());
                    _class.ContentTemp = Str2Str(dt.Rows[0]["ContentTemp"].ToString());
                    _class.PageSize = Validator.StrToInt(dt.Rows[0]["PageSize"].ToString(), 0);
                    _class.IsOut = Validator.StrToInt(dt.Rows[0]["IsOut"].ToString(), 0) == 1;
                    _class.FirstPage = dt.Rows[0]["FirstPage"].ToString();
                    _class.ReadGroup = Validator.StrToInt(dt.Rows[0]["ReadGroup"].ToString(), 0);

                }
            }
            return _class;
        }
        /// <summary>
        /// 绑定数据到实体
        /// </summary>
        /// <param name="_id"></param>
        public void BindData2Entity(string _id, SiteGroupCms.Entity.Normal_Class _class)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [jcms_normal_class] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    _class.Id = dt.Rows[0]["Id"].ToString();
                    _class.ChannelId = Validator.StrToInt(dt.Rows[0]["ChannelId"].ToString(), 0);
                    _class.ParentId = Validator.StrToInt(dt.Rows[0]["ParentId"].ToString(), 0);
                    _class.Title = dt.Rows[0]["Title"].ToString();
                    _class.Info = dt.Rows[0]["Info"].ToString();
                    _class.Img = dt.Rows[0]["Img"].ToString();
                    _class.FilePath = dt.Rows[0]["FilePath"].ToString();
                    _class.Code = dt.Rows[0]["Code"].ToString();
                    _class.IsPost = Validator.StrToInt(dt.Rows[0]["IsPost"].ToString(), 0) == 1;
                    _class.IsTop = Validator.StrToInt(dt.Rows[0]["IsTop"].ToString(), 0) == 1;
                    _class.TopicNum = Validator.StrToInt(dt.Rows[0]["TopicNum"].ToString(), 0);
                    _class.TemplateId = Str2Str(dt.Rows[0]["TemplateId"].ToString());
                    _class.ContentTemp = Str2Str(dt.Rows[0]["ContentTemp"].ToString());
                    _class.PageSize = Validator.StrToInt(dt.Rows[0]["PageSize"].ToString(), 0);
                    _class.IsOut = Validator.StrToInt(dt.Rows[0]["IsOut"].ToString(), 0) == 1;
                    _class.FirstPage = dt.Rows[0]["FirstPage"].ToString();
                    _class.ReadGroup = Validator.StrToInt(dt.Rows[0]["ReadGroup"].ToString(), 0);
                }
            }
        }

        /// <summary>
        /// 获得指定栏目内容页数
        /// </summary>
        /// <param name="_channelid">频道ID</param>
        /// <param name="_classid">栏目ID</param>
        /// <param name="_includechild">是否包含子类内容</param>
        /// <returns></returns>
        public int GetContetPageCount(string _channelid, string _classid, bool _includechild)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                string _channeltype = new Normal_ChannelDAL().GetChannelType(_channelid);
                if (_channeltype.Length == 0) return 0;
                _doh.Reset();
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
            }
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
                string _classname = _doh.GetField("jcms_normal_class", "Title").ToString();
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
            return "";
          /*  SiteGroupCms.Entity.Normal_Class _Class = new SiteGroupCms.Dal.Normal_ClassDAL().GetEntity(_classid);
            if (!_Class.IsOut)
            {
                SiteGroupCms.Entity.Normal_Channel _Channel = new SiteGroupCms.Dal.Normal_ChannelDAL().GetEntity(_channelid);
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
                return _Class.FirstPage;
           */
        }
        /// <summary>
        /// 判断是否有下属栏目
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool HasChild(string _channelid, string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "channelid=" + _channelid + " AND parentid=" + _id;
                bool _haschild = (_doh.Exist("jcms_normal_class"));
                return _haschild;
            }
        }
        /// <summary>
        /// 获得某个频道的栏目树
        /// </summary>
        /// <param name="_channelid"></param>
        /// <param name="_parentid"></param>
        /// <param name="_includechild"></param>
        /// <returns></returns>
        public SiteGroupCms.Entity.Normal_ClassTree GetClassTree(string _channelid, string _classid, bool _includechild)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                return getTree(_doh, _channelid, _classid, _includechild);
            }
        }
        private SiteGroupCms.Entity.Normal_ClassTree getTree(DbOperHandler _doh, string _channelid, string _classid, bool _includechild)
        {
            SiteGroupCms.Entity.Normal_ClassTree _tree = new SiteGroupCms.Entity.Normal_ClassTree();
            SiteGroupCms.Entity.Normal_Channel _channel = new SiteGroupCms.Dal.Normal_ChannelDAL().GetEntity(_channelid);
            bool _channelishtml = true;
            if (_classid == "0")//表示从根节点开始
            {
                _tree.Id = _channel.ID.ToString();
                _tree.Name = _channel.Title;
                _tree.Link = Go2Channel(_channelid,0);
                _tree.RssUrl = "";
            }
            else
            {
                SiteGroupCms.Entity.Normal_Class _class = new SiteGroupCms.Dal.Normal_ClassDAL().GetEntity(_classid);
                _tree.Id = _classid;
                _tree.Name = _class.Title;
                _tree.Link = Go2Class(1, _channelishtml, _channelid, _classid, false);
              //  _tree.RssUrl = Go2Rss(1, false, _channelid, _classid);
            }
            _tree.HasChild = HasChild(_channelid, _classid);
            List<SiteGroupCms.Entity.Normal_ClassTree> subtree = new List<SiteGroupCms.Entity.Normal_ClassTree>();
            if (_includechild)
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT Id FROM [jcms_normal_class] WHERE [ChannelId]=" + _channelid + " AND [ParentId]=" + _classid + " order by code";
                DataTable dtClass = _doh.GetDataTable();
                for (int i = 0; i < dtClass.Rows.Count; i++)
                {
                    string _subclassid = dtClass.Rows[i]["Id"].ToString();
                    subtree.Add(getTree(_doh, _channelid, _subclassid, _includechild));
                }
                dtClass.Clear();
                dtClass.Dispose();
            }
            _tree.SubChild = subtree;
            return _tree;
        }
    }
}
