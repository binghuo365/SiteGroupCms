/*
 * 程序中文名称: 将博内容管理系统通用版
 * 
 * 程序英文名称: JumboTCMS
 * 
 * 程序版本: 5.2.X
 * 
 * 程序编写: 随风缘 (定制开发请联系：jumbot114@126.com,不接受无偿的技术答疑,请见谅)
 * 
 * 官方网站: http://www.jumbotcms.net/
 * 
 * 商业服务: http://www.jumbotcms.net/about/service.html
 * 
 */

using System;
namespace JumboTCMS.Entity
{
    /// <summary>
    /// 语言包实体（主要用以解析程序自动生成内容中的相关信息）
    /// </summary>

    public class Language
    {
        public Language()
        { }

        private string _home;
        private string _more;
        /// <summary>
        /// 首页
        /// </summary>
        public string Home
        {
            set { _home = value; }
            get { return _home; }
        }
        /// <summary>
        /// 更多
        /// </summary>
        public string More
        {
            set { _more = value; }
            get { return _more; }
        }
    }
}

