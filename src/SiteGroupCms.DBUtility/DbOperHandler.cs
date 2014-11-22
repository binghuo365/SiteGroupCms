/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序作者: SiteGroupCms高伟 254850396#qq.com
 */

using System;
using System.Data;
namespace SiteGroupCms.DBUtility
{
    /// <summary>
    /// 表示数据库连接类型。
    /// </summary>
    public enum DatabaseType : byte { SqlServer, OleDb };
    /// <summary>
    /// DbOperHandler 的摘要说明。
    /// </summary>
    public abstract class DbOperHandler : IDisposable
    {
        /// <summary>
        /// 析构函数，释放申请的资源。
        /// </summary>
        ~DbOperHandler()
        {
            conn.Close();
        }
        /// <summary>
        /// 表示数据库连接的类型，目前支持SqlServer和OLEDB
        /// </summary>
        public DatabaseType dbType = DatabaseType.OleDb;
        /// <summary>
        /// 返回当前使用的数据库连接对象。
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return conn;
        }
        /// <summary>
        /// 条件表达式，用于在数据库操作时筛选记录，通常用于仅需指定表名称和某列名称的操作，如GetValue()，Delete()等，支持查询参数，由AddConditionParameters指定。。
        /// </summary>
        public string ConditionExpress = string.Empty;
        /// <summary>
        /// 当前的SQL语句。
        /// </summary>
        public string SqlCmd = string.Empty;
        /// <summary>
        /// 当前操作所涉及的数据表名称。
        /// </summary>
        protected string tableName = string.Empty;
        /// <summary>
        /// 当前操作所设计的字段名称。
        /// </summary>
        protected string fieldName = string.Empty;
        /// <summary>
        /// 当前所使用的数据库连接。
        /// </summary>
        protected IDbConnection conn;
        /// <summary>
        /// 当前所使用的命令对象。
        /// </summary>
        protected IDbCommand cmd;
        /// <summary>
        /// 当前所使用的数据库适配器。
        /// </summary>
        protected IDbDataAdapter da;
        /// <summary>
        /// 用于存储字段/值配对。
        /// </summary>
        protected System.Collections.ArrayList alFieldItems = new System.Collections.ArrayList(10);
        /// <summary>
        /// 用于存储SQL语句中的查询参数。
        /// </summary>
        protected System.Collections.ArrayList alSqlCmdParameters = new System.Collections.ArrayList(5);
        /// <summary>
        /// 用于存储条件表达式中的查询参数。
        /// </summary>
        protected System.Collections.ArrayList alConditionParameters = new System.Collections.ArrayList(5);
        /// <summary>
        /// 重置该对象，使之恢复到构造时的状态。
        /// </summary>
        public void Reset()
        {
            this.alFieldItems.Clear();
            this.alSqlCmdParameters.Clear();
            this.alConditionParameters.Clear();
            this.ConditionExpress = string.Empty;
            this.SqlCmd = string.Empty;
            this.cmd.Parameters.Clear();
            this.cmd.CommandText = string.Empty;
            this.cmd.CommandType = CommandType.Text;//默认非存储过程执行
        }
        /// <summary>
        /// 添加一个字段/值对到数组中。
        /// </summary>
        /// <param name="_fieldName">字段名称。</param>
        /// <param name="_fieldValue">字段值。</param>
        public void AddFieldItem(string _fieldName, object _fieldValue)
        {

            for (int i = 0; i < this.alFieldItems.Count; i++)
            {
                if (((DbKeyItem)this.alFieldItems[i]).fieldName == _fieldName)
                {
                    throw new ArgumentException(_fieldName + "不能重复赋值!");
                }
            }
            this.alFieldItems.Add(new DbKeyItem(_fieldName, _fieldValue));
        }
        /// <summary>
        /// 批量添加字段/值对到数组中。
        /// </summary>
        /// <param name="_vFields">用一个2行X列的二维数组表示。[0, X]为字段名名，[1, X]为字段的值</param>
        public void AddFieldItems(object[,] _vFields)
        {
            if ((!Object.Equals(_vFields, null)) && (_vFields.GetUpperBound(0) == 1) && (_vFields.Rank == 2))
                for (int i = 0; i <= _vFields.GetUpperBound(1); i++)
                    if (!Object.Equals(_vFields[0, i], null)) AddFieldItem(_vFields[0, i].ToString(), _vFields[1, i]);

        }
        /// <summary>
        /// 添加条件表达式中的查询参数到数组中。注意：当数据库连接为SqlServer时，参数名称必须和SQL语句匹配。其它则必须保持添加顺序和ConditionExpress中参数顺序一致，否则会失效。
        /// </summary>
        /// <param name="_conditionName">条件名称。</param>
        /// <param name="_conditionValue">条件值。</param>
        public void AddConditionParameter(string _conditionName, object _conditionValue)
        {
            for (int i = 0; i < this.alConditionParameters.Count; i++)
            {
                if (((DbKeyItem)this.alConditionParameters[i]).fieldName == _conditionName)
                {
                    throw new ArgumentException("条件参数名\"" + _conditionName + "\"不能重复赋值!");
                }
            }
            this.alConditionParameters.Add(new DbKeyItem(_conditionName, _conditionValue));
        }
        /// <summary>
        /// 批量添加条件表达式中的查询参数到数组中。
        /// </summary>
        /// <param name="_vParameters">用一个2行X列的二维数组表示。[0, X]为Parameter名，[1, X]为Parameter的值</param>
        public void AddConditionParameters(object[,] _vParameters)
        {
            if ((!Object.Equals(_vParameters, null)) && (_vParameters.GetUpperBound(0) == 1) && (_vParameters.Rank == 2))
                for (int i = 0; i <= _vParameters.GetUpperBound(1); i++)
                    if (!Object.Equals(_vParameters[0, i], null)) AddConditionParameter(_vParameters[0, i].ToString(), _vParameters[1, i]);

        }
        /// <summary>
        /// 返回满足条件的记录数
        /// </summary>
        /// <param name="tableName">要查询的数据表名</param>
        /// <returns>是/否</returns>
        public int Count(string tableName)
        {
            return Convert.ToInt32(this.GetField(tableName, "count(*)", false).ToString());
        }
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="tableName">要查询的数据表名</param>
        /// <returns>是/否</returns>
        public bool Exist(string tableName)
        {
            return this.GetField(tableName, "count(*)", false).ToString() != "0";
        }
        /// <summary>
        /// 获得最大id
        /// </summary>
        /// <param name="tableName">要查询的数据表名</param>
        public int MaxId(string tableName)
        {
            return Convert.ToInt32("0" + this.GetField(tableName, "max(id)", false).ToString());
        }
        /// <summary>
        /// 获得最小值
        /// </summary>
        /// <param name="tableName">要查询的数据表名</param>
        public int MinValue(string tableName, string fieldName)
        {
            return Convert.ToInt32("0" + this.GetField(tableName, "min(" + fieldName + ")", false).ToString());
        }
        /// <summary>
        /// 获得最大值
        /// </summary>
        /// <param name="tableName">要查询的数据表名</param>
        public int MaxValue(string tableName, string fieldName)
        {
            return Convert.ToInt32("0" + this.GetField(tableName, "max(" + fieldName + ")", false).ToString());
        }
        /// <summary>
        /// 抽象函数。用于产生Command对象所需的参数。
        /// </summary>
        protected abstract void GenParameters();
        /// <summary>
        /// 根据当前alFieldItem数组中存储的字段/值向指定表中添加一条数据。在该表无触发器的情况下返回添加数据所获得的自动增长id值。
        /// </summary>
        /// <param name="_tableName">要插入数据的表名称。</param>
        /// <returns>返回本数据连接上产生的最后一个自动增长id值。</returns>
        public int Insert(string _tableName)
        {

            this.tableName = _tableName;
            this.fieldName = string.Empty;
            this.SqlCmd = "insert into [" + this.tableName + "](";
            string tempValues = " values(";
            for (int i = 0; i < this.alFieldItems.Count - 1; i++)
            {
                this.SqlCmd += "[" + ((DbKeyItem)alFieldItems[i]).fieldName + "]";
                this.SqlCmd += ",";

                tempValues += "@para";
                tempValues += i.ToString();

                tempValues += ",";
            }
            this.SqlCmd += "[" + ((DbKeyItem)alFieldItems[alFieldItems.Count - 1]).fieldName + "]";
            this.SqlCmd += ") ";

            tempValues += "@para";
            tempValues += (alFieldItems.Count - 1).ToString();

            tempValues += ")";
            this.SqlCmd += tempValues;
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            cmd.ExecuteNonQuery();
            int autoId = 0;
            try
            {
                this.cmd.CommandText = "select @@identity as id";
                autoId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception) { }
            return autoId;
        }

