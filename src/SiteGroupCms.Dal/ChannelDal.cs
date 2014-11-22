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
    /// 频道表信息
    /// </summary>
    public class Normal_ChannelDAL : Common
    {
        public Normal_ChannelDAL()
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
            using (DbOperHandler _doh = new Common().Doh())
            {
                int _ext = 0;
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
            }
            return (_ext == 1);
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
                int _del = _doh.Delete("yy_cataloginfo");
                return (_del == 1);
            }

        }
        /// <summary>
        /// 绑定记录至频道实体
        /// </summary>
        /// <param name="_id"></param>
        public SiteGroupCms.Entity.Normal_Channel GetEntity(DataRow dr)
        {
            SiteGroupCms.Entity.Normal_Channel channel = new SiteGroupCms.Entity.Normal_Channel();
                    channel.ID = Str2Int(dr["id"].ToString());
                    channel.Title = dr["title"].ToString();
                    channel.Father = Str2Int(dr["fatherid"].ToString());
                    channel.Linkurl = dr["linkurl"].ToString();
                    channel.Type = dr["type"].ToString();
                    channel.Picurl = dr["picurl"].ToString();
                    channel.Description = dr["description"].ToString();
                    channel.Meta_description = dr["meta_description"].ToString();
                    channel.Meta_Keywords = dr["meta_keywords"].ToString();
                    channel.IsShare = Str2Int(dr["isshare"].ToString());
                    channel.Listtemplate = Str2Int(dr["listtemplate"].ToString());
                    channel.Contentfileex = dr["contentfileex"].ToString();
                    channel.ContentTemplate =Str2Int(dr["contenttemplate"].ToString());
                    channel.Dirname = dr["dirname"].ToString();
            return channel;
        }

        public DataTable getchilren(string fatherid)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_cataloginfo] WHERE [fatherid]=" + fatherid;
                    return _doh.GetDataTable();
            }
        }

        /// <summary>
        /// 获得单页内容的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        public SiteGroupCms.Entity.Normal_Channel GetEntity(string _id)
        {

            SiteGroupCms.Entity.Normal_Channel channel = new SiteGroupCms.Entity.Normal_Channel();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_cataloginfo] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    channel.ID = Str2Int(_id);
                    channel.Title = dt.Rows[0]["title"].ToString();
                    channel.Father = Str2Int(dt.Rows[0]["fatherid"].ToString());
                    channel.Linkurl = dt.Rows[0]["linkurl"].ToString();
                    channel.Type =dt.Rows[0]["type"].ToString();
                    channel.Picurl = dt.Rows[0]["picurl"].ToString();
                    channel.Description = dt.Rows[0]["description"].ToString();
                    channel.Meta_description = dt.Rows[0]["meta_description"].ToString();
                    channel.Meta_Keywords = dt.Rows[0]["meta_keywords"].ToString();
                    channel.IsShare = Str2Int(dt.Rows[0]["isshare"].ToString());
                    channel.Listtemplate = Str2Int(dt.Rows[0]["listtemplate"].ToString());
                    channel.Contentfileex = dt.Rows[0]["contentfileex"].ToString();
                    channel.ContentTemplate = Str2Int(dt.Rows[0]["contenttemplate"].ToString());
                    channel.Dirname = dt.Rows[0]["dirname"].ToString();

                }
            }
            return channel;

        }
        public string GetChannelName(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT [Title] FROM [yy_cataloginfo] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Title"].ToString().ToLower();
                }
                return string.Empty;
            }

        }
        public string GetChannelType(string _id)
        {
            return "article";
        }
        public string GetChannelLink(string _channelid,int type) //type=0 输出a标签 type=1 不输出a标签
        {
            string linkurl = "";
            SiteGroupCms.Entity.Site site = (SiteGroupCms.Entity.Site)HttpContext.Current.Session["site"];
            string linkurltitle = "";
            SiteGroupCms.Entity.Catalog catalog = new SiteGroupCms.Dal.CatalogDal().GetEntity(_channelid);
            while (catalog.Father!=0)
            {
                if(type==0)
                linkurl = "&raquo;<a href="+GetChannelLink(catalog.ID.ToString(),1)+">" + catalog.Title + "</a>" + linkurl;
                else
                linkurltitle =catalog.Dirname + "/"+linkurltitle ;
                catalog = new SiteGroupCms.Dal.CatalogDal().GetEntity(catalog.Father.ToString());
            }
            if(type==0)
                linkurl = "<a href=" + GetChannelLink(catalog.ID.ToString(),1)+ ">" + catalog.Title + "</a>" + linkurl;
            else
            linkurltitle = "/sites/" + site.Location + "/pub/"+catalog.Dirname +"/"+linkurltitle;
            if (type == 0)
                return linkurl;
            else
                return linkurltitle;
        }
        /// <summary>
        /// 解析频道标签
        /// </summary>
        /// <param name="pagestr">原内容</param>
        /// <param name="_channelid">ChannelId不能为0</param>
        public void ExecuteTags(ref string PageStr, string _channelid)
        {
            SiteGroupCms.Entity.Normal_Channel _Channel = GetEntity(_channelid);
            ExecuteTags(ref PageStr, _Channel);
        }
        public void ExecuteTags(ref string PageStr, SiteGroupCms.Entity.Normal_Channel _Channel)
        {
            PageStr = PageStr.Replace("{$ChannelId}", _Channel.ID.ToString());
            PageStr = PageStr.Replace("{$ChannelName}", _Channel.Title);
            PageStr = PageStr.Replace("{$ChannelInfo}", _Channel.Description);
            PageStr = PageStr.Replace("{$ChannelType}", _Channel.Type);
            PageStr = PageStr.Replace("{$ChannelDir}", _Channel.Dirname);
            PageStr = PageStr.Replace("{$ChannelLink}", Go2Channel(_Channel.ID.ToString(),0));
            PageStr = PageStr.Replace("{$ChannelLinktitle}", Go2Channel(_Channel.ID.ToString(),1));
        }
       
    }
}
