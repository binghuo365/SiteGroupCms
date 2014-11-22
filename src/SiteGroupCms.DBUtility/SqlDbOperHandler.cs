/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序作者: SiteGroupCms高伟 254850396#qq.com 
 */

using System;
namespace SiteGroupCms.DBUtility
{
    /// <summary>
    /// 本对象用与提供对SqlServer数据库的常用访问操作。
    /// </summary>
    public class SqlDbOperHandler : DbOperHandler
    {
        /// <summary>
        /// 构造函数，接收一个SqlServer数据库连接对象SqlConnection
        /// </summary>
        public SqlDbOperHandler(System.Data.SqlClient.SqlConnection _conn)
        {
            conn = _conn;
            dbType = DatabaseType.SqlServer;

            conn.Open();
            cmd = conn.CreateCommand();
            da = new System.Data.SqlClient.SqlDataAdapter();

        }
        /// <summary>
        /// 产生SqlCommand对象所需的查询参数。
        /// </summary>
        protected override void GenParameters()
        {
            System.Data.SqlClient.SqlCommand sqlCmd = (System.Data.SqlClient.SqlCommand)cmd;
            if (this.alFieldItems.Count > 0)
            {
                for (int i = 0; i < alFieldItems.Count; i++)
                {
                    sqlCmd.Parameters.AddWithValue("@para" + i.ToString(), ((DbKeyItem)alFieldItems[i]).fieldValue.ToString());
                }
            }

            if (this.alSqlCmdParameters.Count > 0)
            {
                for (int i = 0; i < this.alSqlCmdParameters.Count; i++)
                {
                    sqlCmd.Parameters.AddWithValue(((DbKeyItem)alSqlCmdParameters[i]).fieldName.ToString(), ((DbKeyItem)alSqlCmdParameters[i]).fieldValue.ToString());
                }
            }
            if (this.alConditionParameters.Count > 0)
            {
                for (int i = 0; i < this.alConditionParameters.Count; i++)
                {
                    sqlCmd.Parameters.AddWithValue(((DbKeyItem)alConditionParameters[i]).fieldName.ToString(), ((DbKeyItem)alConditionParameters[i]).fieldValue.ToString());
                }
            }
        }

    }
}