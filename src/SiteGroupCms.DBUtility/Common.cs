/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序作者: SiteGroupCms高伟 254850396#qq.com
 */

using System;
using System.Text.RegularExpressions;
namespace SiteGroupCms.DBUtility
{
    /// <summary>
    /// 枚举，作为Web中常用的用户操作类型。常用于权限相关的判断。
    /// </summary>
    public enum OperationType : byte { Add, Modify, Delete, Audit, Enable };
}