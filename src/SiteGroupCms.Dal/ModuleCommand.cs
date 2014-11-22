using System;
using System.Data;
namespace SiteGroupCms.Dal
{
    public class ModuleCommand
    {
        public static IModule IMD;
        static ModuleCommand()
        {

        }
        /// <summary>
        /// 得到内容页地址
        /// </summary>
        /// <param name="_channelid"></param>
        /// <param name="_contentid"></param>
        /// <returns></returns>
        public static string GetContentLink(string _contentid)
        {
            IMD = (IModule)Activator.CreateInstance(Type.GetType(String.Format("SiteGroupCms.DAL.Module_{0}DAL", "article"), true, true));
            return IMD.GetContentLink( _contentid);
        }
        /// <summary>
        /// 生成内容页
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <param name="_ContentId"></param>
        /// <param name="_CurrentPage"></param>
        public static void CreateContent(string _ContentId)
        {
            IMD = (IModule)Activator.CreateInstance(Type.GetType(String.Format("SiteGroupCms.Dal.Module_{0}DAL", "article"), true, true));
            IMD.CreateContent(_ContentId);
        }
        /// <summary>
        /// 得到内容页
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <param name="_ContentId"></param>
        /// <param name="_CurrentPage"></param>
        public static string GetContent(string _ContentId)
        {
            IMD = (IModule)Activator.CreateInstance(Type.GetType(String.Format("SiteGroupCms.DAL.Module_{0}DAL", "article"), true, true));
            return IMD.GetContent(_ContentId);
        }
        /// <summary>
        /// 删除内容页
        /// </summary>
        /// <param name="_ChannelId"></param>
        /// <param name="_ContentId"></param>
        public static void DeleteContent(string _ContentId)
        {
            IMD = (IModule)Activator.CreateInstance(Type.GetType(String.Format("SiteGroupCms.DAL.Module_{0}DAL", "article"), true, true));
            IMD.DeleteContent(_ContentId);
        }
    }
}