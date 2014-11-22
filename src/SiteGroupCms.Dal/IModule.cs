using System;
using System.Data;
using System.Web;
using System.Web.UI;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
    public interface IModule
    {
        /// <summary>
        /// 得到内容页地址
        /// </summary>>
        /// <param name="_contentid"></param>
        /// <returns></returns>
        string GetContentLink( string _contentid);
        /// <summary>
        /// 生成内容页
        /// </summary>
        /// <param name="_ContentId"></param>
        void CreateContent(string _ContentId);
        /// <summary>
        /// 得到内容页
        /// </summary>
        /// <param name="_ContentId"></param>
        string GetContent(string _ContentId);
        /// <summary>
        /// 删除内容页
        /// </summary>
        /// <param name="_ContentId"></param>
        void DeleteContent(string _ContentId);
    }
}