        /// <summary>
        /// 根据当前alFieldItem数组中存储的字段/值和条件表达式所指定的条件来更新数据库中的记录，返回所影响的行数。
        /// </summary>
        /// <param name="_tableName">要更新的数据表名称。</param>
        /// <returns>返回此次操作所影响的数据行数。</returns>
        public int Update(string _tableName)
        {
            this.tableName = _tableName;
            this.fieldName = string.Empty;
            this.SqlCmd = "UPDATE [" + this.tableName + "] SET ";
            for (int i = 0; i < this.alFieldItems.Count - 1; i++)
            {
                this.SqlCmd += "[" + ((DbKeyItem)alFieldItems[i]).fieldName + "]";
                this.SqlCmd += "=";

                this.SqlCmd += "@para";
                this.SqlCmd += i.ToString();

                this.SqlCmd += ",";
            }
            this.SqlCmd += "[" + ((DbKeyItem)alFieldItems[alFieldItems.Count - 1]).fieldName + "]";
            this.SqlCmd += "=";

            this.SqlCmd += "@para";
            this.SqlCmd += (alFieldItems.Count - 1).ToString();
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd += " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            int effectedLines = this.cmd.ExecuteNonQuery();
            return effectedLines;
        }

        /// <summary>
        /// 执行SqlCmd中的SQL语句，参数由AddSqlCmdParameters指定，与ConditionExpress无关。
        /// </summary>
        /// <returns>返回此次操作所影响的数据行数。</returns>
        public int ExecuteSqlNonQuery()
        {
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return this.cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="_tableName"></param>
        /// <returns></returns>
        public bool DropTable(string _tableName)
        {
            try
            {
                this.cmd.CommandText = "drop table [" + _tableName + "]";
                this.cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 判断表是否存在
        /// </summary>
        /// <param name="_tableName"></param>
        /// <returns></returns>
        public bool ExistTable(string _tableName)
        {
            try
            {
                this.cmd.CommandText = "select top 1 * from [" + _tableName + "]";
                this.cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取指定表，指定列，指定条件的第一个符合条件的值。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <param name="_isField">是否纯字段名？</param>
        /// <returns>获取的值。如果为空则返回null。</returns>
        /// 
        public object GetField(string _tableName, string _fieldName, bool _isField)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            if (_isField)
                this.SqlCmd = "select [" + this.fieldName + "] from [" + this.tableName + "]";
            else
                this.SqlCmd = "select " + this.fieldName + " from [" + this.tableName + "]";
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            object ret = cmd.ExecuteScalar();
            if (ret == null) ret = (object)string.Empty;
            return ret;
        }
        public object GetField(string _tableName, string _fieldName)
        {
            return GetField(_tableName, _fieldName, true);
        }
        /// <summary>
        /// 获取指定表，指定列，指定条件的第一行中符合条件的值的集合。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldNames">字段名称,以逗号隔开。</param>
        /// <returns>获取的值。如果为空则返回null。</returns>
        public object[] GetFields(string _tableName, string _fieldNames)
        {
            this.SqlCmd = "select " + _fieldNames + " from " + _tableName;
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            System.Data.DataSet ds = new System.Data.DataSet();
            this.da.SelectCommand = this.cmd;
            this.da.Fill(ds);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                object[] _obj = new object[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                    _obj[i] = dt.Rows[0][i];
                return _obj;
            }
            return null;
        }
        /// <summary>
        /// 获取指定表，指定列，指定条件的记录数。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>获取的记录数</returns>
        public int GetCount(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            this.SqlCmd = "select count(" + this.fieldName + ") from [" + this.tableName + "]";
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return (int)cmd.ExecuteScalar();
        }
        /// <summary>
        /// 根据当前指定的SqlCmd获取DataTable。如果ConditionExpress不为空则会将其清空，所以条件表达式需要包含在SqlCmd中。
        /// </summary>
        /// <returns>返回查询结果DataTable。</returns>
        public DataTable GetDataTable()
        {
            DataSet ds = this.GetDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// 根据当前指定的SqlCmd获取DataSet。如果ConditionExpress不为空则会将其清空，所以条件表达式需要包含在SqlCmd中。
        /// </summary>
        /// <returns>返回查询结果DataSet。</returns>
        public DataSet GetDataSet()
        {
            this.alConditionParameters.Clear();
            this.ConditionExpress = string.Empty;
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            DataSet ds = new DataSet();
            this.da.SelectCommand = this.cmd;
            this.da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 对指定表，指定字段执行加一计数，返回计数后的值。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>返回计数后的值。</returns>
        public int Add(string _tableName, string _fieldName)
        {
            return Add(_tableName, _fieldName, 1);
        }
        /// <summary>
        /// 对指定表，指定字段执行增加计数，返回计数后的值。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <param name="_num">减少的值。</param>
        /// <returns>返回计数后的值。</returns>
        public int Add(string _tableName, string _fieldName, int _num)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            int count = Convert.ToInt32("0" + this.GetField(this.tableName, this.fieldName));
            count = count + _num;
            this.cmd.Parameters.Clear();
            this.cmd.CommandText = string.Empty;
            this.AddFieldItem(_fieldName, count);
            this.Update(this.tableName);
            return count;
        }

        /// <summary>
        /// 对指定表，指定字段执行减一计数，返回计数后的值。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>返回计数后的值。</returns>
        public int Deduct(string _tableName, string _fieldName)
        {
            return Deduct(_tableName, _fieldName, 1);
        }
        /// <summary>
        /// 对指定表，指定字段执行减少计数，返回计数后的值。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <param name="_num">减少的值。</param>
        /// <returns>返回计数后的值。</returns>
        public int Deduct(string _tableName, string _fieldName, int _num)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            int count = Convert.ToInt32("0" + this.GetField(this.tableName, this.fieldName));
            if (count > 0)
            {
                count = count - _num;
                if (count < 0) count = 0;
            }
            this.cmd.Parameters.Clear();
            this.cmd.CommandText = string.Empty;
            this.AddFieldItem(_fieldName, count);
            this.Update(this.tableName);
            return count;
        }

        /// <summary>
        /// 对指定表，返回字段值的总和。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>返回字段值的总和。</returns>
        public int Sum(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            int sum = Convert.ToInt32("0" + this.GetField(this.tableName, "sum(" + this.fieldName + ")", false));
            return sum;
        }
        /// <summary>
        /// 根据ConditionExpress指定的条件在指定表中删除记录。返回删除的记录数。
        /// </summary>
        /// <param name="_tableName">指定的表名称。</param>
        /// <returns>返回删除的记录数。</returns>
        public int Delete(string _tableName)
        {
            this.tableName = _tableName;
            this.SqlCmd = "delete from [" + this.tableName + "]";
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 审核函数。将指定表，指定字段的值进行翻转，如：1->0或0->1。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>返回影响的行数。</returns>
        public int Audit(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            this.SqlCmd = "UPDATE [" + this.tableName + "] SET [" + this.fieldName + "]=1-" + this.fieldName;
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return cmd.ExecuteNonQuery();
        }
        public DataRow GetSP_Row(string ProcedureName)
        {
            this.cmd.CommandText = ProcedureName;
            this.cmd.CommandType = CommandType.StoredProcedure;//指定是存储过程
            this.GenParameters();
            System.Data.SqlClient.SqlDataAdapter ada = new System.Data.SqlClient.SqlDataAdapter();
            ada.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            DataTable dt = new DataTable();
            ada.Fill(dt);
            ada.Dispose();
            cmd.Parameters.Clear();//清楚参数
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        public DataRowCollection GetSP_Rows(string ProcedureName)
        {
            this.cmd.CommandText = ProcedureName;
            this.cmd.CommandType = CommandType.StoredProcedure;//指定是存储过程
            this.GenParameters();
            System.Data.SqlClient.SqlDataAdapter ada = new System.Data.SqlClient.SqlDataAdapter();
            ada.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            DataTable dt = new DataTable();
            ada.Fill(dt);
            ada.Dispose();
            cmd.Parameters.Clear();//清楚参数
            return dt.Rows;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            conn.Close();
            conn.Dispose();
        }

    }

    /// <summary>
    /// 数据表中的字段属性，包括字段名，字段值。
    /// 常用于保存要提交的数据。
    /// </summary>
    public class DbKeyItem
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_fieldName">字段名称。</param>
        /// <param name="_fieldValue">字段值。</param>
        public DbKeyItem(string _fieldName, object _fieldValue)
        {
            this.fieldName = _fieldName;
            this.fieldValue = _fieldValue.ToString();
        }
        /// <summary>
        /// 字段名称。
        /// </summary>
        public string fieldName;
        /// <summary>
        /// 字段值。
        /// </summary>
        public string fieldValue;
    }
}
