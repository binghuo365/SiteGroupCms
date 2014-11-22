/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序作者: SiteGroupCms高伟 254850396#qq.com
 */

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web;
namespace SiteGroupCms.DBUtility
{
    /// <summary>
    /// 处理表单中的数据，提供数据库添加、修改的通用处理过程。
    /// 如果提交的数据被委托验证器认为无效则不作任何动作，否则操作完后引发操作完成事件。
    /// </summary>
    public class WebFormHandler
    {
        /// <summary>
        /// 将需要绑定的控件以object的形式存储在此数组中。
        /// </summary>
        protected System.Collections.ArrayList alBinderItems = new System.Collections.ArrayList(8);
        /// <summary>
        /// 数据库连接，提供数据访问层的操作。
        /// </summary>
        protected SiteGroupCms.DBUtility.DbOperHandler doh;

        /// <summary>
        /// 用于存放从数据库中取出的数据记录。
        /// </summary>
        protected DataTable myDt;
        //指示处理的提交数据是否通过验证

        /// <summary>
        /// 表示当前的操作类型，添加或修改，默认为添加。
        /// </summary>
        private OperationType _mode = OperationType.Add;
        /// <summary>
        /// 指定当前的操作类型，可以指定为添加或修改。
        /// </summary>
        public OperationType Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                if (value == OperationType.Add)
                {
                    btnSubmit.Text = AddText;
                }
                else if (value == OperationType.Modify)
                {
                    btnSubmit.Text = ModifyText;
                }
                else
                {
                    btnSubmit.Text = UnknownText;
                }
            }
        }

        /// <summary>
        /// 提交按钮在添加状态下显示的文本。
        /// </summary>
        public string AddText = "确认";
        /// <summary>
        /// 提交按钮在修改状态下显示的文本。
        /// </summary>
        public string ModifyText = "确认";

        /// <summary>
        /// 提交按钮在未知状态下显示的文本。
        /// </summary>
        public string UnknownText = "未知";

        /// <summary>
        /// 从数据库中取数据时的条件表达式。
        /// </summary>
        private string _conditionExpress = string.Empty;
        /// <summary>
        /// 存储取得数据的表名称。
        /// </summary>
        private string _tableName = string.Empty;

        /// <summary>
        /// 用于存储表单中的提交按钮的引用。
        /// </summary>
        protected System.Web.UI.WebControls.Button btnSubmit = null;

        /// <summary>
        /// 指定取得数据的表名称。
        /// </summary>
        public string TableName
        {
            set { _tableName = value; }
            get { return _tableName; }
        }

        /// <summary>
        /// 指定从数据库中取数据时的条件表达式。
        /// </summary>
        public string ConditionExpress
        {
            set { _conditionExpress = value; }
            get { return _conditionExpress; }
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_doh">数据库操作对象。</param>
        /// <param name="_table">使用的数据表的名称。</param>
        /// <param name="_btn">表单中的提交按钮。</param>
        public WebFormHandler(SiteGroupCms.DBUtility.DbOperHandler _doh, string _table, Button _btn)
        {
            TableName = _table;
            doh = _doh;
            btnSubmit = _btn;
            this.CheckArgs();
            if (!btnSubmit.Page.IsPostBack && this.Mode == OperationType.Add) this.BindWhenAdd();
            if (!btnSubmit.Page.IsPostBack && this.Mode == OperationType.Modify) this.BindWhenModify();
            btnSubmit.Click += new EventHandler(ProcessTheForm);
            btnSubmit.Page.PreRender += new EventHandler(this.Page_PreRender);
        }
        /// <summary>
        /// 增加数据时候预先将值填充到表单中。
        /// </summary>
        public void BindWhenAdd()
        {
            this.OnBindBeforeAddOk(System.EventArgs.Empty);
        }
        /// <summary>
        /// 需要修改数据时取出数据库中的记录填充到表单中。
        /// </summary>
        public void BindWhenModify()
        {
            if (alBinderItems.Count == 0) return;
            BinderItem bi;
            int i = 0;

            StringBuilder sbSqlCmd = new StringBuilder("SELECT TOP 1 ");
            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                sbSqlCmd.Append(bi.field);
                sbSqlCmd.Append(",");
            }
            sbSqlCmd.Remove(sbSqlCmd.Length - 1, 1);//去掉多余的一个逗号
            sbSqlCmd.Append(" from ");
            sbSqlCmd.Append(TableName);
            sbSqlCmd.Append(" where 1=1 and ");
            sbSqlCmd.Append(this.ConditionExpress);

            doh.Reset();
            doh.SqlCmd = sbSqlCmd.ToString();
            myDt = doh.GetDataTable();
            //如果指定记录不存在则抛出异常
            if (myDt.Rows.Count == 0) throw new ArgumentException("Record does not exist.");

            DataRow dr = myDt.Rows[0];
            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                bi.SetValue(dr[bi.field].ToString());
            }
            this.OnBindBeforeModifyOk(System.EventArgs.Empty);
        }
        /// <summary>
        /// 当操作类型为添加时将输入添加到数据库中
        /// </summary>
        protected void Add()
        {
            if (!DataValid()) return;
            if (alBinderItems.Count == 0) return;
            BinderItem bi;
            int i = 0;
            doh.Reset();

            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                //如果是单选按钮且没有选中则跳过这个字段
                if (bi.o is MyRadioButton)
                {
                    MyRadioButton mrb = (MyRadioButton)bi.o;
                    if (mrb.rb.Checked == false) continue;
                }

                doh.AddFieldItem(bi.field, bi.GetValue());
            }

            int id = doh.Insert(this.TableName);
            this.OnAddOk(new SiteGroupCms.DBUtility.DbOperEventArgs(id));
        }

        /// <summary>
        /// 当操作类型为修改时将输入更新到数据库中
        /// </summary>
        protected void Update()
        {
            if (!DataValid()) return;
            if (alBinderItems.Count == 0) return;
            BinderItem bi;
            int i = 0;
            doh.Reset();
            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                //如果是单选按钮且没有选中则跳过这个字段
                if (bi.o is MyRadioButton)
                {
                    MyRadioButton mrb = (MyRadioButton)bi.o;
                    if (mrb.rb.Checked == false) continue;
                }

                doh.AddFieldItem(bi.field, bi.GetValue());
            }

            doh.ConditionExpress = this.ConditionExpress;
            doh.Update(this.TableName);
            this.OnModifyOk(System.EventArgs.Empty);
        }

        /// <summary>
        /// 建立文本框控件到数据库字段的绑定
        /// </summary>
        /// <param name="tb">需要绑定的文本框对象。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(TextBox tb, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(tb, field, isStringType));
        }

        /// <summary>
        /// 建立下拉框控件到数据库字段的绑定
        /// </summary>
        /// <param name="ddl">需要绑定的下拉框对象。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(DropDownList ddl, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(ddl, field, isStringType));
        }

        /// <summary>
        /// 建立复选框控件到数据库字段的绑定
        /// </summary>
        /// <param name="cb">需要绑定的复选框对象。</param>
        /// <param name="_value">复选框被选中时对应字段应该填写的值。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(CheckBox cb, string _value, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(new MyCheckBox(cb, _value), field, isStringType));
        }

        /// <summary>
        /// 建立单选框控件到数据库字段的绑定
        /// </summary>
        /// <param name="rb">需要绑定的单选框对象。</param>
        /// <param name="_value">单选框被选中时对应字段应该填写的值。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(RadioButton rb, string _value, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(new MyRadioButton(rb, _value), field, isStringType));
        }

        /// <summary>
        /// 建立Label控件到数据库字段的绑定
        /// </summary>
        /// <param name="lbl">需要绑定的Label对象。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(Label lbl, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(lbl, field, isStringType));
        }

        /// <summary>
        /// 建立Literal控件到数据库字段的绑定
        /// </summary>
        /// <param name="ltl">需要绑定的Literal对象。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(Literal ltl, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(ltl, field, isStringType));
        }

        /// <summary>
        /// 建立字符串变量到数据库字段的绑定。
        /// </summary>
        /// <param name="s">需要绑定的字符串引用。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(ref string s, string field, bool isStringType)
        {

            alBinderItems.Add(new BinderItem(ref s, field, isStringType));
        }

        /// <summary>
        /// 建立一个委托对象到数据库字段的绑定。
        /// </summary>
        /// <param name="action">委托的名称。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        private void AddBind(Action action, string field, bool isStringType)
        {

            alBinderItems.Add(new BinderItem(action, field, isStringType));
        }
        /// <summary>
        /// 建立一个string类型属性到数据库字段的绑定。
        /// </summary>
        /// <param name="_o">属性所在的对象。</param>
        /// <param name="_propertyName">属性的名称。</param>
        /// <param name="field">数据库中对应字段的名称。</param>
        /// <param name="isStringType">是否为字符串性质。</param>
        public void AddBind(object _o, string _propertyName, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(new MyProperty(_o, _propertyName), field, isStringType));
        }

        /// <summary>
        /// 检查是否输入了必须的参数，如表名，按钮对象。
        /// </summary>
        public void CheckArgs()
        {
            if (TableName == string.Empty)
            {
                throw new ArgumentException("None table name is specified!");
            }
            if (btnSubmit == null)
            {
                throw new ArgumentException("None submit button is specified!");
            }
        }
        /// <summary>
        /// 捕获按钮点击事件，处理表单，添加或修改
        /// </summary>
        /// <param name="sender">触发对象。</param>
        /// <param name="e">传递的事件参数。</param>
        private void ProcessTheForm(object sender, System.EventArgs e)
        {
            if (Mode == OperationType.Add)
            {
                this.Add();
            }
            else if (Mode == OperationType.Modify)
            {
                this.Update();
            }
            else
            {
                throw new ArgumentException("Unkown operation type.");
            }
        }
        /// <summary>
        /// 表单验证器，现仅支持一个验证器，委托链长度不得多于一个。
        /// </summary>
        public delegate bool Validator();
        /// <summary>
        /// 存储委托对象。
        /// </summary>
        public Validator validator = null;

        /// <summary>
        /// 使用委托验证器验证传递过来的数据是否合法
        /// </summary>
        /// <returns>提交的数据是否符合用户定义的逻辑。</returns>
        private bool DataValid()
        {
            bool validOk = true;
            if (validator != null)
            {
                validOk = validator();
            }
            return validOk;
        }

        /// <summary>
        /// 添加数据完成事件，当数据被添加到数据库之后触发。
        /// </summary>
        public event System.EventHandler AddOk;
        /// <summary>
        ///添加数据完成事件，当数据被添加到数据库之后触发。
        /// </summary>
        /// <param name="e">添加完成的事件参数。</param>
        protected virtual void OnAddOk(System.EventArgs e)
        {
            if (AddOk != null)
            {
                AddOk(this, e);
            }
        }

        /// <summary>
        /// 修改数据完成事件，当数据被更新到数据库之后触发。
        /// </summary>
        public event System.EventHandler ModifyOk;
        /// <summary>
        /// 修改完成事件，当数据被更新到数据库之后触发。
        /// </summary>
        /// <param name="e">修改完成的事件参数。</param>
        protected virtual void OnModifyOk(System.EventArgs e)
        {
            if (ModifyOk != null)
            {
                ModifyOk(this, e);
            }
        }
        /// <summary>
        /// 此事件在页面显示前触发。
        /// </summary>
        public event System.EventHandler BindBeforeAddOk;
        /// <summary>
        /// 此事件在页面显示前触发。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected virtual void OnBindBeforeAddOk(System.EventArgs e)
        {
            if (BindBeforeAddOk != null)
            {
                BindBeforeAddOk(this, e);
            }
        }
        /// <summary>
        /// 当修改时，数据库中的字段值会被填充到相应绑定的控件中，此事件在填充完成后，显示到页面之前触发。
        /// </summary>
        public event System.EventHandler BindBeforeModifyOk;
        /// <summary>
        /// 当修改时，数据库中的字段值会被填充到相应绑定的控件中，此事件在填充完成后，显示到页面之前触发。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected virtual void OnBindBeforeModifyOk(System.EventArgs e)
        {
            if (BindBeforeModifyOk != null)
            {
                BindBeforeModifyOk(this, e);
            }
        }

        /// <summary>
        /// 在页面预生成时，提交到浏览器之前，检查是否需要将数据库中的数据填充到表单中。
        /// </summary>
        /// <param name="sender">传递的对象。</param>
        /// <param name="e">传递的事件参数。</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            if (this.Mode == OperationType.Add && !btnSubmit.Page.IsPostBack)
            {
                BindWhenAdd();
            }
            if (this.Mode == OperationType.Modify && !btnSubmit.Page.IsPostBack)
            {
                BindWhenModify();
            }
        }
    }
    /// <summary>
    /// 定义的委托原型。
    /// </summary>
    public delegate string Action(string s);
    #region 自定义用来支持表单处理的类
    /// <summary>
    /// 每个和数据库字段绑定的对象以BinderItem为容器存储在数组中。
    /// </summary>
    public class BinderItem
    {
        /// <summary>
        /// 每个绑定控件都是以object的形式被存储的。
        /// </summary>
        public object o;
        /// <summary>
        /// 绑定到数据库的字段名称。
        /// </summary>
        public string field;
        /// <summary>
        /// 是否是字符串类型。
        /// </summary>
        public bool isStringType;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_o">需要绑定的控件对象。</param>
        /// <param name="_field">绑定到的数据表字段名称。</param>
        /// <param name="_isStringType">是否是字符串类型。</param>
        public BinderItem(object _o, string _field, bool _isStringType)
        {
            this.o = _o;
            //this.field = ("[" + _field + "]").Replace("[[","[").Replace("]]","]");
            this.field = _field;
            this.isStringType = _isStringType;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_o">需要绑定的string变量引用。</param>
        /// <param name="_field">绑定到的数据表字段名称。</param>
        /// <param name="_isStringType">是否是字符串类型。</param>
        public BinderItem(ref string _o, string _field, bool _isStringType)
        {
            this.o = _o;
            //this.field = ("[" + _field + "]").Replace("[[","[").Replace("]]","]");
            this.field = _field;
            this.isStringType = _isStringType;
        }
        /// <summary>
        /// 根据控件类型获得控件的值。
        /// </summary>
        /// <returns>控件的值。</returns>
        public string GetValue()
        {
            //属性类型
            if (o is MyProperty)
            {
                MyProperty mp = (MyProperty)o;
                System.Type type = mp.po.GetType();
                System.Reflection.PropertyInfo pi = type.GetProperty(mp.propertyName);
                return (string)pi.GetValue(mp.po, null);
                //return type.InvokeMember(mp.propertyName,System.Reflection.BindingFlags.GetProperty,null,mp.po,);
                //return mp.propertyName;
            }

            //字符串类型
            if (o is String)
            {
                string s = (string)o;
                return s;

            }
            //方法委托
            if (o is Action)
            {
                Action action = (Action)o;
                return action("_g_e_t_");
            }
            //下拉框
            if (o is DropDownList)
            {
                DropDownList ddl = (DropDownList)o;
                return ddl.SelectedValue;

            }
            //复选框
            if (o is MyCheckBox)
            {
                MyCheckBox mcb = (MyCheckBox)o;
                if (mcb.cb.Checked) return mcb.Value; else return "0";

            }
            //单选按钮
            if (o is MyRadioButton)
            {
                MyRadioButton mrb = (MyRadioButton)o;
                if (mrb.rb.Checked) return mrb.Value;
            }
            //文本框
            if (o is TextBox)
            {
                TextBox tb = (TextBox)o;
                return tb.Text;
            }
            //Label
            if (o is Label)
            {
                Label lbl = (Label)o;
                return lbl.Text;
            }
            //Literal
            if (o is Literal)
            {
                Literal ltl = (Literal)o;
                return ltl.Text;
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据控件类型设定控件的值
        /// </summary>
        /// <param name="_value">要设定的值。</param>
        public void SetValue(string _value)
        {
            //属性类型
            if (o is MyProperty)
            {
                MyProperty mp = (MyProperty)o;
                System.Type type = mp.po.GetType();
                System.Reflection.PropertyInfo pi = type.GetProperty(mp.propertyName);
                pi.SetValue(mp.po, _value, null);
                //return type.InvokeMember(mp.propertyName,System.Reflection.BindingFlags.GetProperty,null,mp.po,);
                //return mp.propertyName;
            }

            //字符串类型
            if (o is String)
            {
                //this.SetString((string)o,_value);
                //return;
                string s = (string)o;
                s = _value;
                return;
            }
            //委托
            if (o is Action)
            {
                Action action = (Action)o;
                action(_value);
                return;
            }
            //下拉框
            if (o is DropDownList)
            {
                DropDownList ddl = (DropDownList)o;
                ListItem li = ddl.Items.FindByValue(_value);
                if (li != null)
                {
                    ddl.ClearSelection();
                    li.Selected = true;
                }
                return;
            }
            //复选框
            if (o is MyCheckBox)
            {
                MyCheckBox mcb = (MyCheckBox)o;
                if (mcb.Value == _value) mcb.cb.Checked = true;
                return;
            }
            //单选按钮
            if (o is MyRadioButton)
            {
                MyRadioButton mrb = (MyRadioButton)o;
                if (mrb.Value == _value) mrb.rb.Checked = true;
                return;
            }
            //文本框
            if (o is TextBox)
            {
                TextBox tb = (TextBox)o;
                tb.Text = _value;
                return;
            }
            //Label
            if (o is Label)
            {
                Label lbl = (Label)o;
                lbl.Text = _value;
                return;
            }
            //Literal
            if (o is Literal)
            {
                Literal ltl = (Literal)o;
                ltl.Text = _value;
                return;
            }
        }

        private void SetString(string s, string _value)
        {
            s = _value;
        }

    }

    /// <summary>
    /// 扩展复选框，可以使CheckBox具有Value属性，选中时表现。
    /// </summary>
    public class MyCheckBox
    {
        /// <summary>
        /// CheckBox对象。
        /// </summary>
        public CheckBox cb;
        /// <summary>
        /// 选中时应该表现的值。
        /// </summary>
        public string Value;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_cb">CheckBox对象。</param>
        /// <param name="_value">选中时应该表现的值。</param>
        public MyCheckBox(CheckBox _cb, string _value)
        {
            cb = _cb;
            Value = _value;
        }
    }

    /// <summary>
    /// 扩展单选按钮，可以使RadioButton具有Value属性，选中时表现。
    /// </summary>
    public class MyRadioButton
    {
        /// <summary>
        /// RadioButton对象。
        /// </summary>
        public RadioButton rb;
        /// <summary>
        /// 选中时应该表现的值。
        /// </summary>
        public string Value;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_rb">RadioButton对象。</param>
        /// <param name="_value">选中时应该表现的值。</param>
        public MyRadioButton(RadioButton _rb, string _value)
        {
            rb = _rb;
            Value = _value;
        }
    }

    /// <summary>
    /// 扩展属性，存储一个对象引用和需要绑定的属性名称。
    /// </summary>
    public class MyProperty
    {
        /// <summary>
        /// 要绑定属性所在的对象。
        /// </summary>
        public object po;
        /// <summary>
        /// 要绑定的属性名称。
        /// </summary>
        public string propertyName;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_o">要绑定属性所在的对象。</param>
        /// <param name="_propertyName">要绑定的属性名称。</param>
        public MyProperty(object _o, string _propertyName)
        {
            po = _o;
            propertyName = _propertyName;
        }
    }

    #endregion

}
