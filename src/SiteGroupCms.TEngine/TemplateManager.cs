/*****************************************************
 * 本类库的核心系 AderTemplates
 * (C) Andrew Deren 2004
 * http://www.adersoftware.com
 *****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.IO;
using SiteGroupCms.TEngine.Parser.AST;

namespace SiteGroupCms.TEngine
{
    public delegate object TemplateFunction(object[] args);

    public class TemplateManager
    {
        bool silentErrors;

        Dictionary<string, TemplateFunction> functions;

        Dictionary<string, ITagHandler> customTags;

        VariableScope variables;		// current variable scope
        Expression currentExpression;	// current expression being evaluated

        TextWriter writer;				// all output is sent here

        Template mainTemplate;			// main template to execute
        Template currentTemplate;		// current template being executed

        ITemplateHandler handler;		// handler will be set as "this" object

        /// <summary>
        /// create template manager using a template
        /// </summary>
        public TemplateManager(Template template)
        {
            this.mainTemplate = template;
            this.currentTemplate = template;
            this.silentErrors = false;

            Init();
        }

        public static TemplateManager FromString(string template)
        {
            Template itemplate = Template.FromString("", template);
            return new TemplateManager(itemplate);
        }

        public static TemplateManager FromFile(string filename)
        {
            Template template = Template.FromFile("", filename);
            return new TemplateManager(template);
        }

        /// <summary>
        /// handler is used as "this" object, and will receive
        /// before after process message
        /// </summary>
        public ITemplateHandler Handler
        {
            get { return this.handler; }
            set { this.handler = value; }
        }

        /// <summary>
        /// if silet errors is set to true, then any exceptions will not show in the output
        /// If set to false, all exceptions will be displayed.
        /// </summary>
        public bool SilentErrors
        {
            get { return this.silentErrors; }
            set { this.silentErrors = value; }
        }

        private Dictionary<string, ITagHandler> CustomTags
        {
            get
            {
                if (customTags == null)
                    customTags = new Dictionary<string, ITagHandler>(StringComparer.CurrentCultureIgnoreCase);
                return customTags;
            }
        }

        /// <summary>
        /// registers custom tag processor
        /// </summary>
        public void RegisterCustomTag(string tagName, ITagHandler handler)
        {
            CustomTags.Add(tagName, handler);
        }

        /// <summary>
        /// checks whether there is a handler for tagName
        /// </summary>
        public bool IsCustomTagRegistered(string tagName)
        {
            return CustomTags.ContainsKey(tagName);
        }

        /// <summary>
        /// unregistered tagName from custom tags
        /// </summary>
        public void UnRegisterCustomTag(string tagName)
        {
            CustomTags.Remove(tagName);
        }

        /// <summary>
        /// adds template that can be used within execution 
        /// </summary>
        /// <param name="template"></param>
        public void AddTemplate(Template template)
        {
            mainTemplate.Templates.Add(template.Name, template);
        }

        void Init()
        {
            this.functions = new Dictionary<string, TemplateFunction>(StringComparer.InvariantCultureIgnoreCase);

            this.variables = new VariableScope();

            functions.Add("equals", new TemplateFunction(FuncEquals));
            functions.Add("notequals", new TemplateFunction(FuncNotEquals));
            functions.Add("iseven", new TemplateFunction(FuncIsEven));
            functions.Add("isodd", new TemplateFunction(FuncIsOdd));
            functions.Add("isempty", new TemplateFunction(FuncIsEmpty));
            functions.Add("isnotempty", new TemplateFunction(FuncIsNotEmpty));
            functions.Add("isnumber", new TemplateFunction(FuncIsNumber));
            functions.Add("toupper", new TemplateFunction(FuncToUpper));
            functions.Add("tolower", new TemplateFunction(FuncToLower));
            functions.Add("isdefined", new TemplateFunction(FuncIsDefined));
            functions.Add("ifdefined", new TemplateFunction(FuncIfDefined));
            functions.Add("length", new TemplateFunction(FuncLength));
            functions.Add("tolist", new TemplateFunction(FuncToList));
            functions.Add("isnull", new TemplateFunction(FuncIsNull));
            functions.Add("gt", new TemplateFunction(FuncGt));
            functions.Add("lt", new TemplateFunction(FuncLt));
            functions.Add("not", new TemplateFunction(FuncNot));
            functions.Add("div", new TemplateFunction(FuncDiv));//2012-03-15增加

            functions.Add("iif", new TemplateFunction(FuncIif));
            functions.Add("format", new TemplateFunction(FuncFormat));
            functions.Add("trim", new TemplateFunction(FuncTrim));
            functions.Add("filter", new TemplateFunction(FuncFilter));

            functions.Add("compare", new TemplateFunction(FuncCompare));
            functions.Add("or", new TemplateFunction(FuncOr));
            functions.Add("and", new TemplateFunction(FuncAnd));
            functions.Add("comparenocase", new TemplateFunction(FuncCompareNoCase));
            functions.Add("stripnewlines", new TemplateFunction(FuncStripNewLines));
            functions.Add("typeof", new TemplateFunction(FuncTypeOf));
            functions.Add("cint", new TemplateFunction(FuncCInt));
            functions.Add("cdouble", new TemplateFunction(FuncCDouble));
            functions.Add("cdate", new TemplateFunction(FuncCDate));
            functions.Add("cdatestring", new TemplateFunction(FuncCDateString));
            functions.Add("floatstring", new TemplateFunction(FuncFloatString));
            functions.Add("now", new TemplateFunction(FuncNow));
            functions.Add("createtypereference", new TemplateFunction(FuncCreateTypeReference));
            functions.Add("nohtml", new TemplateFunction(FuncNoHTML));
            functions.Add("cutstring", new TemplateFunction(FuncCutString));
            functions.Add("htmlencode", new TemplateFunction(FuncHTMLEncode));
            functions.Add("htmldecode", new TemplateFunction(FuncHTMLDecode));
        }

        #region Functions
        bool CheckArgCount(int count, string funcName, object[] args)
        {
            if (count != args.Length)
            {
                DisplayError(string.Format("Function {0} requires {1} arguments and {2} were passed", funcName, count, args.Length), currentExpression.Line, currentExpression.Col);
                return false;
            }
            else
                return true;
        }

        bool CheckArgCount(int count1, int count2, string funcName, object[] args)
        {
            if (args.Length < count1 || args.Length > count2)
            {
                string msg = string.Format("Function {0} requires between {1} and {2} arguments and {3} were passed", funcName, count1, count2, args.Length);
                DisplayError(msg, currentExpression.Line, currentExpression.Col);
                return false;
            }
            else
                return true;
        }

        object FuncIsEven(object[] args)
        {
            if (!CheckArgCount(1, "iseven", args))
                return null;

            try
            {
                int value = Convert.ToInt32(args[0]);
                return value % 2 == 0;
            }
            catch (FormatException)
            {
                throw new TemplateRuntimeException("IsEven cannot convert parameter to int", currentExpression.Line, currentExpression.Col);
            }
        }

        object FuncIsOdd(object[] args)
        {
            if (!CheckArgCount(1, "isdd", args))
                return null;

            try
            {
                int value = Convert.ToInt32(args[0]);
                return value % 2 == 1;
            }
            catch (FormatException)
            {
                throw new TemplateRuntimeException("IsOdd cannot convert parameter to int", currentExpression.Line, currentExpression.Col);
            }
        }

        object FuncIsEmpty(object[] args)
        {
            if (!CheckArgCount(1, "isempty", args))
                return null;

            string value = args[0].ToString();
            return value.Length == 0;
        }

        object FuncIsNotEmpty(object[] args)
        {
            if (!CheckArgCount(1, "isnotempty", args))
                return null;

            if (args[0] == null)
                return false;

            string value = args[0].ToString();
            return value.Length > 0;
        }


        object FuncEquals(object[] args)
        {
            if (!CheckArgCount(2, "equals", args))
                return null;

            return args[0].Equals(args[1]);
        }
        object FuncGt(object[] args)
        {
            if (!CheckArgCount(2, "gt", args))
                return null;
            //int c1 = Convert.ToInt32(args[0]);
            //int c2 = Convert.ToInt32(args[1]);
            //return c1 > c2;
            IComparable c1 = args[0] as IComparable;
            IComparable c2 = args[1] as IComparable;
            if (c1 == null || c2 == null)
                return false;
            else
                return c1.CompareTo(c2) == 1;
        }

        object FuncLt(object[] args)
        {
            if (!CheckArgCount(2, "lt", args))
                return null;
            //int c1 = Convert.ToInt32(args[0]);
            //int c2 = Convert.ToInt32(args[1]);
            //return c1 < c2;
            IComparable c1 = args[0] as IComparable;
            IComparable c2 = args[1] as IComparable;
            if (c1 == null || c2 == null)
                return false;
            else
                return c1.CompareTo(c2) == -1;
        }

        object FuncNotEquals(object[] args)
        {
            if (!CheckArgCount(2, "notequals", args))
                return null;

            return !args[0].Equals(args[1]);
        }

        object FuncDiv(object[] args)//2012-03-15增加
        {
            if (!CheckArgCount(3, "div", args))
                return null;
            int c1 = Convert.ToInt32(args[0]);
            int c2 = Convert.ToInt32(args[1]);
            int c3 = Convert.ToInt32(args[2]);
            return (c1 % c2 == c3);
        }

        object FuncIsNumber(object[] args)
        {
            if (!CheckArgCount(1, "isnumber", args))
                return null;

            try
            {
                int value = Convert.ToInt32(args[0]);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        object FuncToUpper(object[] args)
        {
            if (!CheckArgCount(1, "toupper", args))
                return null;

            return args[0].ToString().ToUpper();
        }

        object FuncToLower(object[] args)
        {
            if (!CheckArgCount(1, "toupper", args))
                return null;

            return args[0].ToString().ToLower();
        }

        object FuncLength(object[] args)
        {
            if (!CheckArgCount(1, "length", args))
                return null;

            return args[0].ToString().Length;
        }


        object FuncIsDefined(object[] args)
        {
            if (!CheckArgCount(1, "isdefined", args))
                return null;

            return variables.IsDefined(args[0].ToString());
        }

        object FuncIfDefined(object[] args)
        {
            if (!CheckArgCount(2, "ifdefined", args))
                return null;

            if (variables.IsDefined(args[0].ToString()))
            {
                return args[1];
            }
            else
                return string.Empty;
        }

        object FuncToList(object[] args)
        {
            if (!CheckArgCount(2, 3, "tolist", args))
                return null;

            object list = args[0];

            string property;
            string delim;

            if (args.Length == 3)
            {
                property = args[1].ToString();
                delim = args[2].ToString();
            }
            else
            {
                property = string.Empty;
                delim = args[1].ToString();
            }

            if (!(list is IEnumerable))
            {
                throw new TemplateRuntimeException("argument 1 of tolist has to be IEnumerable", currentExpression.Line, currentExpression.Col);
            }

            IEnumerator ienum = ((IEnumerable)list).GetEnumerator();
            StringBuilder sb = new StringBuilder();
            int index = 0;
            while (ienum.MoveNext())
            {
                if (index > 0)
                    sb.Append(delim);

                if (args.Length == 2) // do not evalulate property
                    sb.Append(ienum.Current);
                else
                {
                    sb.Append(EvalProperty(ienum.Current, property));
                }
                index++;
            }

            return sb.ToString();

        }

        object FuncIsNull(object[] args)
        {
            if (!CheckArgCount(1, "isnull", args))
                return null;

            return args[0] == null;
        }

        object FuncNot(object[] args)
        {
            if (!CheckArgCount(1, "not", args))
                return null;

            if (args[0] is bool)
                return !(bool)args[0];
            else
            {
                throw new TemplateRuntimeException("当前'not'的参数不是布尔型", currentExpression.Line, currentExpression.Col);
            }

        }

        object FuncIif(object[] args)
        {
            if (!CheckArgCount(3, "iif", args))
                return null;

            if (args[0] is bool)
            {
                bool test = (bool)args[0];
                return test ? args[1] : args[2];
            }
            else
            {
                throw new TemplateRuntimeException("当前'iif'的参数不是布尔型", currentExpression.Line, currentExpression.Col);
            }
        }

        object FuncFormat(object[] args)
        {
            if (!CheckArgCount(2, "format", args))
                return null;

            string format = args[1].ToString();

            if (args[0] is IFormattable)
                return ((IFormattable)args[0]).ToString(format, null);
            else
                return args[0].ToString();
        }

        object FuncTrim(object[] args)
        {
            if (!CheckArgCount(1, "trim", args))
                return null;

            return args[0].ToString().Trim();
        }

        object FuncFilter(object[] args)
        {
            if (!CheckArgCount(2, "filter", args))
                return null;

            object list = args[0];

            string property;
            property = args[1].ToString();

            if (!(list is IEnumerable))
            {
                throw new TemplateRuntimeException("argument 1 of filter has to be IEnumerable", currentExpression.Line, currentExpression.Col);
            }

            IEnumerator ienum = ((IEnumerable)list).GetEnumerator();
            List<object> newList = new List<object>();

            while (ienum.MoveNext())
            {
                object val = EvalProperty(ienum.Current, property);
                if (val is bool && (bool)val)
                    newList.Add(ienum.Current);
            }

            return newList;

        }

        object FuncCompare(object[] args)
        {
            if (!CheckArgCount(2, "compare", args))
                return null;

            IComparable c1 = args[0] as IComparable;
            IComparable c2 = args[1] as IComparable;
            if (c1 == null || c2 == null)
                return false;
            else
                return c1.CompareTo(c2);
        }

        object FuncOr(object[] args)
        {
            if (!CheckArgCount(2, "or", args))
                return null;

            if (args[0] is bool && args[1] is bool)
                return (bool)args[0] || (bool)args[1];
            else
                return false;
        }

        object FuncAnd(object[] args)
        {
            if (!CheckArgCount(2, "add", args))
                return null;

            if (args[0] is bool && args[1] is bool)
                return (bool)args[0] && (bool)args[1];
            else
                return false;
        }

        object FuncCompareNoCase(object[] args)
        {
            if (!CheckArgCount(2, "compareNoCase", args))
                return null;

            string s1 = args[0].ToString();
            string s2 = args[1].ToString();

            return string.Compare(s1, s2, true) == 0;
        }

        object FuncStripNewLines(object[] args)
        {
            if (!CheckArgCount(1, "StripNewLines", args))
                return null;

            string s1 = args[0].ToString();
            return s1.Replace(Environment.NewLine, " ");

        }

        object FuncTypeOf(object[] args)
        {
            if (!CheckArgCount(1, "TypeOf", args))
                return null;

            return args[0].GetType().Name;

        }

        object FuncCInt(object[] args)
        {
            if (!CheckArgCount(1, "cint", args))
                return null;

            return Convert.ToInt32(args[0]);
        }

        object FuncCDouble(object[] args)
        {
            if (!CheckArgCount(1, "cdouble", args))
                return null;

            return Convert.ToDouble(args[0]);
        }

        object FuncCDate(object[] args)
        {
            if (!CheckArgCount(1, "cdate", args))
                return null;

            return Convert.ToDateTime(args[0]);
        }

        object FuncCDateString(object[] args)
        {
            if (!CheckArgCount(2, "cdatestring", args))
                return null;

            return Convert.ToDateTime(args[0]).ToString(args[1].ToString());
        }
        object FuncFloatString(object[] args)
        {
            if (!CheckArgCount(2, "floatstring", args))
                return null;

            return string.Format(args[1].ToString(), Convert.ToDouble(args[0].ToString())).Replace("￥", "").Replace("$", "").Replace(",", "");
        }

        object FuncNow(object[] args)
        {
            if (!CheckArgCount(0, "now", args))
                return null;

            return DateTime.Now;
        }

        object FuncCreateTypeReference(object[] args)
        {
            if (!CheckArgCount(1, "createtypereference", args))
                return null;

            string typeName = args[0].ToString();


            Type type = System.Type.GetType(typeName, false, true);
            if (type != null)
                return new StaticTypeReference(type);

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in asms)
            {
                type = asm.GetType(typeName, false, true);
                if (type != null)
                    return new StaticTypeReference(type);
            }

            throw new TemplateRuntimeException("Cannot create type " + typeName + ".", currentExpression.Line, currentExpression.Col);
        }
        object FuncNoHTML(object[] args)
        {
            if (!CheckArgCount(1, "nohtml", args))
                return null;

            return SiteGroupCms.Utils.Strings.NoHTML(args[0].ToString());
        }
        object FuncCutString(object[] args)
        {
            if (!CheckArgCount(2, "cutstring", args))
                return null;
            return SiteGroupCms.Utils.Strings.CutString(args[0].ToString(), Convert.ToInt32(args[1].ToString()));
        }
        object FuncHTMLEncode(object[] args)
        {
            if (!CheckArgCount(1, "htmlencode", args))
                return null;
            return SiteGroupCms.Utils.Strings.HtmlEncode(args[0].ToString());
        }
        object FuncHTMLDecode(object[] args)
        {
            if (!CheckArgCount(1, "htmldecode", args))
                return null;
            return SiteGroupCms.Utils.Strings.HtmlDecode(args[0].ToString());
        }
        #endregion

        /// <summary>
        /// gets library of functions that are available
        /// for the tempalte execution
        /// </summary>
        public Dictionary<string, TemplateFunction> Functions
        {
            get { return functions; }
        }

        /// <summary>
        /// sets value for variable called name
        /// </summary>
        public void SetValue(string name, object value)
        {
            variables[name] = value;
        }

        /// <summary>
        /// gets value for variable called name.
        /// Throws exception if value is not found
        /// </summary>
        public object GetValue(string name)
        {
            if (variables.IsDefined(name))
                return variables[name];
            else
                throw new Exception("变量'" + name + "'不存在.");
        }

        /// <summary>
        /// processes current template and sends output to writer
        /// </summary>
        /// <param name="writer"></param>
        public void Process(TextWriter writer)
        {
            this.writer = writer;
            this.currentTemplate = mainTemplate;

            if (handler != null)
            {
                SetValue("this", handler);
                handler.BeforeProcess(this);
            }

            ProcessElements(mainTemplate.Elements);

            if (handler != null)
                handler.AfterProcess(this);
        }

        /// <summary>
        /// processes templates and returns string value
        /// </summary>
        public string Process()
        {
            StringWriter writer = new StringWriter();
            Process(writer);
            return writer.ToString();
        }

        /// <summary>
        /// resets all variables. If TemplateManager is used to 
        /// process template multiple times, Reset() must be 
        /// called prior to Process if varialbes need to be cleared
        /// </summary>
        public void Reset()
        {
            variables.Clear();
        }

        /// <summary>
        /// processes list of elements.
        /// This method is mostly used by extenders of the manager
        /// from custom functions or custom tags.
        /// </summary>
        public void ProcessElements(List<Element> list)
        {
            foreach (Element elem in list)
            {
                ProcessElement(elem);
            }
        }

        protected void ProcessElement(Element elem)
        {
            if (elem is Text)
            {
                Text text = (Text)elem;
                WriteValue(text.Data);
            }
            else if (elem is Expression)
                ProcessExpression((Expression)elem);
            else if (elem is TagIf)
                ProcessIf((TagIf)elem);
            else if (elem is Tag)
                ProcessTag((Tag)elem);
        }

        protected void ProcessExpression(Expression exp)
        {
            object value = EvalExpression(exp);
            WriteValue(value);
        }

        /// <summary>
        /// evaluates expression.
        /// This method is used by TemplateManager extensibility.
        /// </summary>
        public object EvalExpression(Expression exp)
        {
            currentExpression = exp;

            try
            {

                if (exp is StringLiteral)
                    return ((StringLiteral)exp).Content;
                else if (exp is Name)
                {
                    return GetValue(((Name)exp).Id);
                }
                else if (exp is FieldAccess)
                {
                    FieldAccess fa = (FieldAccess)exp;
                    object obj = EvalExpression(fa.Exp);
                    string propertyName = fa.Field=="channelid"?"catalogid":fa.Field;//因为obj里面没有channelid，而对应的是catalgoid
                    return EvalProperty(obj, propertyName);
                }
                else if (exp is MethodCall)
                {
                    MethodCall ma = (MethodCall)exp;
                    object obj = EvalExpression(ma.CallObject);
                    string methodName = ma.Name;

                    return EvalMethodCall(obj, methodName, EvalArguments(ma.Args));
                }
                else if (exp is IntLiteral)
                    return ((IntLiteral)exp).Value;
                else if (exp is DoubleLiteral)
                    return ((DoubleLiteral)exp).Value;
                else if (exp is FCall)
                {
                    FCall fcall = (FCall)exp;
                    if (!functions.ContainsKey(fcall.Name))
                    {
                        string msg = string.Format("函数 '{0}' 未定义.", fcall.Name);
                        throw new TemplateRuntimeException(msg, exp.Line, exp.Col);
                    }

                    TemplateFunction func = functions[fcall.Name];
                    object[] values = EvalArguments(fcall.Args);

                    return func(values);
                }
                else if (exp is StringExpression)
                {
                    StringExpression stringExp = (StringExpression)exp;
                    StringBuilder sb = new StringBuilder();
                    foreach (Expression ex in stringExp.Expressions)
                        sb.Append(EvalExpression(ex));

                    return sb.ToString();
                }
                else if (exp is BinaryExpression)
                    return EvalBinaryExpression(exp as BinaryExpression);
                else if (exp is ArrayAccess)
                    return EvalArrayAccess(exp as ArrayAccess);
                else
                    throw new TemplateRuntimeException("Invalid expression type: " + exp.GetType().Name, exp.Line, exp.Col);

            }
            catch (TemplateRuntimeException ex)
            {
                DisplayError(ex);
                return null;
            }
            catch (Exception ex)
            {
                DisplayError(new TemplateRuntimeException(ex.Message, currentExpression.Line, currentExpression.Col));
                return null;
            }
        }

        protected object EvalArrayAccess(ArrayAccess arrayAccess)
        {
            object obj = EvalExpression(arrayAccess.Exp);

            object index = EvalExpression(arrayAccess.Index);

            if (obj is Array)
            {
                Array array = (Array)obj;
                if (index is Int32)
                {
                    return array.GetValue((int)index);
                }
                else
                    throw new TemplateRuntimeException("Index of array has to be integer", arrayAccess.Line, arrayAccess.Col);
            }
            else
                return EvalMethodCall(obj, "get_Item", new object[] { index });

        }

        protected object EvalBinaryExpression(BinaryExpression exp)
        {
            switch (exp.Operator)
            {
                case SiteGroupCms.TEngine.Parser.TokenKind.OpOr:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        if (Util.ToBool(lhsValue))
                            return true;

                        object rhsValue = EvalExpression(exp.Rhs);
                        return Util.ToBool(rhsValue);
                    }
                case SiteGroupCms.TEngine.Parser.TokenKind.OpAnd:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        if (!Util.ToBool(lhsValue))
                            return false;

                        object rhsValue = EvalExpression(exp.Rhs);
                        return Util.ToBool(rhsValue);

                    }
                case SiteGroupCms.TEngine.Parser.TokenKind.OpIs:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        object rhsValue = EvalExpression(exp.Rhs);

                        return lhsValue.Equals(rhsValue);
                    }
                case SiteGroupCms.TEngine.Parser.TokenKind.OpIsNot:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        object rhsValue = EvalExpression(exp.Rhs);

                        return !lhsValue.Equals(rhsValue);

                    }
                case SiteGroupCms.TEngine.Parser.TokenKind.OpGt:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        object rhsValue = EvalExpression(exp.Rhs);

                        IComparable c1 = lhsValue as IComparable;
                        IComparable c2 = rhsValue as IComparable;
                        if (c1 == null || c2 == null)
                            return false;
                        else
                            return c1.CompareTo(c2) == 1;

                    }
                case SiteGroupCms.TEngine.Parser.TokenKind.OpLt:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        object rhsValue = EvalExpression(exp.Rhs);

                        IComparable c1 = lhsValue as IComparable;
                        IComparable c2 = rhsValue as IComparable;
                        if (c1 == null || c2 == null)
                            return false;
                        else
                            return c1.CompareTo(c2) == -1;

                    }
                case SiteGroupCms.TEngine.Parser.TokenKind.OpGte:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        object rhsValue = EvalExpression(exp.Rhs);

                        IComparable c1 = lhsValue as IComparable;
                        IComparable c2 = rhsValue as IComparable;
                        if (c1 == null || c2 == null)
                            return false;
                        else
                            return c1.CompareTo(c2) >= 0;

                    }
                case SiteGroupCms.TEngine.Parser.TokenKind.OpLte:
                    {
                        object lhsValue = EvalExpression(exp.Lhs);
                        object rhsValue = EvalExpression(exp.Rhs);

                        IComparable c1 = lhsValue as IComparable;
                        IComparable c2 = rhsValue as IComparable;
                        if (c1 == null || c2 == null)
                            return false;
                        else
                            return c1.CompareTo(c2) <= 0;

                    }
                default:
                    throw new TemplateRuntimeException("Operator " + exp.Operator.ToString() + " is not supported.", exp.Line, exp.Col);
            }
        }

        protected object[] EvalArguments(Expression[] args)
        {
            object[] values = new object[args.Length];
            for (int i = 0; i < values.Length; i++)
                values[i] = EvalExpression(args[i]);

            return values;
        }

        protected static object EvalProperty(object obj, string propertyName)
        {
            if (obj is StaticTypeReference)
            {
                Type type = (obj as StaticTypeReference).Type;

                PropertyInfo pinfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Static);
                if (pinfo != null)
                    return pinfo.GetValue(null, null);

                FieldInfo finfo = type.GetField(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetField | BindingFlags.Static);
                if (finfo != null)
                    return finfo.GetValue(null);
                else
                    throw new Exception("Cannot find property or field named '" + propertyName + "' in object of type '" + type.Name + "'");


            }
            else
            {
                PropertyInfo pinfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetProperty | BindingFlags.Instance);

                if (pinfo != null)
                    return pinfo.GetValue(obj, null);

                FieldInfo finfo = obj.GetType().GetField(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.GetField | BindingFlags.Instance);

                if (finfo != null)
                    return finfo.GetValue(obj);
                else
                    throw new Exception("Cannot find property or field named '" + propertyName + "' in object of type '" + obj.GetType().Name + "'");

            }

        }


        protected object EvalMethodCall(object obj, string methodName, object[] args)
        {
            Type[] types = new Type[args.Length];
            for (int i = 0; i < args.Length; i++)
                types[i] = args[i].GetType();

            if (obj is StaticTypeReference)
            {
                Type type = (obj as StaticTypeReference).Type;
                MethodInfo method = type.GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Static,
                    null, types, null);

                if (method == null)
                    throw new Exception(string.Format("method {0} not found for static object of type {1}", methodName, type.Name));

                return method.Invoke(null, args);
            }
            else
            {

                MethodInfo method = obj.GetType().GetMethod(methodName,
                    BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance,
                    null, types, null);

                if (method == null)
                    throw new Exception(string.Format("method {0} not found for object of type {1}", methodName, obj.GetType().Name));

                return method.Invoke(obj, args);
            }
        }


        protected void ProcessIf(TagIf tagIf)
        {
            bool condition = false;

            try
            {
                object value = EvalExpression(tagIf.Test);

                condition = Util.ToBool(value);
            }
            catch (Exception ex)
            {
                DisplayError("Error evaluating condition for if statement: " + ex.Message,
                    tagIf.Line, tagIf.Col);
                return;
            }

            if (condition)
                ProcessElements(tagIf.InnerElements);
            else
                ProcessElement(tagIf.FalseBranch);

        }

        protected void ProcessTag(Tag tag)
        {
            string name = tag.Name.ToLowerInvariant();
            try
            {
                switch (name)
                {
                    case "template":
                        // skip those, because those are processed first
                        break;
                    case "else":
                        ProcessElements(tag.InnerElements);
                        break;
                    case "apply":
                        object val = EvalExpression(tag.AttributeValue("template"));
                        ProcessTemplate(val.ToString(), tag);
                        break;
                    case "foreach":
                        ProcessForEach(tag);
                        break;
                    case "for":
                        ProcessFor(tag);
                        break;
                    case "set":
                        ProcessTagSet(tag);
                        break;
                    default:
                        ProcessTemplate(tag.Name, tag);
                        break;
                }
            }
            catch (TemplateRuntimeException ex)
            {
                DisplayError(ex);
            }
            catch (Exception ex)
            {
                DisplayError("Error executing tag '" + name + "': " + ex.Message, tag.Line, tag.Col);

            }
        }

        protected void ProcessTagSet(Tag tag)
        {
            Expression expName = tag.AttributeValue("name");
            if (expName == null)
            {
                throw new TemplateRuntimeException("Set is missing required attribute: name", tag.Line, tag.Col);
            }

            Expression expValue = tag.AttributeValue("value");
            if (expValue == null)
            {
                throw new TemplateRuntimeException("Set is missing required attribute: value", tag.Line, tag.Col);
            }


            string name = EvalExpression(expName).ToString();
            if (!Util.IsValidVariableName(name))
                throw new TemplateRuntimeException("'" + name + "' is not valid variable name.", expName.Line, expName.Col);

            object value = EvalExpression(expValue);

            this.SetValue(name, value);
        }

        protected void ProcessForEach(Tag tag)
        {
            Expression expCollection = tag.AttributeValue("collection");
            if (expCollection == null)
            {
                throw new TemplateRuntimeException("Foreach is missing required attribute: collection", tag.Line, tag.Col);
            }

            object collection = EvalExpression(expCollection);
            if (!(collection is IEnumerable))
            {
                throw new TemplateRuntimeException("Collection used in foreach has to be enumerable", tag.Line, tag.Col);
            }

            Expression expVar = tag.AttributeValue("var");
            if (expCollection == null)
            {
                throw new TemplateRuntimeException("Foreach is missing required attribute: var", tag.Line, tag.Col);
            }
            object varObject = EvalExpression(expVar);
            if (varObject == null)
                varObject = "foreach";
            string varname = varObject.ToString();

            Expression expIndex = tag.AttributeValue("index");
            string indexname = null;
            if (expIndex != null)
            {
                object obj = EvalExpression(expIndex);
                if (obj != null)
                    indexname = obj.ToString();
            }

            IEnumerator ienum = ((IEnumerable)collection).GetEnumerator();
            int index = 0;
            while (ienum.MoveNext())
            {
                index++;
                object value = ienum.Current;
                variables[varname] = value;
                if (indexname != null)
                    variables[indexname] = index;

                ProcessElements(tag.InnerElements);
            }
        }

        protected void ProcessFor(Tag tag)
        {
            Expression expFrom = tag.AttributeValue("from");
            if (expFrom == null)
            {
                throw new TemplateRuntimeException("For is missing required attribute: start", tag.Line, tag.Col);
            }

            Expression expTo = tag.AttributeValue("to");
            if (expTo == null)
            {
                throw new TemplateRuntimeException("For is missing required attribute: to", tag.Line, tag.Col);
            }

            Expression expIndex = tag.AttributeValue("index");
            if (expIndex == null)
            {
                throw new TemplateRuntimeException("For is missing required attribute: index", tag.Line, tag.Col);
            }

            object obj = EvalExpression(expIndex);
            string indexName = obj.ToString();

            int start = Convert.ToInt32(EvalExpression(expFrom));
            int end = Convert.ToInt32(EvalExpression(expTo));

            for (int index = start; index <= end; index++)
            {
                SetValue(indexName, index);
                //variables[indexName] = index;

                ProcessElements(tag.InnerElements);
            }
        }

        protected void ExecuteCustomTag(Tag tag)
        {
            ITagHandler tagHandler = customTags[tag.Name];

            bool processInnerElements = true;
            bool captureInnerContent = false;

            tagHandler.TagBeginProcess(this, tag, ref processInnerElements, ref captureInnerContent);

            string innerContent = null;

            if (processInnerElements)
            {
                TextWriter saveWriter = writer;

                if (captureInnerContent)
                    writer = new StringWriter();

                try
                {
                    ProcessElements(tag.InnerElements);

                    innerContent = writer.ToString();
                }
                finally
                {
                    writer = saveWriter;
                }
            }

            tagHandler.TagEndProcess(this, tag, innerContent);

        }

        protected void ProcessTemplate(string name, Tag tag)
        {
            if (customTags != null && customTags.ContainsKey(name))
            {
                ExecuteCustomTag(tag);
                return;
            }

            Template useTemplate = currentTemplate.FindTemplate(name);
            if (useTemplate == null)
            {
                string msg = string.Format("Template '{0}' not found", name);
                throw new TemplateRuntimeException(msg, tag.Line, tag.Col);
            }

            // process inner elements and save content
            TextWriter saveWriter = writer;
            writer = new StringWriter();
            string content = string.Empty;

            try
            {
                ProcessElements(tag.InnerElements);

                content = writer.ToString();
            }
            finally
            {
                writer = saveWriter;
            }

            Template saveTemplate = currentTemplate;
            variables = new VariableScope(variables);
            variables["innerText"] = content;

            try
            {
                foreach (TagAttribute attrib in tag.Attributes)
                {
                    object val = EvalExpression(attrib.Expression);
                    variables[attrib.Name] = val;
                }

                currentTemplate = useTemplate;
                ProcessElements(currentTemplate.Elements);
            }
            finally
            {
                variables = variables.Parent;
                currentTemplate = saveTemplate;
            }


        }

        /// <summary>
        /// writes value to current writer
        /// </summary>
        /// <param name="value">value to be written</param>
        public void WriteValue(object value)
        {
            if (value == null)
                writer.Write("[null]");
            else
                writer.Write(value);
        }

        private void DisplayError(Exception ex)
        {
            if (ex is TemplateRuntimeException)
            {
                TemplateRuntimeException tex = (TemplateRuntimeException)ex;
                DisplayError(ex.Message, tex.Line, tex.Col);
            }
            else
                DisplayError(ex.Message, 0, 0);
        }

        private void DisplayError(string msg, int line, int col)
        {
            if (!silentErrors)
                writer.Write("[ERROR ({0}, {1}): {2}]", line, col, msg);
        }

    }
}
