using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;

namespace SiteGroupCms.Utils.LuceneHelp
{
    /// <summary>
    /// 搜索内容
    /// </summary>
    public class SearchItem
    {
        public string TableName
        {
            get;
            set;
        }
        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 频道ID
        /// </summary>
        public string ChannelId
        {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url
        {
            get;
            set;
        }
        /// <summary>
        /// 简介
        /// </summary>
        public string Summary
        {
            get;
            set;
        }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags
        {
            get;
            set;
        }
        /// <summary>
        /// 更新日期
        /// </summary>
        public string AddDate
        {
            get;
            set;
        }
    }
    public class SearchIndex
    {
        /// <summary>
        /// 分布检索
        /// </summary>
        /// <param name="type">多个用,隔开</param>
        /// <param name="ccid"></param>
        /// <param name="keywords">已经使用分词工具处理过</param>
        /// <param name="pageLen"></param>
        /// <param name="pageNo"></param>
        /// <param name="recCount"></param>
        /// <param name="eventTime"></param>
        /// <returns></returns>
        public static List<SearchItem> Search(string type, string ccid, String keywords, int pageLen, int pageNo, out int recCount, out double eventTime)
        {
            DateTime start = DateTime.Now;
            string[] types = type.Split(',');
            int _type_num = types.Length;
            IndexSearcher[] searchers = new IndexSearcher[_type_num];
            for (int i = 0; i < _type_num; i++)
            {
                searchers[i] = new IndexSearcher(HttpContext.Current.Server.MapPath("~/_data/index/" + types[i] + "/"));
            }
            MultiSearcher search = new MultiSearcher(searchers);

            BooleanQuery bq = new BooleanQuery();
            if (ccid != "0")
            {
                Term Term_channel = new Term("channelid", ccid);
                var termQuery = new TermQuery(Term_channel);
                bq.Add(termQuery, BooleanClause.Occur.MUST);
            }

            string[] fields = { "title", "tags", "summary" };
            MultiFieldQueryParser parser = new MultiFieldQueryParser(fields, new StandardAnalyzer());//要查询的字段
            Query query = parser.Parse(keywords);
            bq.Add(query, BooleanClause.Occur.MUST);//添加到条件
            Hits hits = search.Search(bq);
            TimeSpan duration = DateTime.Now - start;
            List<SearchItem> result = new List<SearchItem>();
            recCount = hits.Length();
            eventTime = duration.TotalMilliseconds;
            if (recCount > 0)
            {
                int i = (pageNo - 1) * pageLen;
                while (i < recCount && result.Count < pageLen)
                {
                    SearchItem news = null;
                    try
                    {
                        news = new SearchItem();
                        news.Id = hits.Doc(i).Get("id");
                        news.ChannelId = hits.Doc(i).Get("channelid");
                        news.TableName = hits.Doc(i).Get("tablename");
                        news.Title = hits.Doc(i).Get("title");
                        news.Summary = hits.Doc(i).Get("summary");
                        news.Tags = hits.Doc(i).Get("tags");
                        news.Url = hits.Doc(i).Get("url");
                        news.AddDate = hits.Doc(i).Get("adddate");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    finally
                    {
                        result.Add(news);
                        i++;
                    }
                }
                search.Close();
                return result;
            }
            else
                return null;
        }
    }
}
