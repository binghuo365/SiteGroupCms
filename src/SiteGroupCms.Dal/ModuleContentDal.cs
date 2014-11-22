
using System;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
    /// <summary>
    /// 模型内容业务类
    /// </summary>
    public class ModuleContentDAL : Common
    {
        public ModuleContentDAL()
        {
            base.SetupSystemDate();
        }
        /// <summary>
        /// 获得内容的某些属性(第一个是时间，第二个是内容页另名)
        /// </summary>
        /// <param name="_channelid">频道ID</param>
        /// <param name="_channeltype">频道模型</param>
        /// <param name="_contentid">内容ID</param>
        /// <returns></returns>
        public object[] GetSome(string _channelid, string _channeltype, string _contentid)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "ChannelId=" + _channelid + " and Id=" + _contentid;
                return _doh.GetFields("jcms_module_" + _channeltype, "AddDate,FirstPage,AliasPage");
            }
        }
    }
}
