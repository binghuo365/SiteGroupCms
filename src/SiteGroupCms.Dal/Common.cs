using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
    /// <summary>
    /// 作为dal层的基础类，包含数值转换等 一些基本的方法
    /// </summary>
    public class Common
    {
        public static string connectionString = Const.ConnectionString;
        public string DBType = "1";
        public string ORDER_BY_RND()
        {
            /*Access版本的随机没Sql Server的好，凑合着用吧
             * */
            if (DBType == "0")
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                return " ORDER BY rnd(-(id+" + rand.Next(99999) + "))";
            }
            else
                return " ORDER BY newid()";
        }
        public string vbCrlf = "\r\n";//换行符
        protected SiteGroupCms.Entity.Site site=new SiteGroupCms.Entity.Site();
        public Common()
        {
             
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_formatsystem">是否初始化site</param>
        public Common(bool _formatsystem)
        {
           
        }
        /// <summary>
        /// 初始化系统信息
        /// </summary>
        protected void SetupSystemDate()
        {
          
        }
        public DbOperHandler Doh()
        {

            return new SqlDbOperHandler(new SqlConnection(connectionString));
        }
        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="int_NumberLength">数字长度</param>
        /// <returns></returns>
        public string GetRandomNumberString(int int_NumberLength)
        {
            return GetRandomNumberString(int_NumberLength, false);
        }
        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="int_NumberLength">数字长度</param>
        /// <returns></returns>
        public string GetRandomNumberString(int int_NumberLength, bool onlyNumber)
        {
            Random random = new Random();
            return GetRandomNumberString(int_NumberLength, onlyNumber, random);
        }
        /// <summary>
        /// 生成随机数字字符串
        /// </summary>
        /// <param name="int_NumberLength">数字长度</param>
        /// <returns></returns>
        public string GetRandomNumberString(int int_NumberLength, bool onlyNumber, Random random)
        {
            string strings = "123456789";
            if (!onlyNumber) strings += "abcdefghjkmnpqrstuvwxyz";
            char[] chars = strings.ToCharArray();
            string returnCode = string.Empty;
            for (int i = 0; i < int_NumberLength; i++)
                returnCode += chars[random.Next(0, chars.Length)].ToString();
            return returnCode;
        }
     
        /// <summary>
        /// 产生随机数字字符串
        /// </summary>
        /// <returns></returns>
        public string RandomStr(int Num)
        {
            int number;
            char code;
            string returnCode = String.Empty;

            Random random = new Random();

            for (int i = 0; i < Num; i++)
            {
                number = random.Next();
                code = (char)('0' + (char)(number % 10));
                returnCode += code.ToString();
            }
            return returnCode;
        }
        /// <summary>
        /// 执行Sql脚本文件
        /// </summary>
        /// <param name="pathToScriptFile">物理路径</param>
        /// <returns></returns>
        public bool ExecuteSqlInFile(string pathToScriptFile)
        {
            return SiteGroupCms.Utils.ExecuteSqlBlock.Go(DBType, connectionString, pathToScriptFile);

        }
        /// <summary>
        /// 获得翻页Bar，适合js和html
        /// </summary>
        /// <param name="mode">支持1=simple,2=normal,3=full</param>
        /// <param name="stype"></param>
        /// <param name="countNum"></param>
        /// <param name="PSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="Http"></param>
        /// <returns></returns>
        public string getPageBar(int mode, string stype, int stepNum, int countNum, int PSize, int currentPage, string HttpN)
        {
            return SiteGroupCms.Utils.PageBar.GetPageBar(mode, stype, stepNum, countNum, PSize, currentPage, HttpN);
        }
        /// <summary>
        /// 获得翻页Bar，适合js和html
        /// </summary>
        /// <param name="mode">支持1=simple,2=normal,3=full</param>
        /// <param name="stype"></param>
        /// <param name="stepNum"></param>
        /// <param name="countNum"></param>
        /// <param name="PSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="Http1"></param>
        /// <param name="HttpM"></param>
        /// <param name="HttpN"></param>
        /// <param name="limitPage"></param>
        /// <returns></returns>
        public string getPageBar(int mode, string stype, int stepNum, int countNum, int PSize, int currentPage, string Http1, string HttpM, string HttpN, int limitPage)
        {
            return SiteGroupCms.Utils.PageBar.GetPageBar(mode, stype, stepNum, countNum, PSize, currentPage, Http1, HttpM, HttpN, limitPage);
        }

        /// <summary>
        /// 获取querystring
        /// </summary>
        /// <param name="s">参数名</param>
        /// <returns>返回值</returns>
        public string q(string s)
        {
            if (HttpContext.Current.Request.QueryString[s] != null && HttpContext.Current.Request.QueryString[s] != "")
            {
                return HttpContext.Current.Request.QueryString[s].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取post得到的参数
        /// </summary>
        /// <param name="s">参数名</param>
        /// <returns>返回值</returns>
        public string f(string s)
        {
            if (HttpContext.Current.Request.Form[s] != null && HttpContext.Current.Request.Form[s] != "")
            {
                return HttpContext.Current.Request.Form[s].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回非负整数，默认为t
        /// </summary>
        /// <param name="s">参数值</param>
        /// <returns>返回值</returns>
        public int Str2Int(string s, int t)
        {
            return SiteGroupCms.Utils.Validator.StrToInt(s, t);
        }

        /// <summary>
        /// 返回非负整数，默认为0
        /// </summary>
        /// <param name="s">参数值</param>
        /// <returns>返回值</returns>
        public int Str2Int(string s)
        {
            return Str2Int(s, 0);
        }

        /// <summary>
        /// 返回非空字符串，默认为"0"
        /// </summary>
        /// <param name="s">参数值</param>
        /// <returns>返回值</returns>
        public string Str2Str(string s)
        {
            return SiteGroupCms.Utils.Validator.StrToInt(s, 0).ToString();
        }
     
        /// <summary>
        /// 字符串截断
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length">以汉字计算，比如Length为100表示取200个字符，100个汉字</param>
        /// <returns></returns>
        protected string GetCutString(string str, int Length)
        {
            Length *= 2;
            byte[] bs = System.Text.Encoding.Default.GetBytes(str);//请勿随意改编码，否则计算有误
            if (bs.Length <= Length)
            {
                return str;
            }
            else
            {
                return System.Text.Encoding.Default.GetString(bs, 0, Length);//请勿随意改编码，否则计算有误
            }
        }
        #region 保存Js文件
        /// <summary>
        /// 保存js文件
        /// </summary>
        /// <param name="TxtStr">文件内容</param>
        /// <param name="TxtFile">输出路径，物理路径</param>
        protected void SaveJsFile(string TxtStr, string TxtFile)
        {
            SaveJsFile(TxtStr, TxtFile, "2");
        }
        /// <summary>
        /// 保存js文件
        /// </summary>
        /// <param name="TxtStr">文件内容</param>
        /// <param name="TxtFile">输出路径，物理路径</param>
        /// <param name="Edcode">编码：1=gb2312,2=utf-8,3=unicode</param>
        protected void SaveJsFile(string TxtStr, string TxtFile, string Edcode)
        {
            System.Text.Encoding FileType = System.Text.Encoding.Default;
            switch (Edcode)
            {
                case "3":
                    FileType = System.Text.Encoding.Unicode;
                    break;
                case "2":
                    FileType = System.Text.Encoding.UTF8;
                    break;
                case "1":
                    FileType = System.Text.Encoding.GetEncoding("GB2312");
                    break;
            }
            SiteGroupCms.Utils.DirFile.CreateFolder(SiteGroupCms.Utils.DirFile.GetFolderPath(false, TxtFile));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(TxtFile, false, FileType);
            sw.Write("/*本文件由jcms于 " + System.DateTime.Now.ToString() + " 自动生成,请勿手动修改*/\r\n" + TxtStr);
            sw.Close();
        }
        #endregion
        #region 保存Css文件
        /// <summary>
        /// 保存Css文件
        /// </summary>
        /// <param name="TxtStr">文件内容</param>
        /// <param name="TxtFile">输出路径，物理路径</param>
        protected void SaveCssFile(string TxtStr, string TxtFile)
        {
            SaveCssFile(TxtStr, TxtFile, "2");
        }
        /// <summary>
        /// 保存Css文件
        /// </summary>
        /// <param name="TxtStr">文件内容</param>
        /// <param name="TxtFile">输出路径，物理路径</param>
        /// <param name="Edcode">编码：1=gb2312,2=utf-8,3=unicode</param>
        protected void SaveCssFile(string TxtStr, string TxtFile, string Edcode)
        {
            System.Text.Encoding FileType = System.Text.Encoding.Default;
            switch (Edcode)
            {
                case "3":
                    FileType = System.Text.Encoding.Unicode;
                    break;
                case "2":
                    FileType = System.Text.Encoding.UTF8;
                    break;
                case "1":
                    FileType = System.Text.Encoding.GetEncoding("GB2312");
                    break;
            }
            SiteGroupCms.Utils.DirFile.CreateFolder(SiteGroupCms.Utils.DirFile.GetFolderPath(false, TxtFile));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(TxtFile, false, FileType);
            sw.Write("/*本文件由jcms于 " + System.DateTime.Now.ToString() + " 自动生成,请勿手动修改*/\r\n" + TxtStr);
            sw.Close();
        }
        #endregion
        #region 处理Cache文件
        /// <summary>
        /// 读取Cache文件并保存到Html文件
        /// </summary>
        /// <param name="CacheStr">缓存内容</param>
        /// <param name="OutFile">输出路径，物理路径</param>
        protected void SaveCacheFile(string CacheStr, string OutFile)
        {
            SaveCacheFile(CacheStr, OutFile, "2");
        }
        /// <summary>
        /// 保存Cache文件
        /// </summary>
        /// <param name="CacheStr">缓存内容</param>
        /// <param name="OutFile">输出路径，物理路径</param>
        /// <param name="Edcode">编码：1=gb2312,2=utf-8,3=unicode</param>
        protected void SaveCacheFile(string CacheStr, string OutFile, string Edcode)
        {
            System.Text.Encoding FileType = System.Text.Encoding.Default;
            switch (Edcode)
            {
                case "3":
                    FileType = System.Text.Encoding.Unicode;
                    break;
                case "2":
                    FileType = System.Text.Encoding.UTF8;
                    break;
                case "1":
                    FileType = System.Text.Encoding.GetEncoding("GB2312");
                    break;
            }
            SiteGroupCms.Utils.DirFile.CreateFolder(SiteGroupCms.Utils.DirFile.GetFolderPath(false, OutFile));
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(OutFile, false, FileType);
                //下面这行测试所用，可以注释
                //CacheStr += "\r\n<!--Published " + System.DateTime.Now.ToString() + "-->";
                sw.Write(CacheStr);
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// 链接到站点首页
        /// </summary>
        public string Go2Site()
        {
            SiteGroupCms.Entity.Site site=(SiteGroupCms.Entity.Site) HttpContext.Current.Session["site"];
            return "<a href='/sites/"+site.Location + "/pub/'>首页</a>"; 
        }
        /// <summary>
        /// 链接到频道首页
        /// </summary>
        public string Go2Channel(string _channelid,int type)
        {
            return (new SiteGroupCms.Dal.Normal_ChannelDAL()).GetChannelLink(_channelid,type);
        }
        /// <summary>
        /// 链接到栏目页
        /// </summary>
        public string Go2Class(int _page, bool _ishtml, string _channelid, string _classid, bool _truefile)
        {
            return "";
          //  return (new SiteGroupCms.DAL.Normal_ClassDAL()).GetClassLink(_page, _ishtml, _channelid, _classid, _truefile);
        }
        /// <summary>
        /// 链接到内容页
        /// </summary>
        /// <param name="_contentid">内容ID</param>
        /// <param name="_initialize">是否初始化</param>
        /// <returns></returns>
        public string Go2View(string _contentid)
        {
            SiteGroupCms.Dal.ArticleDal articledal = new ArticleDal();
            SiteGroupCms.Entity.Article article = articledal.GetEntity(_contentid);
            SiteGroupCms.Entity.Normal_Channel _Channel = new SiteGroupCms.Dal.Normal_ChannelDAL().GetEntity(article.Catalogid.ToString());
            return ModuleCommand.GetContentLink( _contentid);
        }
       
    }
}
