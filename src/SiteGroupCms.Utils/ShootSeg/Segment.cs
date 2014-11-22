using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace SiteGroupCms.Utils.ShootSeg
{
    /// <summary>
    /// 改写自ShootSearch 中文分词组件
    /// </summary>
    public class Segment
    {
        private string m_DicPath = System.Web.HttpContext.Current.Server.MapPath("~/_data/ShootSeg/sDict.dic");
        private string m_NoisePath = System.Web.HttpContext.Current.Server.MapPath("~/_data/ShootSeg/sNoise.dic");
        private string m_NumberPath = System.Web.HttpContext.Current.Server.MapPath("~/_data/ShootSeg/sNumber.dic");
        private string m_WordPath = System.Web.HttpContext.Current.Server.MapPath("~/_data/ShootSeg/sWord.dic");
        private string m_PrefixPath = System.Web.HttpContext.Current.Server.MapPath("~/_data/ShootSeg/sPrefix.dic");
        private double m_EventTime = 0;

        private Hashtable htWords;
        private ArrayList alNoise;
        private ArrayList alNumber;
        private ArrayList alWord;
        private ArrayList alPrefix;

        /// <summary>
        /// 分隔符
        /// </summary>
        private string m_Separator = " ";

        /// <summary>
        /// 用于验证汉字的正则表达式
        /// </summary>
        private string strChinese = "[\u4e00-\u9fa5]";
        /// <summary>
        /// 基本词典路径
        /// </summary>
        public string DicPath
        {
            get
            {
                return m_DicPath;
            }
            set
            {
                m_DicPath = value;
            }
        }
        /// <summary>
        /// 数据缓存函数
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="val">缓存的数据</param>
        private static void SetCache(string key, object val)
        {
            if (val == null)
                val = " ";
            System.Web.HttpContext.Current.Application.Lock();
            System.Web.HttpContext.Current.Application.Set(key, val);
            System.Web.HttpContext.Current.Application.UnLock();
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="mykey"></param>
        /// <returns></returns>
        private static object GetCache(string key)
        {
            return System.Web.HttpContext.Current.Application.Get(key);
        }
        /// <summary>
        /// 暂时无用
        /// </summary>
        public string NoisePath
        {
            get
            {
                return m_NoisePath;
            }
            set
            {
                m_NoisePath = value;
            }
        }

        /// <summary>
        /// 数字词典路径
        /// </summary>
        public string NumberPath
        {
            get
            {
                return m_NumberPath;
            }
            set
            {
                m_NumberPath = value;
            }
        }

        /// <summary>
        /// 字母词典路径
        /// </summary>
        public string WordPath
        {
            get
            {
                return m_WordPath;
            }
            set
            {
                m_WordPath = value;
            }
        }

        /// <summary>
        /// 姓名前缀字典 用于纠错姓名
        /// </summary>
        public string PrefixPath
        {
            get
            {
                return m_PrefixPath;
            }
            set
            {
                m_PrefixPath = value;
            }
        }


        /// <summary>
        /// 是否开启姓名纠错功能
        /// </summary>
        public bool EnablePrefix
        {
            get
            {
                if (alPrefix.Count == 0)
                    return false;
                else
                    return true;
            }
            set
            {
                if (value)
                    alPrefix = LoadWords(PrefixPath, alPrefix);
                else
                    alPrefix = new ArrayList();
            }
        }

        /// <summary>
        /// 操作用时
        /// 每次进行加载或分词动作后改属性表示为上一次动作所用时间
        /// 已精确到毫秒但分词操作在字符串教短时可能为0
        /// 改属性只读
        /// </summary>
        public double EventTime
        {
            get
            {
                return m_EventTime;
            }
        }

        /// <summary>
        /// 分隔符,默认为空格
        /// </summary>
        public string Separator
        {
            get
            {
                return m_Separator;
            }
            set
            {
                if (value != "" && value != null)
                    m_Separator = value;
            }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public Segment(string p_DicPath, string p_NoisePath, string p_NumberPath, string p_WordPath)
        {
            m_WordPath = p_DicPath;
            m_WordPath = p_NoisePath;
            m_WordPath = p_NumberPath;
            m_WordPath = p_WordPath;
            this.InitWordDics();
        }

        /// <summary>
        /// 重载构造方法,不进行任何操作
        /// </summary>
        public Segment()
        {
        }
        /// <summary>
        /// 加载词列表
        /// </summary>
        public void InitWordDics()
        {

            DateTime start = DateTime.Now;
            if (GetCache("jcms_dict") == null)
            {
                htWords = new Hashtable();
                string strChar1;
                string strChar2;

                StreamReader reader = new StreamReader(DicPath, System.Text.Encoding.UTF8);
                string strline = reader.ReadLine();

                Hashtable father = htWords;

                Hashtable child = new Hashtable();
                Hashtable forfather = htWords;
                SegList list;
                long i = 0;
                while (strline != null && strline.Trim() != "")
                {
                    i++;

                    strChar1 = strline.Substring(0, 1);
                    strChar2 = strline.Substring(1, 1);
                    if (!htWords.ContainsKey(strChar1))
                    {
                        father = new Hashtable();
                        htWords.Add(strChar1, father);
                    }
                    else
                    {
                        father = (Hashtable)htWords[strChar1];
                    }

                    if (!father.ContainsKey(strChar2))
                    {
                        list = new SegList();
                        if (strline.Length > 2)
                            list.Add(strline.Substring(2));
                        else
                            list.Add("null");
                        father.Add(strChar2, list);
                    }
                    else
                    {
                        list = (SegList)father[strChar2];
                        if (strline.Length > 2)
                        {
                            list.Add(strline.Substring(2));
                        }
                        else
                        {
                            list.Add("null");
                        }
                        father[strChar2] = list;


                    }
                    htWords[strChar1] = father;




                    strline = reader.ReadLine();
                }
                try
                {
                    reader.Close();
                }
                catch
                { }
                SetCache("jcms_dict", htWords);
            }
            htWords = (Hashtable)GetCache("jcms_dict");

            alNoise = LoadWords(NoisePath, alNoise);
            alNumber = LoadWords(NumberPath, alNumber);
            alWord = LoadWords(WordPath, alWord);
            alPrefix = LoadWords(PrefixPath, alPrefix);

            TimeSpan duration = DateTime.Now - start;

            m_EventTime = duration.TotalMilliseconds;

        }
        /// <summary>
        /// 加载文本词组到ArrayList
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public ArrayList LoadWords(string strPath, ArrayList list)
        {
            StreamReader reader = new StreamReader(strPath, System.Text.Encoding.UTF8);
            list = new ArrayList();
            string strline = reader.ReadLine();
            while (strline != null)
            {
                list.Add(strline);
                strline = reader.ReadLine();
            }
            try
            {
                reader.Close();
            }
            catch
            { }
            return list;
        }
        /// <summary>
        /// 输出词列表
        /// </summary>
        public void OutWords()
        {
            IDictionaryEnumerator idEnumerator1 = htWords.GetEnumerator();
            while (idEnumerator1.MoveNext())
            {
                IDictionaryEnumerator idEnumerator2 = ((Hashtable)idEnumerator1.Value).GetEnumerator();
                while (idEnumerator2.MoveNext())
                {
                    SegList aa = (SegList)idEnumerator2.Value;
                    for (int i = 0; i < aa.Count; i++)
                    {
                        Console.WriteLine(idEnumerator1.Key.ToString()
                            + idEnumerator2.Key.ToString() + aa.GetElem(i).ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 输出ArrayList
        /// </summary>
        public void OutArrayList(ArrayList list)
        {
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].ToString());
            }
        }
        /// <summary>
        /// 分词过程 , 不支持回车 
        /// </summary>
        /// <param name="strText">要分词的文本</param>
        /// <returns>分词后的文本</returns>
        public string SegmentText(string strText)
        {

            string reText = "";
            strText = (strText + "$").Trim();//随风缘添加，解决最后为字母时丢失的BUG
            int length = 0;
            //Console.WriteLine(strText.Length.ToString());

            if (htWords == null) return strText;
            if (strText.Length < 3) return strText;

            DateTime start = DateTime.Now;
            bool word = false;
            bool number = false;
            //bool han = false ;
            string strLastWords = Separator;
            string strPrefix = "";
            int preFix = 0;
            string strLastChar = "";
            //遍历每一个字
            for (int i = 0; i < strText.Length - 1; i++)
            {
                #region 对于每一个字的处理过程

                string strChar1 = strText.Substring(i, 1);
                string strChar2 = strText.Substring(i + 1, 1).Trim();
                Hashtable h;
                SegList l;
                bool yes;



                if (reText.Length > 0) strLastChar = reText.Substring(reText.Length - 1);
                //首先处理掉空格
                if (strChar1 == " ")
                {
                    if ((number || word) && strLastChar != Separator)
                        reText += this.Separator;
                    yes = true;
                }
                else
                    yes = false;

                #region 开始判断字符类型

                int CharType = GetCharType(strChar1);
                switch (CharType)
                {
                    case 1:
                        #region 如果数字
                        //如果数字的上一位是字母要和后面的数字分开
                        if (word)
                        {
                            reText += Separator;
                        }

                        strLastWords = "";
                        word = false;
                        number = true;
                        break;
                        #endregion
                    case 2:
                    case 5:
                        #region 如果是字母
                        if (number)
                        {
                            strLastWords = Separator;
                        }
                        else
                            strLastWords = "";

                        word = true;
                        number = false;
                        break;
                        #endregion
                    case 3:
                    case 4:
                        #region 如果是汉字
                        // 第一级哈希表是否包含关键字
                        // 假如包含处理第二级哈希表
                        #region 处理第二级哈希表

                        //首先看上一个字是不是字母
                        if (word)
                            reText += Separator;

                        #region 检测上一个是否是数字 test code
                        //检测上一个是否是数字
                        //这个过程是用于修正数字后的量词的
                        if (number && CharType != 4)
                        {
                            h = (Hashtable)htWords["n"];
                            if (h.ContainsKey(strChar1))
                            {
                                l = (SegList)h[strChar1];
                                if (l.Contains(strChar2))
                                {
                                    reText += strChar1 + strChar2 + Separator;
                                    yes = true;
                                    i++;

                                }
                                else if (l.Contains("null"))
                                {
                                    reText += strChar1 + Separator;

                                    yes = true;
                                }
                            }
                            else
                                reText += Separator;
                        }

                        #endregion

                        //非汉字数字的汉字
                        if (CharType == 3)
                        {
                            //当前为汉字
                            word = false;
                            number = false;

                            //汉字词分隔符为" "
                            strLastWords = Separator;
                        }
                        else
                        {
                            //汉字数字
                            strLastWords = "";
                            word = false;
                            number = true;
                        }
                        // 第二级哈希表取出
                        h = (Hashtable)htWords[strChar1];
                        //第二级哈希表是否包含关键字
                        if (h.ContainsKey(strChar2))
                        {


                            #region  第二级包含关键字

                            //取出ArrayList对象
                            l = (SegList)h[strChar2];

                            //遍历每一个对象 看是否能组合成词
                            for (int j = 0; j < l.Count; j++)
                            {
                                bool have = false;
                                string strChar3 = l.GetElem(j).ToString();

                                //对于每一个取出的词进行检测,看是否匹配
                                //长度保护
                                if ((strChar3.Length + i + 2) < strText.Length)
                                {
                                    //向i+2后取出m长度的字
                                    string strChar = strText.Substring(i + 2, strChar3.Length).Trim();
                                    if (strChar3 == strChar && !yes)
                                    {
                                        if (strPrefix != "")
                                        {
                                            reText += strPrefix + Separator;
                                            strPrefix = "";
                                            preFix = 0;
                                        }
                                        reText += strChar1 + strChar2 + strChar;

                                        i += strChar3.Length + 1;
                                        have = true;
                                        yes = true;
                                        break;
                                    }
                                }
                                else if ((strChar3.Length + i + 2) == strText.Length)
                                {
                                    string strChar = strText.Substring(i + 2).Trim();
                                    if (strChar3 == strChar && !yes)
                                    {
                                        if (strPrefix != "")
                                        {
                                            reText += strPrefix + Separator;
                                            strPrefix = "";
                                            preFix = 0;
                                        }
                                        reText += strChar1 + strChar2 + strChar;


                                        i += strChar3.Length + 1;
                                        have = true;
                                        yes = true;
                                        break;
                                    }
                                }

                                //}//end for m
                                if (!have && j == l.Count - 1 && l.Contains("null") && !yes)
                                {

                                    #region
                                    if (preFix == 1)
                                    {
                                        reText += strPrefix + strChar1 + strChar2;
                                        strPrefix = "";
                                        preFix = 0;
                                    }
                                    else if (preFix > 1)
                                    {
                                        reText += strPrefix + strLastWords + strChar1 + strChar2;
                                        strPrefix = "";
                                        preFix = 0;
                                    }
                                    else
                                    {
                                        if (CharType == 4) reText += strChar1 + strChar2;
                                        else reText += strChar1 + strChar2;
                                        strLastWords = this.Separator;
                                        number = false;
                                    }
                                    #endregion


                                    i++;
                                    yes = true;
                                    break;
                                }
                                else if (have)
                                {
                                    break;
                                }

                            }//end for j
                            #endregion

                            //如果没有匹配还可能有一种情况
                            //这个词语只有两个字
                            //以这两个字开头的词语不存在
                            if (!yes && l.Contains("null"))
                            {
                                #region
                                if (preFix == 1)
                                {
                                    reText += strPrefix + strChar1 + strChar2;
                                    strPrefix = "";
                                    preFix = 0;
                                }
                                else if (preFix > 1)
                                {
                                    reText += strPrefix + strLastWords + strChar1 + strChar2;
                                    strPrefix = "";
                                    preFix = 0;
                                }
                                else
                                {
                                    if (CharType == 4) reText += strChar1 + strChar2;
                                    else reText += strChar1 + strChar2;
                                    strLastWords = this.Separator;
                                    number = false;
                                }
                                #endregion
                                i++;
                                yes = true;
                            }

                            if (reText.Length > 0) strLastChar = reText.Substring(reText.Length - 1);
                            if (CharType == 4 && GetCharType(strLastChar) == 4)
                            {
                                number = true;
                            }
                            else if (strLastChar != this.Separator)
                                reText += this.Separator;

                        }//end if h

                        #endregion
                        break;
                        #endregion
                    default:
                        #region 未知字符,可能是生僻字,也可能是标点符合之类

                        if (word && !yes)
                        {
                            reText += Separator;
                        }
                        else if (number && !yes)
                        {
                            reText += Separator;
                        }
                        number = false;
                        word = false;
                        strLastWords = this.Separator;
                        break;

                        #endregion
                }

                #endregion

                #endregion

                if (!yes && number || !yes && word)
                {
                    reText += strChar1;
                    yes = true;
                }

                if (!yes)
                {
                    #region 处理姓名问题 test code
                    if (preFix == 0)
                    {
                        if (alPrefix.Contains(strChar1 + strChar2))
                        {
                            i++;
                            strPrefix = strChar1 + strChar2;
                            preFix++;

                        }
                        else if (alPrefix.Contains(strChar1))
                        {
                            if (!number)
                            {
                                strPrefix = strChar1;
                                preFix++;
                            }
                            else
                            {
                                reText += strChar1 + strLastWords;
                                number = false;
                                word = false;
                            }
                        }
                        else
                        {
                            #region
                            if (preFix == 3)
                            {
                                reText += strPrefix + Separator + strChar1 + Separator;
                                strPrefix = "";
                                preFix = 0;

                            }
                            else if (preFix > 0)
                            {
                                if (Regex.IsMatch(strChar1, strChinese))
                                {
                                    strPrefix += strChar1;
                                    preFix++;
                                }
                                else
                                {
                                    reText += strPrefix + Separator + strChar1 + Separator;
                                    strPrefix = "";
                                    preFix = 0;
                                }

                            }
                            else
                            {
                                reText += strChar1 + strLastWords;
                                number = false;
                                word = false;
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region
                        if (preFix == 3)
                        {
                            reText += strPrefix + Separator + strChar1 + Separator;
                            strPrefix = "";
                            preFix = 0;

                        }
                        else if (preFix > 0)
                        {
                            if (Regex.IsMatch(strChar1, strChinese))
                            {
                                strPrefix += strChar1;
                                preFix++;
                            }
                            else
                            {
                                reText += strPrefix + Separator + strChar1 + Separator;
                                strPrefix = "";
                                preFix = 0;
                            }

                        }
                        else
                        {
                            reText += strChar1 + strLastWords;
                            number = false;
                        }
                        #endregion
                    }
                    #endregion
                }

                length = i;
            }
            #region 最后防止最后一个字的丢失
            //最后防止最后一个字的丢失
            if (length < strText.Length - 1)
            {
                string strLastChar1 = strText.Substring(strText.Length - 1).Trim();
                string strLastChar2 = strText.Substring(strText.Length - 2).Trim();

                if (reText.Length > 0) strLastChar = reText.Substring(reText.Length - 1);
                if (preFix != 0)
                {
                    reText += strPrefix + strLastChar1;
                }
                else
                {
                    switch (GetCharType(strLastChar1))
                    {
                        case 1:
                            if (strLastChar1 != "." && strLastChar1 != "．")
                                reText += strLastChar1;
                            else
                                reText += Separator + strLastChar1;
                            break;
                        case 2:
                        case 5:
                            if (alWord.Contains(strLastChar2))
                                reText += strLastChar1;
                            break;
                        case 3:
                        case 4:
                            if ((number || word) && strLastChar != Separator)
                                reText += Separator + strLastChar1;
                            else
                                reText += strLastChar1;
                            break;
                        default:
                            if (strLastChar != Separator)
                                reText += Separator + strLastChar1;
                            else
                                reText += strLastChar1;
                            break;

                    }
                }
                if (reText.Length > 0) strLastChar = (reText.Substring(reText.Length - 1));
                if (strLastChar != this.Separator) reText += this.Separator;

            }
            #endregion

            //计算时间
            TimeSpan duration = DateTime.Now - start;
            m_EventTime = duration.TotalMilliseconds;

            return reText.Replace(" $", "");//这里包含一个字的，我们去之
            //string[] reTexts = reText.Split(new string[] { this.Separator }, StringSplitOptions.RemoveEmptyEntries);
            //string reText2 = "";
            //for (int i = 0; i < reTexts.Length; i++)
            //{
            //    if (reTexts[i].Length > 1) reText2 += reTexts[i] + this.Separator;
            //}
            //return reText2;
        }

        /// <summary>
        /// 重载分词过程 支持回车
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="Enter"></param>
        /// <returns></returns>
        public string SegmentText(string strText, bool Enter)
        {
            if (Enter)
            {
                DateTime start = DateTime.Now;

                string[] strArr = strText.Split('\n');
                string reText = "";

                for (int i = 0; i < strArr.Length; i++)
                {
                    reText += SegmentText(strArr[i]) + "\r\n";
                }

                TimeSpan duration = DateTime.Now - start;
                m_EventTime = duration.TotalMilliseconds;

                return reText;
            }
            else
            {
                return SegmentText(strText);
            }
        }

        #region 辅助函数,判断字符类型
        /// <summary>
        /// 辅助函数,判断字符类型,0为未知,1为数字,2为字母,3为汉字,4为汉字数字
        /// </summary>
        /// <param name="p_Char"></param>
        /// <returns></returns>
        private int GetCharType(string p_Char)
        {
            int CharType = 0;
            //汉字数字＆阿拉伯字母
            if (alNumber.Contains(p_Char))
                CharType = 1;
            //字母
            if (alWord.Contains(p_Char))
                CharType = 2;
            //汉字
            if (htWords.ContainsKey(p_Char))
                CharType += 3;

            return CharType;


        }

        #endregion

        #region 对加载的词典排序并重新写入
        /// <summary>
        /// 对加载的词典排序并重新写入
        /// </summary>
        /// <param name="Reload">是否重新加载</param>
        public void SortDic(bool Reload)
        {
            DateTime start = DateTime.Now;
            StreamWriter sw = new StreamWriter(DicPath, false, System.Text.Encoding.UTF8);

            IDictionaryEnumerator idEnumerator1 = htWords.GetEnumerator();
            while (idEnumerator1.MoveNext())
            {
                IDictionaryEnumerator idEnumerator2 = ((Hashtable)idEnumerator1.Value).GetEnumerator();
                while (idEnumerator2.MoveNext())
                {
                    SegList aa = (SegList)idEnumerator2.Value;
                    aa.Sort();
                    for (int i = 0; i < aa.Count; i++)
                    {
                        if (aa.GetElem(i).ToString() == "null")
                            sw.WriteLine(idEnumerator1.Key.ToString() + idEnumerator2.Key.ToString());
                        else
                            sw.WriteLine(idEnumerator1.Key.ToString()
                                + idEnumerator2.Key.ToString() + aa.GetElem(i).ToString());
                    }
                }
            }
            sw.Close();

            //重新加载
            if (Reload)
                InitWordDics();

            TimeSpan duration = DateTime.Now - start;
            m_EventTime = duration.TotalMilliseconds;
        }

        /// <summary>
        /// 重载字典写入,默认Reload=false
        /// </summary>
        public void SortDic()
        {
            SortDic(false);
        }
        #endregion
        /// <summary>
        /// 目前功能是删除两行完全相同的词 暂时无用!
        /// </summary>
        /// <returns>相同词条个数</returns>
        public int Optimize()
        {
            int l = 0;
            DateTime start = DateTime.Now;

            Hashtable htOptimize = new Hashtable();
            StreamReader reader = new StreamReader(DicPath, System.Text.Encoding.UTF8);
            string strline = reader.ReadLine();
            while (strline != null && strline.Trim() != "")
            {
                if (!htOptimize.ContainsKey(strline))
                    htOptimize.Add(strline, null);
                else
                    l++;
            }
            Console.WriteLine("ready");
            try
            {
                reader.Close();
            }
            catch { }
            StreamWriter sw = new StreamWriter(DicPath, false, System.Text.Encoding.UTF8);

            IDictionaryEnumerator ide = htOptimize.GetEnumerator();
            while (ide.MoveNext())
                sw.WriteLine(ide.Key.ToString());
            try
            {
                sw.Close();
            }
            catch { }


            TimeSpan duration = DateTime.Now - start;
            m_EventTime = duration.TotalMilliseconds;

            return l;
        }

    }
}
