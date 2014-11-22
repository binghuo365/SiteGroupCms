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
namespace SiteGroupCms.Utils
{
    /// <summary>
    /// CKEditorToolbar
    /// </summary>
    public static class CKEditorToolbar
    {
        /// <summary>
        /// 简单模式
        /// </summary>
        public static object[] Simple
        {
            get
            {
                return new object[]{
                new object[] {"Source", "-", "JustifyLeft", "JustifyCenter", "JustifyRight", "-","Styles","FontSize",},
				new object[] { "Bold", "Italic", "-", "NumberedList", "BulletedList", "-", "Link", "Unlink"},
			};
            }
        }
        /// <summary>
        /// 最简模式
        /// </summary>
        public static object[] Basic
        {
            get
            {
                return new object[]{
				new object[] { "Bold","Italic","-","OrderedList","UnorderedList","-","Link","Unlink"}
                };
            }
        }
    }
}
