using System;
namespace JumboTCMS
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

