/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序版本: 5.2.X
 * 
 * 程序作者: 高伟 ( 合作请联系：254860396#qq.com)
 * 
 * 
 * 
 * 
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace SiteGroupCms.Utils
{
    /// <summary>
    /// 分词类
    /// </summary>
    public static class WordSpliter
    {
        /// <summary>
        /// 得到分词关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKeyword(string key, string splitchar)
        {
            SiteGroupCms.Utils.ShootSeg.Segment seg = new SiteGroupCms.Utils.ShootSeg.Segment();
            seg.InitWordDics();
            seg.EnablePrefix = true;
            seg.Separator = splitchar;
            return seg.SegmentText(key, false).Trim();
        }
        public static string GetKeyword(string key)
        {
            return GetKeyword(key," ");
        }
    }
}