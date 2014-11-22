<%@ Page Language="C#" ContentType="text/html" ResponseEncoding="utf-8" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script runat="server">
    public void Page_Load(object sender, EventArgs e)
    {
        Response.Write("Hello world.");

        /***连接Access数据库
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;data source=" + Server.MapPath("db.mdb"));
        string cmdText = "SELECT * FROM TableName";
        OleDbDataAdapter adp = new OleDbDataAdapter(cmdText, conn);
        DataSet ds = new DataSet();
        adp.Fill(ds);

        this.gridView1.DataSource = ds.Tables[0];
        this.gridView1.DataBind();
        */

        /***连接MS Sql Server数据库
        SqlConnection conn = new SqlConnection("server=127.0.0.1;uid=userID;pwd=password;database=databaseName");
        string cmdText = "SELECT * FROM TableName";
        SqlDataAdapter adp = new SqlDataAdapter(cmdText, conn);
        DataSet ds = new DataSet();
        adp.Fill(ds);

        this.gridView1.DataSource = ds.Tables[0];
        this.gridView1.DataBind();
        */
    }

    void AddData()
    {
        /****Access数据库 - 添加数据
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;data source=" + Server.MapPath("db.mdb"));
        string cmdText = "INSERT INTO TableName(FiledName1,FieldName2)VALUES(@FiledName1,@FieldName2)";
        OleDbCommand cmdObj = new OleDbCommand(cmdText, conn);

        cmdObj.Parameters.Add(new OleDbParameter("@FiledName1", OleDbType.VarChar));
        cmdObj.Parameters["@FiledName1"].Value = "Data_FieldName1";

        cmdObj.Parameters.Add(new OleDbParameter("@FiledName2", OleDbType.VarChar));
        cmdObj.Parameters["@FiledName2"].Value = "Data_FieldName2";

        conn.Open();
        cmdObj.ExecuteNonQuery();
        conn.Close();
        */
    }

    void UpdateData()
    {
        /****Access数据库 - 更新数据
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;data source=" + Server.MapPath("db.mdb"));
        string cmdText = "UPDATE TableName SET FiledName1=@FieldName,FieldName2=@FieldName2";
        OleDbCommand cmdObj = new OleDbCommand(cmdText, conn);

        cmdObj.Parameters.Add(new OleDbParameter("@FiledName1", OleDbType.VarChar));
        cmdObj.Parameters["@FiledName1"].Value = "Data_FieldName1";

        cmdObj.Parameters.Add(new OleDbParameter("@FiledName2", OleDbType.VarChar));
        cmdObj.Parameters["@FiledName2"].Value = "Data_FieldName2";

        conn.Open();
        cmdObj.ExecuteNonQuery();
        conn.Close();
        */
    }

    void DeleteData()
    {
        /****Access数据库 - 删除数据
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;data source=" + Server.MapPath("db.mdb"));
        string cmdText = "DELETE * FROM TableName WHERE KeyFiledName=@KeyFieldName";
        OleDbCommand cmdObj = new OleDbCommand(cmdText, conn);

        cmdObj.Parameters.Add(new OleDbParameter("@KeyFiledName", OleDbType.VarChar));
        cmdObj.Parameters["@KeyFiledName"].Value = "KeyValue1";

        conn.Open();
        cmdObj.ExecuteNonQuery();
        conn.Close();
        */
    }
    
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
</head>
<body>
<form id="form1" runat="server">

<asp:GridView ID="gridView1" runat="server"></asp:GridView>

</form>
</body>
</html>